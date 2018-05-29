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
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.Simple.Crossovers
{
  [Item("IfStatementCrossover", "A simple crossover operator which crosses two ASR synatx trees by a crossover point of an if statement.")]
  [StorableClass]
  public sealed class IfStatementCrossover : ASRCrossover {
    [StorableConstructor]
    private IfStatementCrossover(bool deserializing) : base(deserializing) { }

    public IfStatementCrossover()
        : base() {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new IfStatementCrossover(this, cloner);
    }

    private IfStatementCrossover(IfStatementCrossover original, Cloner cloner)
        : base(original, cloner) {
    }

    protected override IASREncoding Crossover (IRandom random, IASREncoding parent1, IASREncoding parent2) {
      var tree1 = parent1.SolutionProgram.TreeRepresentation.GetRoot ();
      var ifStatements1 = tree1.DescendantNodesAndSelf ().OfType<IfStatementSyntax> ().ToArray ();
      var ifStatements2 = parent2.SolutionProgram.TreeRepresentation.GetRoot ().DescendantNodesAndSelf ().OfType<IfStatementSyntax> ().ToArray ();

      var selectedIfStat1 = ifStatements1[random.Next (ifStatements1.Length)];
      var selectedIfStat2 = ifStatements2[random.Next (ifStatements2.Length)];

      var syntaxTree = tree1.ReplaceNode (selectedIfStat1, selectedIfStat2).SyntaxTree;

      parent1.SolutionProgram.TreeRepresentation = syntaxTree;

      return parent1;
    }
  }
}