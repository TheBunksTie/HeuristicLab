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

using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.Simple;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General {
  /// <summary>
  ///  A base class for manipulating automatic software repair solutions.
  /// </summary>
  [Item("ASRManipulator", "A base class for manipulating automatic software repair solutions.")]
  [StorableClass]
  public abstract class ASRManipulator : ASROperator, IASRManipulator {
    private const string AsrSolutionProgramParameterName = "ASRSolutionProgram";
    private const string AsrManipulationProbabilityParameterName = "AsrManipulationProbability";

    public ILookupParameter<IASREncoding> ASRSolutionProgramParameter {
      get { return (ILookupParameter<IASREncoding>)Parameters[AsrSolutionProgramParameterName]; }
    }

    public ILookupParameter<DoubleValue> ASRManipulationProbability {
      get { return (ILookupParameter<DoubleValue>)Parameters[AsrManipulationProbabilityParameterName]; }
    }

    [StorableConstructor]
    protected ASRManipulator(bool deserializing) : base(deserializing) { }

    public ASRManipulator()
        : base() {
      Parameters.Add(new LookupParameter<IASREncoding>(AsrSolutionProgramParameterName, "The ARS solution program to be manipulated."));
      Parameters.Add(new LookupParameter<DoubleValue>(AsrManipulationProbabilityParameterName, "The probability of a manipulation."));
    }

    protected ASRManipulator(ASRManipulator original, Cloner cloner)
        : base(original, cloner) {
    }

    public override IOperation InstrumentedApply() {
      //var solution = ASRSolutionProgramParameter.ActualValue;
      //if (!(solution is SimpleEncoding)) {
      //  ASRSolutionProgramParameter.ActualValue = SimpleEncoding.ConvertFrom(solution, ProblemInstance);
      //}

      Manipulate (RandomParameter.ActualValue, ASRSolutionProgramParameter.ActualValue as ASREncoding);

      return base.InstrumentedApply();
    }

    protected abstract void Manipulate (IRandom random, IASREncoding individual);
  }
}