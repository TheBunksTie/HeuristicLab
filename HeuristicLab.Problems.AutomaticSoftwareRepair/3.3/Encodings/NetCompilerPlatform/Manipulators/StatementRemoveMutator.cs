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
using Microsoft.CodeAnalysis;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Manipulators
{
  [Item ("StatementRemoveMutator", "A mutation operator which randomly removes a statement from the syntax tree.")]
  [StorableClass]
  public sealed class StatementRemoveMutator : SyntaxTreeManipulator {

    [StorableConstructor]
    private StatementRemoveMutator (bool deserializing) : base(deserializing) { }

    public StatementRemoveMutator  ()
        : base () {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new StatementRemoveMutator (this, cloner);
    }

    private StatementRemoveMutator (StatementRemoveMutator  original, Cloner cloner)
        : base(original, cloner) {
    }

    protected override SyntaxTreeEncoding ApplyMutation (IRandom random, SyntaxTreeEncoding individual) {
      var statements = GetAllStatements (individual.SyntaxTree.GetRoot());
      if (statements.Length == 0)
        return individual;

      var removee = statements[random.Next (statements.Length)];

      var mutatedSyntaxTree = individual.SyntaxTree.GetRoot().RemoveNode(removee, SyntaxRemoveOptions.KeepNoTrivia).SyntaxTree;

      individual.SyntaxTree = mutatedSyntaxTree;

      return individual;
    }
  }
}