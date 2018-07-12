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
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Util;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;
using Microsoft.CodeAnalysis;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Creators
{
  [Item("ModifyingProductionCodeSolutionCreator", "Creates a ASR solution from production code stored in variable.")]
  [StorableClass]
  public sealed class ModifyingProductionCodeSolutionCreator :  ProductionCodeBasedSolutionCreator {
    private const string RandomParameterName = "Random";
    private const string CreationModificationProbabilityParameterName = "CreationModificationProbability";

    public ILookupParameter<IRandom> RandomParameter {
      get { return (LookupParameter<IRandom>)Parameters[RandomParameterName]; }
    }

    public ILookupParameter<DoubleValue> CreationModificationProbabilityParameter {
      get { return (LookupParameter<DoubleValue>)Parameters[CreationModificationProbabilityParameterName]; }
    }

    [StorableConstructor]
    private ModifyingProductionCodeSolutionCreator(bool deserializing) : base(deserializing) { }

    public ModifyingProductionCodeSolutionCreator()
        : base() {
      Parameters.Add(new LookupParameter<IRandom>(RandomParameterName, "The pseudo random number generator."));
      Parameters.Add(new LookupParameter<DoubleValue>(CreationModificationProbabilityParameterName, "The probability of modifications to the initial solution candidates."));
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new ModifyingProductionCodeSolutionCreator(this, cloner);
    }

    private ModifyingProductionCodeSolutionCreator(ModifyingProductionCodeSolutionCreator original, Cloner cloner)
        : base(original, cloner) {
    }

    protected override IASREncoding CreateSolution (string productionCode, IASRProblemInstance instance) {
      var result = new SyntaxTreeEncoding(productionCode, instance);

      if (RandomParameter.ActualValue.NextDouble() < CreationModificationProbabilityParameter.ActualValue.Value)
        result.SyntaxTree = ModifySyntaxTree(result.SyntaxTree);

      return result;
    }

    private SyntaxTree ModifySyntaxTree (SyntaxTree originalTree) {
      // replace a random statement with another existing and fitting statement
      var statements = StatementExtractionUtility.GetAllStatements (originalTree.GetRoot());
      var replacee = statements[RandomParameter.ActualValue.Next (statements.Length)];

      var fittingStatements = statements.Where(s => s.Kind() == replacee.Kind()).ToArray();
      if (!fittingStatements.Any())
        return originalTree;

      var replacement = fittingStatements[RandomParameter.ActualValue.Next (fittingStatements.Length)];
      if (replacement.Equals (replacee))
        return originalTree;

      var modifySyntaxTree = originalTree.GetRoot().ReplaceNode(replacee, replacement).SyntaxTree;
      return modifySyntaxTree;
    }
  }
}