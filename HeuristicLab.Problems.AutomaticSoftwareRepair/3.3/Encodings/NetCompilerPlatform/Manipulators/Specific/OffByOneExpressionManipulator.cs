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
  [Item ("OffByOneExpressionManipulator", "A specific mutation operator which adds or removes 1 to a randomly selected binary expression")]
  [StorableClass]
  public sealed class OffByOneExpressionManipulator : SpecificManipulator {

    private readonly IList<SyntaxKind> mathematicalOperations = new List<SyntaxKind> {
                                                                                       SyntaxKind.AddExpression,
                                                                                       SyntaxKind.SubtractExpression   
                                                                                   };

    [StorableConstructor]
    private OffByOneExpressionManipulator (bool deserializing) : base (deserializing) { }

    public OffByOneExpressionManipulator ()
        : base () {
    }

    public override IDeepCloneable Clone (Cloner cloner) {
      return new OffByOneExpressionManipulator (this, cloner);
    }

    private OffByOneExpressionManipulator (OffByOneExpressionManipulator original, Cloner cloner)
        : base (original, cloner) {
    }

    protected override SyntaxTreeEncoding ApplyManipulation (SyntaxTreeEncoding individual) {
      var random = RandomParameter.ActualValue;
      var syntaxTreeRoot = individual.SyntaxTree.GetRoot();
      var binaryExpressions = syntaxTreeRoot.DescendantNodes().OfType<BinaryExpressionSyntax>()
          .Where (e => e.Right is IdentifierNameSyntax).ToArray();
      if (binaryExpressions.Length == 0) {
        OperatorPerformanceParameter.ActualValue.OperatorApplicable = false;
        return individual;
      }

      var insertionPointBinaryExpression = binaryExpressions[random.Next (binaryExpressions.Length)].Right;

      var operation = mathematicalOperations[random.Next (mathematicalOperations.Count)];

      var extendedBinaryExpression = SyntaxFactory.BinaryExpression (
          operation,
          insertionPointBinaryExpression,
          SyntaxFactory.LiteralExpression (SyntaxKind.NullLiteralExpression, SyntaxFactory.Literal (1)));
      
      var mutatedSyntaxTree = syntaxTreeRoot.ReplaceNode (insertionPointBinaryExpression, extendedBinaryExpression).SyntaxTree;

      individual.SyntaxTree = mutatedSyntaxTree;

      return individual;
    }
  }
}