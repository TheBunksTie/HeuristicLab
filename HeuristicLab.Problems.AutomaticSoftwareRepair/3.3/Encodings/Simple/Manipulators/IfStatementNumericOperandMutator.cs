﻿#region License Information
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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.Simple.Manipulators
{
  [Item("IfStatementNumericOperandMutator", "A simple operator which manipulates a ASR representation by changing numeric constants in if statements.")]
  [StorableClass]
  public sealed class IfStatementNumericOperandMutator : ASRManipulator {
    [StorableConstructor]
    private IfStatementNumericOperandMutator(bool deserializing) : base(deserializing) { }

    public IfStatementNumericOperandMutator()
        : base() {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new IfStatementNumericOperandMutator(this, cloner);
    }

    private IfStatementNumericOperandMutator(IfStatementNumericOperandMutator original, Cloner cloner)
        : base(original, cloner) {
    }

    protected override void Manipulate(IRandom random, SolutionProgramEncoding individual) {
      // TODO: should this be done outside or by infrastructure
      if (random.NextDouble() <=  ASRManipulationProbability.ActualValue.Value)
        return;

      var mutator = new SyntaxTreeRewriter(random);

      var tree = individual.SolutionPrograms.First ();

      var mutatedTree = mutator.MutateTree (tree.TreeRepresentation.Tree);

      individual.SolutionPrograms.Add (new SolutionProgram (new NetCompilerPlatformBasedSyntaxTree (mutatedTree)));
    }

    private sealed class SyntaxTreeRewriter : CSharpSyntaxRewriter {

      private readonly IRandom random;

      public SyntaxTreeRewriter (IRandom random) {
        this.random = random;
      }

      public SyntaxTree MutateTree (SyntaxTree tree)
      {
        var root = tree.GetRoot ();
        root = Visit (root);
        return root.SyntaxTree;
      }

      public override SyntaxNode VisitBinaryExpression (BinaryExpressionSyntax node) {

        var literalExpressionSyntax = node.Right as LiteralExpressionSyntax;
        if (literalExpressionSyntax == null)
          return base.VisitBinaryExpression(node);
        
        if (literalExpressionSyntax.Kind () != SyntaxKind.NumericLiteralExpression)
          return base.VisitBinaryExpression(node);


        var currentValue = int.Parse (literalExpressionSyntax.Token.ValueText);
        var newValue = currentValue + random.Next (-2, 2);
        return node.WithRight (SyntaxFactory.LiteralExpression (SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal (newValue)));
      }
    }
  }
}