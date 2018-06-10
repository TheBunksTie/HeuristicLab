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

using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Operators;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings {
  [Item("ASROperator", "Represents an ASR operator.")]
  [StorableClass]
  public abstract class ASROperator : InstrumentedOperator, IASROperator {
    private const string ProblemInstanceParameterName = "ProblemInstance";

    public ILookupParameter<IASRProblemInstance> ProblemInstanceParameter {
      get { return (LookupParameter<IASRProblemInstance>)Parameters[ProblemInstanceParameterName]; }
    }

   

    public IASRProblemInstance ProblemInstance {
      get { return ProblemInstanceParameter.ActualValue; }
    }

    [StorableConstructor]
    protected ASROperator(bool deserializing) : base(deserializing) { }

    public ASROperator()
        : base() {
      Parameters.Add(new LookupParameter<IASRProblemInstance>(ProblemInstanceParameterName, "The ASR problem instance"));
    }

    protected ASROperator(ASROperator original, Cloner cloner)
        : base(original, cloner) {
    }

  }
}
