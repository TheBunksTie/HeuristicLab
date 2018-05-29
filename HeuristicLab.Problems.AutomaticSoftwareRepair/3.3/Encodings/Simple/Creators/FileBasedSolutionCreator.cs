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

using System.IO;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General.Creators;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.Simple.Creators {
  [Item("FileBasedSolutionCreator", "Creates a ASR solution from source code in given file.")]
  [StorableClass]
  public sealed class FileBasedSolutionCreator :  ASRCreator {
    
    public ILookupParameter<TextFileValue> SourceFileParameter {
      get { return (LookupParameter<TextFileValue>)Parameters["SourceFile"]; }
    }

    public TextFileValue SourceFile {
      get { return SourceFileParameter.ActualValue; }
    }

    [StorableConstructor]
    private FileBasedSolutionCreator(bool deserializing) : base(deserializing) { }

    public FileBasedSolutionCreator()
        : base() {
      Parameters.Add(new LookupParameter<TextFileValue>("SourceFile", "The text file containing the buggy program source code."));
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new FileBasedSolutionCreator(this, cloner);
    }

    private FileBasedSolutionCreator(FileBasedSolutionCreator original, Cloner cloner)
        : base(original, cloner) {
    }

    public override IOperation InstrumentedApply() {
      ASRProgramSolutionsParameter.ActualValue = CreateSolution(ProblemInstance);

      return base.InstrumentedApply();
    }

    public NetCompilerPlatformBasedEncoding CreateSolution (IASRProblemInstance instance) {
      var result = new NetCompilerPlatformBasedEncoding(instance);

      var sourceFileContent = File.ReadAllText(SourceFile.Value);
      result.SolutionProgram = new SolutionProgram (sourceFileContent);

      return result;

    }
  }
}