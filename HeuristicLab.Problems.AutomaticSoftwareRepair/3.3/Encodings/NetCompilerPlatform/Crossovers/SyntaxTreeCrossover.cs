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
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Crossovers
{
  [Item("SyntaxTreeCrossover", "Crosses ASR solutions encoded as .NET Compiler Platform Syntax Trees.")]
  [StorableClass]
  public abstract class SyntaxTreeCrossover : ASRCrossover { 
    [StorableConstructor]
    protected SyntaxTreeCrossover(bool deserializing) : base(deserializing) { }

    public SyntaxTreeCrossover()
        : base() {
    }

    protected SyntaxTreeCrossover(SyntaxTreeCrossover original, Cloner cloner)
        : base(original, cloner) {
    }
    
    protected abstract IASREncoding Crossover(IRandom random, SyntaxTreeEncoding parent1, SyntaxTreeEncoding parent2);

    public override IOperation InstrumentedApply() {

      var parents = new ItemArray<IASREncoding> (ParentsParameter.ActualValue.Length);
      for (var i = 0; i < ParentsParameter.ActualValue.Length; i++) {
        parents[i] = ParentsParameter.ActualValue[i];
      }

      ParentsParameter.ActualValue = parents;

      ChildParameter.ActualValue = Crossover(RandomParameter.ActualValue, parents[0] as SyntaxTreeEncoding, parents[1] as SyntaxTreeEncoding);

      return base.InstrumentedApply();
    }
  }
}