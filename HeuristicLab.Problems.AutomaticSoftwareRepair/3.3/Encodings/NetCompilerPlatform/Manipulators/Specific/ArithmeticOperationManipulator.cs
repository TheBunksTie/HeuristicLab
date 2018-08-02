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
  [Item ("ArithmeticOperationManipulator", "A specific mutation operator which adds a basic arithmetic operation with a random identifier to a binary expression.")]
  [StorableClass]
  public sealed class ArithmeticOperationManipulator : SpecificManipulator {

    private readonly IList<SyntaxKind> arithmeticOperations = new List<SyntaxKind> {
                                 SyntaxKind.AddExpression,
                                 SyntaxKind.SubtractExpression,
                                 SyntaxKind.MultiplyExpression,
                                 SyntaxKind.DivideExpression,
                             };

    [StorableConstructor]
    private ArithmeticOperationManipulator (bool deserializing) : base (deserializing) { }

    public ArithmeticOperationManipulator ()
        : base () {
    }

    public override IDeepCloneable Clone (Cloner cloner) {
      return new ArithmeticOperationManipulator (this, cloner);
    }

    private ArithmeticOperationManipulator (ArithmeticOperationManipulator original, Cloner cloner)
        : base (original, cloner) {
    }

    protected override SyntaxTreeEncoding ApplyManipulation (SyntaxTreeEncoding individual) {
      var random = RandomParameter.ActualValue;
      var syntaxTreeRoot = individual.SyntaxTree.GetRoot ();
      var rhsIdentifierInBinaryExpressions = syntaxTreeRoot.DescendantNodes ().OfType<BinaryExpressionSyntax> ().Select(e => e.Right as IdentifierNameSyntax).Where(i => i != null).ToArray ();
      if (rhsIdentifierInBinaryExpressions.Length == 0) {
        OperatorPerformanceParameter.ActualValue.OperatorApplicable = false;
        return individual;
      }

      var overallIdentifierExpressions = syntaxTreeRoot.DescendantNodes ().OfType<IdentifierNameSyntax> ().Distinct().ToArray ();
      if (overallIdentifierExpressions.Length == 0) {
        OperatorPerformanceParameter.ActualValue.OperatorApplicable = false;
        return individual;
      }

      var modifiableRhsIdentifierInBinaryExpression = rhsIdentifierInBinaryExpressions[random.Next (rhsIdentifierInBinaryExpressions.Length)];
      var operation = arithmeticOperations[random.Next (arithmeticOperations.Count)];
      var operandIdentifier = overallIdentifierExpressions[random.Next (overallIdentifierExpressions.Length)];

      var operationChangedBinaryExpression = SyntaxFactory.BinaryExpression (
          operation,
          modifiableRhsIdentifierInBinaryExpression,
          operandIdentifier);

      var mutatedSyntaxTree = syntaxTreeRoot.ReplaceNode (modifiableRhsIdentifierInBinaryExpression, operationChangedBinaryExpression).SyntaxTree;

      individual.SyntaxTree = mutatedSyntaxTree;

      return individual;
    }
  }
}