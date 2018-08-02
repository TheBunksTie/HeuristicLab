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
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Manipulators
{
  [Item ("StatementAddManipulator", "A mutation operator which randomly adds a statement from the same syntax tree.")]
  [StorableClass]
  public sealed class StatementAddManipulator : SyntaxTreeManipulator {

    [StorableConstructor]
    private StatementAddManipulator(bool deserializing) : base(deserializing) {
    }

    public StatementAddManipulator ()
        : base () {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new StatementAddManipulator(this, cloner);
    }

    private StatementAddManipulator(StatementAddManipulator original, Cloner cloner)
        : base(original, cloner) {
    }

    protected override SyntaxTreeEncoding ApplyManipulation (SyntaxTreeEncoding individual) {
      var random = RandomParameter.ActualValue;
      var statements = GetAllStatements(individual.SyntaxTree.GetRoot());
      if (statements.Length == 0)
        return individual;

      var addee = statements[random.Next (statements.Length)];
      var addingLocation = statements[random.Next (statements.Length)];

      if (addee.Equals (addingLocation))
        return individual;

      BlockSyntax combinedStatementBlock;
      SyntaxNode replacementLocation = addingLocation;
      var blockParent = addingLocation.Parent as BlockSyntax;
      if (blockParent != null) {
        combinedStatementBlock = blockParent.AddStatements(addee);
        replacementLocation = addingLocation.Parent;
      }
      else
        combinedStatementBlock = SyntaxFactory.Block(addingLocation, addee);

      var mutatedSyntaxTree = individual.SyntaxTree.GetRoot().ReplaceNode(replacementLocation, combinedStatementBlock).SyntaxTree;

      individual.SyntaxTree = mutatedSyntaxTree;

      return individual;
    }
  }
}