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
  [Item("SolutionProgramEncoding", "Represents a base class for solution program encodings of ASR solutions.")]
  [StorableClass]
  public abstract class ASREncoding : Item, IASREncoding {
    //public static new Image StaticItemImage {
    //  get { return HeuristicLab.Common.Resources.VSImageLibrary.Class; }
    //}

    //public IList<SolutionProgram> GetSolutionPrograms () {

    //  var result = new List<SolutionProgram>();
    //  foreach (var solutionProgram in SolutionPrograms)
    //    result.Add(solutionProgram.Clone() as SolutionProgram);

    //  return result;
    //}

    //public int GetSolutionProgramIndex (SolutionProgram solutionProgram) {
    //  int index = -1;

    //  for (int i = 0; i < SolutionPrograms.Count; i++) {
    //    if (SolutionPrograms[i].IsEqual(solutionProgram)) {
    //      index = i;
    //      break;
    //    }
    //  }

    //  return index;
    //}

    //[Storable]
    //public ItemList<SolutionProgram> SolutionPrograms { get; set; }

    [Storable]
    protected IASRProblemInstance ProblemInstance { get; set; }

    public ASREncoding(IASRProblemInstance problemInstance) { 
      ProblemInstance = problemInstance;
    }

    public ASREncoding(SolutionProgram solutionProgram, IASRProblemInstance problemInstance)
        : base() { 

      this.SolutionProgram = solutionProgram;
      this.ProblemInstance = problemInstance;
    }

    [StorableConstructor]
    protected ASREncoding(bool serializing)
        : base(serializing) {
    }

    //public static void ConvertFrom(IASREncoding encoding, SolutionProgramEncoding solution, IASRProblemInstance problemInstance) {
    //  solution.SolutionPrograms = new ItemList<SolutionProgram>(encoding.GetSolutionPrograms());
    //  //TODO what is solution.Repair();
    //}

    //public static void ConvertFrom(string solutionProgram, SolutionProgramEncoding solution) {
    //  solution.SolutionPrograms = new ItemList<SolutionProgram>();

    //  var tour = new SolutionProgram(solutionProgram, -1);
    //  solution.SolutionPrograms.Add(tour);
    //  //solution.Repair();
    //}

   
    protected ASREncoding(ASREncoding original, Cloner cloner)
      : base(original, cloner) {
      
      if (original.ProblemInstance != null && cloner.ClonedObjectRegistered(original.ProblemInstance))
        this.ProblemInstance = (IASRProblemInstance)cloner.Clone(original.ProblemInstance);
      else
        this.ProblemInstance = original.ProblemInstance;

      SolutionProgram = original.SolutionProgram;
    }

    protected ASREncoding(string name)
      : base() {
    }

    public SolutionProgram GetSolutionPrograms () {
      throw new NotImplementedException();
    }

    public SolutionProgram SolutionProgram { get; set; }
  }
}
