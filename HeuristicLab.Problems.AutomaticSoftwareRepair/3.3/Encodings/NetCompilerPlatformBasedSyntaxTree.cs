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
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;
using Microsoft.CodeAnalysis;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings
{
  [StorableClass]
  [Item ("NetCompilerPlatformBasedSyntaxTree ", "Represents a syntax tree based on MS Compiler Platform representation.")]
  public class NetCompilerPlatformBasedSyntaxTree : Item, IASRSyntaxTree {
    //public static new Image StaticItemImage {
    //  get { return HeuristicLab.Common.Resources.VSImageLibrary.Function; }
    //}

    [Storable]
    private SyntaxTree treeRepresentation;

    public SyntaxTree Tree {
      get { return treeRepresentation; }
      set {
        if (value == null) throw new ArgumentNullException();
        else if (value != treeRepresentation) {
          treeRepresentation = value;
          OnToStringChanged();
        }
      }
    }

    [StorableConstructor]
    protected NetCompilerPlatformBasedSyntaxTree(bool deserializing) : base(deserializing) { }
    protected NetCompilerPlatformBasedSyntaxTree(NetCompilerPlatformBasedSyntaxTree original, Cloner cloner)
        : base(original, cloner) {
      treeRepresentation = original.Tree;
    }
    public NetCompilerPlatformBasedSyntaxTree() : base() { }
    public NetCompilerPlatformBasedSyntaxTree(SyntaxTree syntaxTree)
        : base() {
      this.Tree = syntaxTree;
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new NetCompilerPlatformBasedSyntaxTree(this, cloner);
    }
  }
}