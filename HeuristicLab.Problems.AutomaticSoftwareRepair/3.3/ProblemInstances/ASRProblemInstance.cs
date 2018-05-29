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
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.ProblemInstances {
  [Item ("ASRProblemInstance", "Represents a ASR instance.")]
  [StorableClass]
  public abstract class ASRProblemInstance : ParameterizedNamedItem, IASRProblemInstance, IStatefulItem {

    IASREvaluator evaluator;

    private object locker = new object();

    public IASREvaluator SolutionEvaluator {
      get {
        return evaluator;
      }
      set {
        lock (locker) {
          evaluator = value;
        }
      }
    }

    protected abstract IEnumerable<IOperator> GetOperators();
    protected abstract IEnumerable<IOperator> GetAnalyzers();

    public IEnumerable<IOperator> Operators {
      get {
        return GetOperators().Union(GetAnalyzers());
      }
    }

    protected ValueParameter<StringValue> CorrectnessSpecificationParameter {
      get { return (ValueParameter<StringValue>)Parameters["CorrectnessSpecification"]; }
    }
    protected ValueParameter<StringValue> BuggyProgramParameter {
      get { return (ValueParameter<StringValue>)Parameters["BuggyProgram"]; }
    }

    public StringValue CorrectnessSpecification {
      get { return CorrectnessSpecificationParameter.Value; }
      set { CorrectnessSpecificationParameter.Value = value; }
    }
    public StringValue BuggyProgram {
      get { return BuggyProgramParameter.Value; }
      set { BuggyProgramParameter.Value = value; }
    }

    
    protected abstract IASREvaluator Evaluator { get; }
    protected abstract IASRCreator Creator { get; }

    [StorableConstructor]
    protected ASRProblemInstance(bool deserializing) : base(deserializing) { }

    public ASRProblemInstance()
        : base() {
      Parameters.Add(new ValueParameter<StringValue>("CorrectnessSpecification", "The correctness specification for the buggy program.", new StringValue()));
      Parameters.Add(new ValueParameter<StringValue>("BuggyProgram", "The buggy program.", new StringValue()));
      
      evaluator = Evaluator;
      AttachEventHandlers();
    }

    protected ASRProblemInstance(ASRProblemInstance original, Cloner cloner)
        : base(original, cloner) {
      evaluator = Evaluator;
      AttachEventHandlers();
    }

    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      evaluator = Evaluator;
      AttachEventHandlers();
    }

    private void AttachEventHandlers () {
      //CorrectnessSpecificationParameter.ValueChanged += CoordinatesParameter_ValueChanged;
    }

    public virtual void InitializeState () {
    }

    public virtual void ClearState () {
    }
  }
}

