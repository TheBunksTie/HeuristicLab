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
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Crossovers
{
  [Item("ExpressionStatementCrossover", "A simple crossover operator which crosses two ASR synatx trees at a randomly selected expression statement.")]
  [StorableClass]
  public sealed class ExpressionStatementCrossover : SyntaxTreeCrossover {
    [StorableConstructor]
    private ExpressionStatementCrossover(bool deserializing) : base(deserializing) { }

    public ExpressionStatementCrossover()
        : base() {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new ExpressionStatementCrossover(this, cloner);
    }

    private ExpressionStatementCrossover(ExpressionStatementCrossover original, Cloner cloner)
        : base(original, cloner) {
    }

    protected override IASREncoding Crossover (IRandom random, SyntaxTreeEncoding parent1, SyntaxTreeEncoding parent2) {
      // TODO try with StatementSyntax?
      var expressions1 = parent1.SyntaxTree.GetRoot().DescendantNodesAndSelf().OfType<ExpressionStatementSyntax>().ToArray();
      var expressions2 = parent2.SyntaxTree.GetRoot().DescendantNodesAndSelf().OfType<ExpressionStatementSyntax>().ToArray();

      if (expressions1.Length == 0)
        return parent2;

      if (expressions2.Length == 0)
        return parent1;

      var selectedExpression1 = expressions1[random.Next (expressions1.Length)];
      var selectedExpression2 = expressions2[random.Next (expressions2.Length)];

      var syntaxTree = parent1.SyntaxTree.GetRoot().ReplaceNode (selectedExpression1, selectedExpression2).SyntaxTree;

      parent1.SyntaxTree = syntaxTree;

      return parent1;
    }
  }
}