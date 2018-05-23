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

using System.IO;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General.Creators;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.Simple.Creators {
  [Item("TextBasedSolutionCreator", "Creates a ASR solution from source code as text.")]
  [StorableClass]
  public sealed class TextBasedSolutionCreator :  ASRCreator {
    
    public ILookupParameter<StringValue> SourceCodeParameter {
      get { return (LookupParameter<StringValue>)Parameters["SourceCode"]; }
    }

    public string SourceCode {
      get { return SourceCodeParameter.ActualValue.Value; }
    }

    [StorableConstructor]
    private TextBasedSolutionCreator(bool deserializing) : base(deserializing) { }

    public TextBasedSolutionCreator()
        : base() {
      Parameters.Add(new LookupParameter<StringValue>("SourceCode", "The source code of the buggy program."));
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new TextBasedSolutionCreator(this, cloner);
    }

    private TextBasedSolutionCreator(TextBasedSolutionCreator original, Cloner cloner)
        : base(original, cloner) {
    }

    public override IOperation InstrumentedApply() {
      ASRProgramSolutionsParameter.ActualValue = CreateSolution(ProblemInstance);

      return base.InstrumentedApply();
    }

    public SimpleEncoding CreateSolution (IASRProblemInstance instance) {
      var result = new SimpleEncoding(instance);

      result.SolutionPrograms.Add(new SolutionProgram(SourceCode));

      return result;
    }
  }
}