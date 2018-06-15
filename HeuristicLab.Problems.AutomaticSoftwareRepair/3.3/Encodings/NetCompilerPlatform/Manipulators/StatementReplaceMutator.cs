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
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using Microsoft.CodeAnalysis;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Manipulators
{
  [Item ("StatementReplaceMutator", "A mutation operator which randomly replaces an expression with another one from the same syntax tree.")]
  [StorableClass]
  public sealed class StatementReplaceMutator : SyntaxTreeManipulator {

    [StorableConstructor]
    private StatementReplaceMutator(bool deserializing) : base(deserializing) { }

    public StatementReplaceMutator ()
        : base () {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new StatementReplaceMutator(this, cloner);
    }

    private StatementReplaceMutator(StatementReplaceMutator original, Cloner cloner)
        : base(original, cloner) {
    }

    protected override SyntaxTreeEncoding ApplyMutation (IRandom random, SyntaxTreeEncoding individual) {
      var statements = GetAllStatements(individual.SyntaxTree.GetRoot());
      var replacee = statements[random.Next (statements.Length)];

      var fittingStatements = statements.Where(s => s.Kind() == replacee.Kind()).ToArray();
      if (!fittingStatements.Any())
        return individual;

      var replacement = fittingStatements[random.Next (fittingStatements.Length)];
      if (replacement.Equals (replacee))
        return individual;

      var mutatedSyntaxTree = individual.SyntaxTree.GetRoot().ReplaceNode(replacee, replacement).SyntaxTree;

      individual.SyntaxTree = mutatedSyntaxTree;

      return individual;
    }
  }
}