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
using System.Linq;
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
  [Item("ASRNUnitBasedEvaluator", "An operator which evaluates ASR solutions given ASR as source code against a given test suite using NUnit.")]
  [StorableClass]
  public sealed class ASRNUnitBasedEvaluator : ASREvaluator
  {
    private const string c_evaluationClassTemplate = @"using System;
using System.Linq;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;

namespace ASRNUnitBasedEvaluator.Evaluation
{{
  public class EvaluationClass
  {{
    // --------------------- test code ----------------------------

    {0}

     // --------------------- production code ----------------------

    {1}

  }}
}}";

    // TODO RequiredAssembliesSection in Instance File und alle dort spezifizierten Asms. dann als Metadataref mitgeben
    private static readonly MetadataReference coreLibReference = MetadataReference.CreateFromFile (typeof (object).Assembly.Location);
    private static readonly MetadataReference nunitReference = MetadataReference.CreateFromFile (typeof (TestFixtureAttribute).Assembly.Location);
    private static readonly MetadataReference enumerableReference = MetadataReference.CreateFromFile (typeof (Enumerable).Assembly.Location);

    private readonly Dictionary<string, object> testSettings = new Dictionary<string, object>();
    private static readonly DefaultTestAssemblyBuilder defaultTestAssemblyBuilder = new DefaultTestAssemblyBuilder ();
    private SyntaxTree originalProductionCodeTree;

    private SyntaxTree OriginalProductionCodeTree {
      get { return originalProductionCodeTree ?? (originalProductionCodeTree = CSharpSyntaxTree.ParseText (ProblemInstance.ProductionCode.Value)); }
    }

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

    protected override double Evaluate (string solutionCandidateProductionCode, string correctessSpecification) {
      
      var completeEvaluationCode = string.Format (c_evaluationClassTemplate, correctessSpecification, solutionCandidateProductionCode);
      var solutionCandidateSyntaxTree = CSharpSyntaxTree.ParseText (completeEvaluationCode);

      var evaluationAssembly = CompileToAssembly (solutionCandidateSyntaxTree);
      if (evaluationAssembly == null)
        return 0;

      // run tests from test code assembly and calculate fitness for current candidate
      var nUnitTestAssemblyRunner = new NUnitTestAssemblyRunner (defaultTestAssemblyBuilder);
      nUnitTestAssemblyRunner.Load (evaluationAssembly, testSettings);

      var diagnosticTestListener = new DiagnosticTestListener();

      nUnitTestAssemblyRunner.Run (diagnosticTestListener, TestFilter.Empty);

      var solutionCandidateQuality = CalculateFitness (diagnosticTestListener);

      return solutionCandidateQuality;
    }

    protected override double CalculateSimilarity (string currentSolutionProgram, string correctSolutionValue) {
      return 0;
    }

    private double CalculateFitness (IDetailedTestResult testRunResult)
    {
      const int positiveWeight = 1;
      const int negativeWeight = 10;


      var fitnessValue = 0L;

      foreach (var testName in ProblemInstance.PassingTests) {
        if (testRunResult.PassedTests.Contains(testName.Value)) {
          fitnessValue += positiveWeight;          
        }
      }

      foreach (var testName in ProblemInstance.FailingTests) {
        if (testRunResult.PassedTests.Contains(testName.Value)) {
          fitnessValue += negativeWeight;          
        }
      }

      return fitnessValue;
    }

    private Assembly CompileToAssembly (SyntaxTree tree)
    {
      var comp = CSharpCompilation.Create (
          "ASRNUnitBasedEvaluator_" + Guid.NewGuid ().ToString ("D"),
          syntaxTrees: new[] { tree },
          references: new[] { coreLibReference, nunitReference,
                                enumerableReference ,
                            },
          options: new CSharpCompilationOptions (OutputKind.DynamicallyLinkedLibrary));

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

    private class DiagnosticTestListener : ITestListener, IDetailedTestResult
    {
      public DiagnosticTestListener () {
        PassedTests = new HashSet<string>();
      }

      public void TestStarted (ITest test) { }
      public void TestFinished (ITestResult result) {
        var testPassed = result.PassCount >= 1;
        if (testPassed)
          PassedTests.Add (result.Test.Name);

      }

      public void TestOutput (TestOutput output) { }

      public ISet<string> PassedTests { get; private set; }
    }

    private interface IDetailedTestResult {
      ISet<string> PassedTests { get; }
    }
  }
}
