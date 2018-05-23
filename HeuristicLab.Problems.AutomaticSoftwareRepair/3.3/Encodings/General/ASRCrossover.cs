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
  public abstract class ASRCrossover : ASRSyntaxTreeOperator, IASRCrossover {
    private const string parentsParameterName = "Parents";
    private const string childParameterName = "Child";

    public ILookupParameter<ItemArray<IASREncoding>> ParentsParameter {
      get { return (ScopeTreeLookupParameter<IASREncoding>)Parameters[parentsParameterName]; }
    }

    public ILookupParameter<IASREncoding> ChildParameter {
      get { return (ILookupParameter<IASREncoding>)Parameters[childParameterName]; }
    }

    [StorableConstructor]
    protected ASRCrossover(bool deserializing) : base(deserializing) { }

    public ASRCrossover()
        : base() {
      Parameters.Add(new ScopeTreeLookupParameter<IASREncoding>(parentsParameterName, "The parent ASR solutions which should be crossed."));
      ParentsParameter.ActualName = "ARSSolutionPrograms";
      Parameters.Add(new LookupParameter<IASREncoding>(childParameterName, "The child ASR solution resulting from the crossover."));
      ChildParameter.ActualName = "IStochasticOperator";
    }

    protected ASRCrossover(ASRCrossover original, Cloner cloner)
        : base(original, cloner) {
    }
    
    protected abstract IASREncoding Crossover(IRandom random, IASREncoding parent1, IASREncoding parent2);

    public override IOperation InstrumentedApply() {
      var parents = new ItemArray<IASREncoding>(ParentsParameter.ActualValue.Length);
      for (var i = 0; i < ParentsParameter.ActualValue.Length; i++) {
        parents[i] = ParentsParameter.ActualValue[i];
      }
      ParentsParameter.ActualValue = parents;

      ChildParameter.ActualValue = Crossover(RandomParameter.ActualValue, parents[0], parents[1]);

      return base.InstrumentedApply();
    }
  }
}