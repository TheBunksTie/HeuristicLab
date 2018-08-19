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
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.ProblemInstances {
  [Item ("ASRProblemInstance", "Represents an ASR problem instance.")]
  [StorableClass]
  public abstract class ASRProblemInstance : ParameterizedNamedItem, IASRProblemInstance, IStatefulItem {
    private const string CorrectnessSpecificationParameterName = "CorrectnessSpecification";
    private const string ProductionCodeParameterName = "ProductionCode";
    private const string CorrectSolutionParameterName = "CorrectSolution";
    private const string PassingTestsParameterName = "PassingTests";
    private const string FailingTestsParameterName = "FailingTests";

    protected ValueParameter<StringValue> CorrectnessSpecificationParameter {
      get { return (ValueParameter<StringValue>)Parameters[CorrectnessSpecificationParameterName]; }
    }
    protected ValueParameter<StringValue> ProductionCodeParameter {
      get { return (ValueParameter<StringValue>)Parameters[ProductionCodeParameterName]; }
    }

    protected ValueParameter<StringValue> CorrectSolutionParameter {
      get { return (ValueParameter<StringValue>)Parameters[CorrectSolutionParameterName]; }
    }

    public ValueParameter<ItemArray<StringValue>> PassingTestsParameter {
      get { return (ValueParameter<ItemArray<StringValue>>)Parameters[PassingTestsParameterName]; }
    }
    public ValueParameter<ItemArray<StringValue>> FailingTestsParameter {
      get { return (ValueParameter<ItemArray<StringValue>>)Parameters[FailingTestsParameterName]; }
    }

    public StringValue CorrectnessSpecification {
      get { return CorrectnessSpecificationParameter.Value; }
      set { CorrectnessSpecificationParameter.Value = value; }
    }
    public StringValue ProductionCode {
      get { return ProductionCodeParameter.Value; }
      set { ProductionCodeParameter.Value = value; }
    }
    public StringValue CorrectSolution {
      get { return CorrectSolutionParameter.Value; }
      set { CorrectSolutionParameter.Value = value; }
    }
    public ItemArray<StringValue> PassingTests {
      get { return PassingTestsParameter.Value; }
      set { PassingTestsParameter.Value = value; }
    }
    public ItemArray<StringValue> FailingTests {
      get { return FailingTestsParameter.Value; }
      set { FailingTestsParameter.Value = value; }
    }
    
    [StorableConstructor]
    protected ASRProblemInstance(bool deserializing) : base(deserializing) { }

    public ASRProblemInstance()
        : base() {
      Parameters.Add(new ValueParameter<StringValue>(CorrectnessSpecificationParameterName, "The correctness specification for the buggy program."));
      Parameters.Add(new ValueParameter<StringValue>(ProductionCodeParameterName, "The buggy production code to repair."));
      Parameters.Add(new ValueParameter<StringValue>(CorrectSolutionParameterName, "A correct version of the buggy production code, fulfilling the tests."));
      Parameters.Add(new ValueParameter<ItemArray<StringValue>>(FailingTestsParameterName, "The initially failing tests")); 
      Parameters.Add(new ValueParameter<ItemArray<StringValue>>(PassingTestsParameterName, "The initially passing tests"));
    }

    protected ASRProblemInstance(ASRProblemInstance original, Cloner cloner)
        : base(original, cloner) {
    }

    public virtual void InitializeState () {
    }

    public virtual void ClearState () {
    }
  }
}

