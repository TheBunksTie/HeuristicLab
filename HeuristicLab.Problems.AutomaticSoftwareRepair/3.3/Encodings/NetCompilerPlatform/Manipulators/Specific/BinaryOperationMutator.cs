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
using System.Collections.Generic;
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Manipulators.Specific {
  [Item ("BinaryOperationMutator", "A specific mutation operator which replaces a binary bitwise operation with a randomly selected one.")]
  [StorableClass]
  public sealed class BinaryOperationMutator : SyntaxTreeManipulator {

    private readonly IList<SyntaxKind> binaryOperations = new List<SyntaxKind> {
                                 SyntaxKind.ExclusiveOrExpression,
                                 SyntaxKind.BitwiseAndExpression,
                                 SyntaxKind.BitwiseOrExpression,
                                 SyntaxKind.RightShiftExpression,
                                 SyntaxKind.LeftShiftExpression
                             };

    [StorableConstructor]
    private BinaryOperationMutator (bool deserializing) : base (deserializing) { }

    public BinaryOperationMutator ()
        : base () {
    }

    public override IDeepCloneable Clone (Cloner cloner) {
      return new BinaryOperationMutator (this, cloner);
    }

    private BinaryOperationMutator (BinaryOperationMutator original, Cloner cloner)
        : base (original, cloner) {
    }

    protected override SyntaxTreeEncoding ApplyMutation (IRandom random, SyntaxTreeEncoding individual) {
      var binaryExpressions = individual.SyntaxTree.GetRoot ().DescendantNodes ().OfType<BinaryExpressionSyntax> ().Where(e => binaryOperations.Contains(e.Kind())).ToArray ();
      if (binaryExpressions.Length == 0)
        return individual;

      var modifiyableBinaryExpression = binaryExpressions[random.Next (binaryExpressions.Length)];
      var operation = binaryOperations[random.Next (binaryOperations.Count)];
      if (operation.Equals (modifiyableBinaryExpression.Kind()))
        return individual;

      var operationChangedBinaryExpression = SyntaxFactory.BinaryExpression (
          operation,
          modifiyableBinaryExpression.Left,
          modifiyableBinaryExpression.Right);
      
      var mutatedSyntaxTree = individual.SyntaxTree.GetRoot ().ReplaceNode (modifiyableBinaryExpression, operationChangedBinaryExpression).SyntaxTree;

      individual.SyntaxTree = mutatedSyntaxTree;

      return individual;
    }
  }
}