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
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Manipulators.Specific {
  [Item ("IdentifierReplacer", "A specific mutation operator which randomly replaces an identifier (variable/propery/method/etc.) name with another one.")]
  [StorableClass]
  public sealed class IdentifierReplacer : SpecificManipulator {

    [StorableConstructor]
    private IdentifierReplacer (bool deserializing) : base (deserializing) { }

    public IdentifierReplacer ()
        : base () {
    }

    public override IDeepCloneable Clone (Cloner cloner) {
      return new IdentifierReplacer (this, cloner);
    }

    private IdentifierReplacer (IdentifierReplacer original, Cloner cloner)
        : base (original, cloner) {
    }

    protected override SyntaxTreeEncoding ApplyManipulation (SyntaxTreeEncoding individual) {
      var random = RandomParameter.ActualValue;
      var syntaxTreeRoot = individual.SyntaxTree.GetRoot();
      var identifiers =syntaxTreeRoot.DescendantNodes ().OfType<IdentifierNameSyntax> ().ToArray ();
      if (identifiers.Length == 0) {
        OperatorPerformanceParameter.ActualValue.OperatorApplicable = false;
        return individual;
      }

      var replacee = identifiers[random.Next (identifiers.Length)];
      var replacement = identifiers[random.Next (identifiers.Length)];

      if (replacee == replacement)
        return individual;
            
      var mutatedSyntaxTree =syntaxTreeRoot.ReplaceNode (replacee, replacement).SyntaxTree;

      individual.SyntaxTree = mutatedSyntaxTree;

      return individual;
    }
  }
}