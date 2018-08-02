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
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Analyzer {
  /// <summary>
  /// An operator for analyzing the best solution of Automatic Software Repair Problems given in source code as string representation.
  /// </summary>
  [Item("OperatorPerformanceASRSolutionAnalyzer", "An operator for analyzing the performance of specific operators")]
  [StorableClass]
  public sealed class OperatorPerformanceASRSolutionAnalyzer : ASROperator, IASRAnalyzer, ISingleObjectiveOperator {
    private const string ASRSolutionParameterName = "ASRSolution";
    private const string OperatorPerformanceParameterName = "OperatorPerformance";
    private const string AggregatedOperatorPerformanceParameterName = "AggregatedOperatorPerformance";

    public bool EnabledByDefault {
      get { return false; }
    }
    public ScopeTreeLookupParameter<IASREncoding> ASRSolutionParameter {
      get { return (ScopeTreeLookupParameter<IASREncoding>)Parameters[ASRSolutionParameterName]; }
    }
    public ScopeTreeLookupParameter<OperatorPerformanceResultsCollection> OperatorPerformanceParameter {
      get { return (ScopeTreeLookupParameter<OperatorPerformanceResultsCollection>) Parameters[OperatorPerformanceParameterName]; }
    }
    public ILookupParameter<ResultCollection> AggregatedOperatorPerformanceParameter {
      get { return (LookupParameter<ResultCollection>) Parameters[AggregatedOperatorPerformanceParameterName]; }
    }

    [StorableConstructor]
    private OperatorPerformanceASRSolutionAnalyzer(bool deserializing) : base(deserializing) { }
    private OperatorPerformanceASRSolutionAnalyzer(OperatorPerformanceASRSolutionAnalyzer original, Cloner cloner) : base(original, cloner) { }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new OperatorPerformanceASRSolutionAnalyzer(this, cloner);
    }
    public OperatorPerformanceASRSolutionAnalyzer()
      : base() {
      Parameters.Add(new ScopeTreeLookupParameter<IASREncoding>(ASRSolutionParameterName, "The ASR solutions given in source code as string representation from which the best solution should be analyzed."));
      Parameters.Add(new ScopeTreeLookupParameter<OperatorPerformanceResultsCollection>(OperatorPerformanceParameterName, "The performance data of operations on the solution candidate."));
      Parameters.Add(new LookupParameter<ResultCollection>(AggregatedOperatorPerformanceParameterName, "The result collection where the aggregated operator performance values are stored."));
    }

    public override IOperation InstrumentedApply() {
      if (AggregatedOperatorPerformanceParameter.ActualValue == null)
        AggregatedOperatorPerformanceParameter.ActualValue = new ResultCollection();

      foreach (var operatorPerformance in OperatorPerformanceParameter.ActualValue.GroupBy (g => g.OperatorName)) {
        AggregatedOperatorPerformanceParameter.ActualValue.AddOrUpdateResult (operatorPerformance.Key, new DoubleValue ((double) operatorPerformance.Count (o => o.OperatorApplicable && o.OperatorCompilable) / operatorPerformance.Count ()));        
      }

      return base.InstrumentedApply();
    }
  }
}
