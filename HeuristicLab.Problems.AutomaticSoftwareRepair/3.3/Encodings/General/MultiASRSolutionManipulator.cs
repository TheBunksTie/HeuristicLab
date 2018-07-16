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
using System.Collections.Generic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Manipulators.Specific;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General {
  
  [Item("MultiASRSolutionManipulator", "Randomly selects and applies one of its manipulators every time it is called.")]
  [StorableClass]
  public class MultiASRSolutionManipulator :  StochasticMultiBranch<IASRManipulator>, IASRManipulator, IMultiASROperator, IStochasticOperator {
    private const string ProblemInstanceParameterName = "ProblemInstance";
    private const string SolutionParameterName = "ASRSolution";

    public override bool CanChangeName {
      get { return false; }
    }
    protected override bool CreateChildOperation {
      get { return true; }
    }

    public ILookupParameter<IASREncoding> ASRSolutionParameter {
      get { return (ILookupParameter<IASREncoding>) Parameters[SolutionParameterName]; }
    }

    public ILookupParameter<IASRProblemInstance> ProblemInstanceParameter {
      get { return (LookupParameter<IASRProblemInstance>)Parameters[ProblemInstanceParameterName]; }
    }

    [StorableConstructor]
    protected MultiASRSolutionManipulator(bool deserializing) : base(deserializing) { }
    public MultiASRSolutionManipulator()
        : base() {
      Parameters.Add (new LookupParameter<IASRProblemInstance> (ProblemInstanceParameterName, "The ASR problem instance"));
      Parameters.Add (new LookupParameter<IASREncoding> (SolutionParameterName, "The ASR solution to be manipulated."));
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new MultiASRSolutionManipulator(this, cloner);
    }

    protected MultiASRSolutionManipulator(MultiASRSolutionManipulator original, Cloner cloner)
        : base(original, cloner) {
    }

    public virtual void SetOperators (IEnumerable<IOperator> operators) {
      foreach (var op in operators) {
        if (op is IASRManipulator && !(op is MultiASRSolutionManipulator) && !(op is SpecificManipulator)) {
          Operators.Add(op.Clone() as IASRManipulator);
        }
      }
    }

    public override IOperation InstrumentedApply() {
      if (Operators.Count == 0) throw new InvalidOperationException(Name + ": Please add at least one ASR solution manipulator to choose from.");
      return base.InstrumentedApply();
    }
  }
}
 