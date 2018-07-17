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

using System;
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Analyzer {
  /// <summary>
  /// An operator for analyzing the best solution of Automatic Software Repair Problems given in source code as string representation.
  /// </summary>
  [Item("OperatorPerformanceASRSolutionAnalyzer", "An operator for analyzing the performance of specific operators")]
  [StorableClass]
  public sealed class OperatorPerformanceASRSolutionAnalyzer : SingleSuccessorOperator, IAnalyzer, ISingleObjectiveOperator {
    private const string ResultsParameterName = "Results";
    private const string OperatorPerformanceParameterName = "OperatorPerformance";

    public bool EnabledByDefault {
      get { return false; }
    }
    public ValueLookupParameter<ResultCollection> ResultsParameter {
      get { return (ValueLookupParameter<ResultCollection>)Parameters[ResultsParameterName]; }
    }
    public ScopeTreeLookupParameter<OperatorPerformanceResultsCollection> OperatorPerformanceParameter {
      get { return (ScopeTreeLookupParameter<OperatorPerformanceResultsCollection>) Parameters[OperatorPerformanceParameterName]; }
    }

    [StorableConstructor]
    private OperatorPerformanceASRSolutionAnalyzer(bool deserializing) : base(deserializing) { }
    private OperatorPerformanceASRSolutionAnalyzer(OperatorPerformanceASRSolutionAnalyzer original, Cloner cloner) : base(original, cloner) { }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new OperatorPerformanceASRSolutionAnalyzer(this, cloner);
    }
    public OperatorPerformanceASRSolutionAnalyzer()
      : base() {
      Parameters.Add(new ValueLookupParameter<ResultCollection>(ResultsParameterName, "The result collection where the best ASR solution should be stored."));
      Parameters.Add(new ScopeTreeLookupParameter<OperatorPerformanceResultsCollection>(OperatorPerformanceParameterName, "The performance data of operations on the solution candidate."));

      ResultsParameter.Hidden = true;
    }

    public override IOperation Apply() {
      var results = ResultsParameter.ActualValue;

      var operatorGroupedPerformance = OperatorPerformanceParameter.ActualValue
          .GroupBy (g => g.OperatorName);
         // .ToDictionary (g => g.Key, g => (double) g.Count (o => o.OperatorApplicable && o.OperatorCompilable) / g.Count());

      foreach (var operatorPerformance in operatorGroupedPerformance) {
        results.AddOrUpdateResult (operatorPerformance.Key, new DoubleValue((double) operatorPerformance.Count (o => o.OperatorApplicable && o.OperatorCompilable) / operatorPerformance.Count()));        
      }

      return base.Apply();
    }
  }
}