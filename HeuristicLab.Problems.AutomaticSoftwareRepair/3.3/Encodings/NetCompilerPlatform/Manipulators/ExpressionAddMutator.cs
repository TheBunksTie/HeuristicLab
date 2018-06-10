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
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Manipulators
{
  [Item ("ExpressionAddMutator", "A mutation operator which randomly adds an expression from the same syntax tree.")]
  [StorableClass]
  public sealed class ExpressionAddMutator : SyntaxTreeManipulator {

    [StorableConstructor]
    private ExpressionAddMutator(bool deserializing) : base(deserializing) {
    }

    public ExpressionAddMutator ()
        : base () {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new ExpressionAddMutator(this, cloner);
    }

    private ExpressionAddMutator(ExpressionAddMutator original, Cloner cloner)
        : base(original, cloner) {
    }

    protected override CSharpSyntaxRewriter CreateSyntaxTreeRewriter (IRandom random) {
      return new ExpressionAddSyntaxTreeRewriter (random);
    }

    private sealed class ExpressionAddSyntaxTreeRewriter : CSharpSyntaxRewriter {
      private readonly IRandom random;

      public ExpressionAddSyntaxTreeRewriter (IRandom random) {
        this.random = random;
      }

      public override SyntaxNode VisitExpressionStatement (ExpressionStatementSyntax node) {
        var expressions = node.SyntaxTree.GetRoot().DescendantNodesAndSelf().OfType<ExpressionStatementSyntax>().ToArray();
        if (expressions.Length > 0) {
          var replacementEpression = expressions[random.Next (expressions.Length)];
          node = node.WithExpression(replacementEpression.Expression);
        }

        return base.VisitExpressionStatement (node);
      }
    }
  }
}