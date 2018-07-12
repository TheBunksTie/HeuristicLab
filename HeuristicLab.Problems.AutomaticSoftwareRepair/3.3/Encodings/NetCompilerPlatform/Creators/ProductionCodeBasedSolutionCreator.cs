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
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Creators
{
  [Item("ProductionCodeBasedSolutionCreator", "A base class for creating solutions based on a textual representation of source code.")]
  [StorableClass]
  public abstract class ProductionCodeBasedSolutionCreator :  ASRCreator {
    [StorableConstructor]
    protected ProductionCodeBasedSolutionCreator (bool deserializing) : base (deserializing) { }

    protected ProductionCodeBasedSolutionCreator(ProductionCodeBasedSolutionCreator original, Cloner cloner)
        : base(original, cloner) {
    }

    public ProductionCodeBasedSolutionCreator()
        : base() {
    }
 
    public override IOperation InstrumentedApply() {
      ASRSolutionParameter.ActualValue = CreateSolution(ProblemInstance.ProductionCode.Value, ProblemInstance);
      return base.InstrumentedApply();
    }

    protected abstract IASREncoding CreateSolution (string productionCode, IASRProblemInstance instance);
  }
}