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
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Creators {
  [Item("NonModifyingProductionCodeSolutionCreator", "Creates an ASR solution from production code stored in variable.")]
  [StorableClass]
  public sealed class NonModifyingProductionCodeSolutionCreator :  ProductionCodeBasedSolutionCreator {

    [StorableConstructor]
    private NonModifyingProductionCodeSolutionCreator(bool deserializing) : base(deserializing) { }

    public NonModifyingProductionCodeSolutionCreator()
        : base() {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new NonModifyingProductionCodeSolutionCreator(this, cloner);
    }

    private NonModifyingProductionCodeSolutionCreator(NonModifyingProductionCodeSolutionCreator original, Cloner cloner)
        : base(original, cloner) {
    }
 
    protected override IASREncoding CreateSolution (string productionCode, IASRProblemInstance instance) {
      var result = new SyntaxTreeEncoding(productionCode, instance);
      return result;
    }
  }
}