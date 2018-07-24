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
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Util;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using IOperation = HeuristicLab.Core.IOperation;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Crossovers {
  [Item("SyntaxTreeCrossover", "Crosses ASR solutions encoded as .NET Compiler Platform syntax trees.")]
  [StorableClass]
  public abstract class SyntaxTreeCrossover : ASRCrossover {
    private const string RandomParameterName = "Random";
    private const string ExecutionProbabilityParameterName = "ExecutionProbability";

    public ILookupParameter<IRandom> RandomParameter {
      get { return (LookupParameter<IRandom>) Parameters[RandomParameterName]; }
    }

    public ValueParameter<PercentValue> ExecutionProbabilityParameter {
      get { return (ValueParameter<PercentValue>) Parameters[ExecutionProbabilityParameterName]; }
    }

    [StorableConstructor]
    protected SyntaxTreeCrossover(bool deserializing) : base(deserializing) { }

    public SyntaxTreeCrossover()
        : base() {
      Parameters.Add(new LookupParameter<IRandom>(RandomParameterName,  "The pseudo random number generator which should be used for symbolic expression tree operators."));
      Parameters.Add(new ValueParameter<PercentValue>(ExecutionProbabilityParameterName,  "The probability whith which the crossover operator is applied.", new PercentValue(0.1)));
    }

    protected SyntaxTreeCrossover(SyntaxTreeCrossover original, Cloner cloner)
        : base(original, cloner) {
    }
    
    public override IOperation InstrumentedApply() {

      var parents = new ItemArray<IASREncoding> (ParentsParameter.ActualValue.Length);
      for (var i = 0; i < ParentsParameter.ActualValue.Length; i++) {
        parents[i] = ParentsParameter.ActualValue[i];
      }

      ParentsParameter.ActualValue = parents;

      if (RandomParameter.ActualValue.NextDouble() > ExecutionProbabilityParameter.Value.Value) {
        ChildParameter.ActualValue = parents[0];
        return base.InstrumentedApply();
      }

      var crossedIndividual = Crossover(RandomParameter.ActualValue, parents[0] as SyntaxTreeEncoding, parents[1] as SyntaxTreeEncoding);
      ChildParameter.ActualValue = crossedIndividual;

      return base.InstrumentedApply();
    }

    protected StatementSyntax[] GetAllStatements (SyntaxNode rootNode, Func<StatementSyntax, bool> whereCondition = null) {
      return StatementExtractionUtility.GetAllStatements (rootNode, whereCondition);
    }

    protected abstract SyntaxTreeEncoding Crossover (IRandom random, SyntaxTreeEncoding parent1, SyntaxTreeEncoding parent2);
  }
}