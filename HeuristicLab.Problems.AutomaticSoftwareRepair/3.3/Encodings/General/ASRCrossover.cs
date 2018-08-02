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
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General {
  [Item("ASRCrossover", "Crosses ASR solutions.")]
  [StorableClass]
  public abstract class ASRCrossover : ASROperator, IASRCrossover {
    private const string ParentsParameterName = "Parents";
    private const string ChildParameterName = "Child";
    private const string SolutionParameterName = "ASRSolution";

    public ILookupParameter<ItemArray<IASREncoding>> ParentsParameter {
      get { return (ScopeTreeLookupParameter<IASREncoding>)Parameters[ParentsParameterName]; }
    }

    public ILookupParameter<IASREncoding> ChildParameter {
      get { return (ILookupParameter<IASREncoding>)Parameters[ChildParameterName]; }
    }

    [StorableConstructor]
    protected ASRCrossover(bool deserializing) : base(deserializing) { }

    public ASRCrossover()
        : base() {
      Parameters.Add(new ScopeTreeLookupParameter<IASREncoding>(ParentsParameterName, "The list of all current ASR solutions which could be potentials parents for crossover."));
      ParentsParameter.ActualName = SolutionParameterName;
      Parameters.Add(new LookupParameter<IASREncoding>(ChildParameterName, "The child ASR solution resulting from the crossover."));
      ChildParameter.ActualName = SolutionParameterName;
    }

    protected ASRCrossover(ASRCrossover original, Cloner cloner)
        : base(original, cloner) {
    }
  }
}