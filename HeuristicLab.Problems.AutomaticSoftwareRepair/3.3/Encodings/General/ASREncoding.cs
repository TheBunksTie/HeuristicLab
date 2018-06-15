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

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General {
  [Item("ASREncoding", "A base class for encodings of ASR solutions.")]
  [StorableClass]
  public abstract class ASREncoding : Item, IASREncoding {
    [Storable]
    protected IASRProblemInstance ProblemInstance { get; set; }

    public ASREncoding(IASRProblemInstance problemInstance) { 
      ProblemInstance = problemInstance;
    }

    [StorableConstructor]
    protected ASREncoding(bool serializing)
        : base(serializing) {
    }
   
    protected ASREncoding(ASREncoding original, Cloner cloner)
      : base(original, cloner) {
      
      if (original.ProblemInstance != null && cloner.ClonedObjectRegistered(original.ProblemInstance))
        this.ProblemInstance = cloner.Clone(original.ProblemInstance);
      else
        this.ProblemInstance = original.ProblemInstance;
    }

    public abstract string GetSolutionProgram ();
  }
}
