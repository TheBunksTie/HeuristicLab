#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2018 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit;
using NUnit.Framework;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Evaluators {
  /// <summary>
  /// An operator which evaluates ASR solutions given ASR solutions against a given test suite using NUnit.
  /// </summary>
  [Item("ASRNUnitBasedEvaluator", "An operator which evaluates ASR solutions given ASR solutions against a given test suite using NUnit.")]
  [StorableClass]
  public sealed class ASRNUnitBasedEvaluator : ASRTestSuiteBasedEvaluator
  {
    private const string c_evaluationClassTemplate = @"using System;
using NUnit;
using NUnit.Framework;

namespace GeneticImprovement.Evaluation
{{
  public class EvaluationClass
  {{
    // --------------------- test code ----------------------------

    {0}

     // --------------------- production code ----------------------

    {1}

  }}
}}";

    private static readonly MetadataReference coreLibReference = MetadataReference.CreateFromFile (typeof (object).Assembly.Location);
    private static readonly MetadataReference nunitReference = MetadataReference.CreateFromFile (typeof (TestFixtureAttribute).Assembly.Location);

    private static readonly Dictionary<string, object> testSettings = new Dictionary<string, object>();
    private static readonly NullTestListener nullTestListener = new NullTestListener ();
    private static readonly DefaultTestAssemblyBuilder defaultTestAssemblyBuilder = new DefaultTestAssemblyBuilder ();

    [StorableConstructor]
    private ASRNUnitBasedEvaluator(bool deserializing) : base(deserializing) { }
    private ASRNUnitBasedEvaluator(ASRNUnitBasedEvaluator original, Cloner cloner) : base(original, cloner) { }
    public ASRNUnitBasedEvaluator() : base() {

      testSettings.Add(FrameworkPackageSettings.NumberOfTestWorkers, 0);
      testSettings.Add(FrameworkPackageSettings.InternalTraceLevel, "Off");
    }

    public override IDeepCloneable Clone (Cloner cloner) {
      return new ASRNUnitBasedEvaluator(this, cloner);
    }

    protected override double Evaluate (string productionCode, string testCode) {
      
      var completeCode = string.Format (c_evaluationClassTemplate, testCode, productionCode);
      var tree = CSharpSyntaxTree.ParseText (completeCode);

      var evaluationAssembly = CompileToAssembly (tree);
      if (evaluationAssembly == null)
        return -1;

      // run tests from test code assembly and calculate fitness for current candidate
      var nUnitTestAssemblyRunner = new NUnitTestAssemblyRunner (defaultTestAssemblyBuilder);
      nUnitTestAssemblyRunner.Load (evaluationAssembly, testSettings);

      var testRunResult = nUnitTestAssemblyRunner.Run (nullTestListener, TestFilter.Empty);
      var solutionCandidateQuality = CalculateFitness (testRunResult);

      return solutionCandidateQuality;
    }

    private double CalculateFitness (ITestResult testRunResult)
    {
      const int positiveWeight = 1;
      const int negativeWeight = 10;

      var positiveTestCount = testRunResult.PassCount;
      var negativeTestCount = testRunResult.FailCount + testRunResult.InconclusiveCount + testRunResult.SkipCount;

      return (positiveWeight * positiveTestCount - negativeWeight * negativeTestCount) - testRunResult.Duration;
    }

    private Assembly CompileToAssembly (SyntaxTree tree)
    {
      var comp = CSharpCompilation.Create (
          "ASRNUnitBasedEvaluator_" + Guid.NewGuid ().ToString ("D"),
          syntaxTrees: new[] { tree },
          references: new[] { coreLibReference, nunitReference },
          options: new CSharpCompilationOptions (OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release));

      var evaluationAssembly = EmitToAssembly (comp);
      return evaluationAssembly;
    }

    private Assembly EmitToAssembly (Compilation compilation)
    {
      using (var compilationStream = new MemoryStream ())
      {
        var result = compilation.Emit (compilationStream);

        if (!result.Success)
            //var failures = result.Diagnostics.Where (d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error);
          return null;

        compilationStream.Seek (0, SeekOrigin.Begin);
        return Assembly.Load (compilationStream.ToArray ());
      }

    }

    private class NullTestListener : ITestListener
    {
      public void TestStarted (ITest test) { }
      public void TestFinished (ITestResult result) { }
      public void TestOutput (TestOutput output) { }
    }
  }
}