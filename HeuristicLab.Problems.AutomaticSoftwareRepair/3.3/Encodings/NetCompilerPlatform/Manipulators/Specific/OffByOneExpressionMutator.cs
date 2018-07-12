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

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Manipulators.Specific {
  [Item ("OffByOneExpressionMutator", "A specific mutation operator which adds or removes 1 to a randomly selected binary expression")]
  [StorableClass]
  public sealed class OffByOneExpressionMutator : SyntaxTreeManipulator {

    [StorableConstructor]
    private OffByOneExpressionMutator (bool deserializing) : base (deserializing) { }

    public OffByOneExpressionMutator ()
        : base () {
    }

    public override IDeepCloneable Clone (Cloner cloner) {
      return new OffByOneExpressionMutator (this, cloner);
    }

    private OffByOneExpressionMutator (OffByOneExpressionMutator original, Cloner cloner)
        : base (original, cloner) {
    }

    protected override SyntaxTreeEncoding ApplyMutation (IRandom random, SyntaxTreeEncoding individual) {
      var binaryExpressions = individual.SyntaxTree.GetRoot ().DescendantNodes ().OfType<BinaryExpressionSyntax> ().ToArray ();
      if (binaryExpressions.Length == 0)
        return individual;

      var insertionPointBinaryExpression = binaryExpressions[random.Next (binaryExpressions.Length)].Right;

      var operation = random.NextDouble() > 0.5 ? SyntaxKind.AddExpression : SyntaxKind.SubtractExpression;   

      var extendedBinaryExpression = SyntaxFactory.BinaryExpression (
          operation,
          insertionPointBinaryExpression,
          SyntaxFactory.LiteralExpression (SyntaxKind.NullLiteralExpression, SyntaxFactory.Literal (1)));
      
      var mutatedSyntaxTree = individual.SyntaxTree.GetRoot ().ReplaceNode (insertionPointBinaryExpression, extendedBinaryExpression).SyntaxTree;

      individual.SyntaxTree = mutatedSyntaxTree;

      return individual;
    }
  }
}