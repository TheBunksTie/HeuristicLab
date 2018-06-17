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

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Crossovers
{
  [Item("NullCrossover", "A null crossover strategy operator wich just returns parent 1 syntax tree.")]
  [StorableClass]
  public sealed class NullCrossover : SyntaxTreeCrossover {
    [StorableConstructor]
    private NullCrossover (bool deserializing) : base(deserializing) { }

    public NullCrossover ()
        : base() {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new NullCrossover (this, cloner);
    }

    private NullCrossover (NullCrossover original, Cloner cloner)
        : base(original, cloner) {
    }

    protected override SyntaxTreeEncoding Crossover (IRandom random, SyntaxTreeEncoding parent1, SyntaxTreeEncoding parent2) {
      return parent1;
    }
  }
}