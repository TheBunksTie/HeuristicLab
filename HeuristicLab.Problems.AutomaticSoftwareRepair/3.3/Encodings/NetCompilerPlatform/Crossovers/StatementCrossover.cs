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
using HeuristicLab.Optimization;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using Microsoft.CodeAnalysis;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Crossovers
{
  [Item("StatementCrossover", "A simple crossover operator which crosses two ASR synatx trees at a randomly selected statement.")]
  [StorableClass]
  public sealed class StatementCrossover : SyntaxTreeCrossover, IStochasticOperator {
    [StorableConstructor]
    private StatementCrossover(bool deserializing) : base(deserializing) { }

    public StatementCrossover()
        : base() {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new StatementCrossover(this, cloner);
    }

    private StatementCrossover(StatementCrossover original, Cloner cloner)
        : base(original, cloner) {
    }

    protected override SyntaxTreeEncoding Crossover (IRandom random, SyntaxTreeEncoding parent1, SyntaxTreeEncoding parent2) {

      var statements = GetAllStatements(parent1.SyntaxTree.GetRoot());
      var selectedStatement = statements[random.Next (statements.Length)];

      var statements2 = GetAllStatements (parent2.SyntaxTree.GetRoot(), s => s.Kind() == selectedStatement.Kind());
      if (statements2.Length == 0)
        return parent1;

      var selectedExpression2 = statements2[random.Next (statements2.Length)];

      var syntaxTree = parent1.SyntaxTree.GetRoot().ReplaceNode (selectedStatement, selectedExpression2).SyntaxTree;

      parent1.SyntaxTree = syntaxTree;

      return parent1;
    }
  }
}