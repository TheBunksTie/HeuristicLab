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
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Analyzer;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Manipulators.Specific;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;
using HeuristicLab.Random;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General {
  [Item("AdaptingMultiASRSolutionManipulator", "Adapts the probabilities of specific manipulators and selects/applies one of its manipulators every time it is called based on the manipulator performance.")]
  [StorableClass]
  public class AdaptingMultiASRSolutionManipulator : MultiASRSolutionManipulator {
    private const string ActualProbabilitiesParameterName = "ActualProbabilities";
    private const string FactorParameterName = "Factor";
    private const string LowerBoundParameterName = "LowerBound";
    private const string ResultsParameterName = "Results";
    private const string OperatorPerformanceParameterName = "OperatorPerformance";
    private const string SelectedManipulatorParameterName = "SelectedManipulationOperator";

    public ValueLookupParameter<DoubleArray> ActualProbabilitiesParameter {
      get { return (ValueLookupParameter<DoubleArray>)Parameters[ActualProbabilitiesParameterName]; }
    }
    public ValueLookupParameter<DoubleValue> FactorParameter {
      get { return (ValueLookupParameter<DoubleValue>)Parameters[FactorParameterName]; }
    }
    public ValueParameter<DoubleValue> LowerBoundParameter {
      get { return (ValueParameter<DoubleValue>)Parameters[LowerBoundParameterName]; }
    }

    public ILookupParameter<OperatorPerformanceResultsCollection> OperatorPerformanceParameter {
      get { return (LookupParameter<OperatorPerformanceResultsCollection>)Parameters[OperatorPerformanceParameterName]; }
    }

    [StorableConstructor]
    protected AdaptingMultiASRSolutionManipulator(bool deserializing) : base(deserializing) { }
    protected AdaptingMultiASRSolutionManipulator(AdaptingMultiASRSolutionManipulator original, Cloner cloner) : base(original, cloner) { }
    public AdaptingMultiASRSolutionManipulator()
      : base() {
      Parameters.Add(new ValueLookupParameter<DoubleArray>(ActualProbabilitiesParameterName, "The array of relative probabilities for each operator.")); 
      Parameters.Add(new ValueLookupParameter<DoubleValue>(FactorParameterName, "The factor with which the probabilities should be updated.", new DoubleValue(0.2)));
      Parameters.Add(new ValueParameter<DoubleValue>(LowerBoundParameterName, "The minimum weight an operator can be assigned during the adaptation process.", new DoubleValue(0.01)));
      Parameters.Add(new LookupParameter<OperatorPerformanceResultsCollection> (OperatorPerformanceParameterName, "The performance data of operations on current solution."));
      
      SelectedOperatorParameter.ActualName = SelectedManipulatorParameterName;
      OperatorPerformanceParameter.Hidden = true;
      TraceSelectedOperatorParameter.Value = new BoolValue (true);
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new AdaptingMultiASRSolutionManipulator(this, cloner);
    }

    public override void InitializeState() {
      base.InitializeState();

      ActualProbabilitiesParameter.Value = null;
    }

    public override void SetOperators (IEnumerable<IOperator> operators) {
      foreach (var op in operators) {
        if (op is SpecificManipulator) {
          Operators.Add(op.Clone() as IASRManipulator);
        }
      }
    }

    public override IOperation InstrumentedApply() {
      IOperator successor = null;

      if (ActualProbabilitiesParameter.ActualValue == null) {
        ActualProbabilitiesParameter.Value = ProbabilitiesParameter.ActualValue.Clone() as DoubleArray;
      } else {
        ResultCollection results = null;
        var scope = ExecutionContext.Parent.Scope;

        if (scope != null)
          results = scope.Variables[ResultsParameterName].Value as ResultCollection;

        if (results != null) { 
            for (var i = 0; i < Operators.Count; i++) {
              var current = Operators[i];

              if (results.ContainsKey (current.Name)) {
                var operatorSuccesRate = results[current.Name].Value as DoubleValue;
                ActualProbabilitiesParameter.ActualValue[i] += operatorSuccesRate.Value / ActualProbabilitiesParameter.ActualValue[i] * FactorParameter.Value.Value;
            }
          }
        }

        //normalize
        var max = ActualProbabilitiesParameter.ActualValue.Max();
        for (var i = 0; i < ActualProbabilitiesParameter.ActualValue.Length; i++) {
          ActualProbabilitiesParameter.ActualValue[i] /= max;
          ActualProbabilitiesParameter.ActualValue[i] = Math.Max(LowerBoundParameter.Value.Value, ActualProbabilitiesParameter.ActualValue[i]);
        }
      }

      //////////////// code has to be duplicated since ActualProbabilitiesParameter.ActualValue are updated and used for operator selection
      var random = RandomParameter.ActualValue;
      var probabilities = ActualProbabilitiesParameter.ActualValue;

      if (probabilities.Length != Operators.Count) {
        throw new InvalidOperationException(Name + ": The list of probabilities has to match the number of operators");
      }
      var checkedOperators = Operators.CheckedItems;
      if (checkedOperators.Any()) {
        // select a weighted random operator from the checked operators
        successor = checkedOperators.SampleProportional(random, 1, checkedOperators.Select(x => probabilities[x.Index]), false, false).First().Value;
      }

      IOperation successorOp = null;
      if (Successor != null)
        successorOp = ExecutionContext.CreateOperation(Successor);
      var next = new OperationCollection(successorOp);
      if (successor != null) {
        SelectedOperatorParameter.ActualValue = new StringValue(successor.Name);

        if (CreateChildOperation)
          next.Insert(0, ExecutionContext.CreateChildOperation(successor));
        else next.Insert(0, ExecutionContext.CreateOperation(successor));
      } else {
        SelectedOperatorParameter.ActualValue = new StringValue("");
      }

      if (OperatorPerformanceParameter.ActualValue == null)
        OperatorPerformanceParameter.ActualValue = new OperatorPerformanceResultsCollection { OperatorName = SelectedOperatorParameter.ActualValue.Value, };

      return next;
    }
  }
}
