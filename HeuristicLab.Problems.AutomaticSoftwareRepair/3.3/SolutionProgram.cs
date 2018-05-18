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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair {
  [StorableClass]
  public sealed class SolutionProgram : NamedItem {
    [Storable]
    public SyntaxTree TreeRepresentation { get; private set; }
    [Storable]
    public string TextRepresentation { get; private set; }
    [Storable]
    public double Quality { get; private set; }

    #region item cloning and persistence
    [StorableConstructor]
    private SolutionProgram(bool deserializing) : base(deserializing) { }
    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() { }

    private SolutionProgram(SolutionProgram original, Cloner cloner)
        : base(original, cloner) {
  
      // TODO deep cloning?
      this.TreeRepresentation = original.TreeRepresentation;
      this.Quality = original.Quality;
    }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new SolutionProgram(this, cloner);
    }
    #endregion

    public SolutionProgram(SyntaxTree treeRepresentation, double quality)
        : base("Solution", "A roslyn based automatic software repair solution.") {
      this.TreeRepresentation = treeRepresentation;
      this.TextRepresentation = treeRepresentation.ToString();
      this.Quality = quality;
    }

    public SolutionProgram(string textRepresentation, double quality)
        : base("Solution", "A roslyn based automatic software repair solution.") {
      this.TextRepresentation = textRepresentation;  
      this.Quality = quality;

      try
      {
        this.TreeRepresentation = CSharpSyntaxTree.ParseText (textRepresentation);
      }
      catch (Exception)
      {
        // ignored
      }
    }

    public bool IsEqual(SolutionProgram solutionProgram) {
      return solutionProgram.TreeRepresentation.Equals(TreeRepresentation);
    }

  }
}