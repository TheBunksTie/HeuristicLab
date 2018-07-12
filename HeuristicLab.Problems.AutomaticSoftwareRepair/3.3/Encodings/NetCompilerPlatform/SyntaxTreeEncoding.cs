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
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform {
  [Item ("SyntaxTreeEncoding", "Represents a simple encoding of ASR solutions as .NET Compiler Platform syntax tree.")]
  [StorableClass]
  public class SyntaxTreeEncoding : ASREncoding {
    public SyntaxTreeEncoding(SyntaxTree syntaxTree, IASRProblemInstance instance)
        : base(instance) {
      this.SyntaxTree = syntaxTree;
    }

    public SyntaxTreeEncoding(string sourceCode, IASRProblemInstance instance)
        : base(instance) {
      try {
        this.SyntaxTree = CSharpSyntaxTree.ParseText (sourceCode);
      } catch (Exception) {
        // ignored
      }
    }

    [StorableConstructor]
    protected SyntaxTreeEncoding(bool serializing)
        : base(serializing) {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new SyntaxTreeEncoding(this, cloner);
    }

    protected SyntaxTreeEncoding(SyntaxTreeEncoding original, Cloner cloner)
        : base(original, cloner) {

      SyntaxTree = original.SyntaxTree;
    }
    public SyntaxTree SyntaxTree { get; set; }

    public override string ToString () {
      return SyntaxTree != null ? SyntaxTree.ToString() : string.Empty;
    }

    public override string GetSolutionProgram () {
      return ToString();
    }
  }
}