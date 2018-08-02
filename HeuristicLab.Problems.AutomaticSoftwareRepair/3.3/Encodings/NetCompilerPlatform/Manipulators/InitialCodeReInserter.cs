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

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Manipulators
{
  [Item ("InitialCodeReInserter", "A mutation operator which inserts the original buggy production code (Arcuri/Yao. \"A novel co-evolutionary approach to automatic software bug fixing.\" In: IEEE Congress on Evolutionary Computation. IEEE, 2008,).")]
  [StorableClass]
  public sealed class InitialCodeReInserter : SyntaxTreeManipulator {

    [StorableConstructor]
    private InitialCodeReInserter(bool deserializing) : base(deserializing) { }

    public InitialCodeReInserter ()
        : base () {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new InitialCodeReInserter(this, cloner);
    }

    private InitialCodeReInserter(InitialCodeReInserter original, Cloner cloner)
        : base(original, cloner) {
    }

    protected override SyntaxTreeEncoding ApplyManipulation (SyntaxTreeEncoding individual) {
      var originalProductionCodeSyntaxTree = new SyntaxTreeEncoding (ProblemInstance.ProductionCode.Value, ProblemInstance);
      return originalProductionCodeSyntaxTree;
    }
  }
}