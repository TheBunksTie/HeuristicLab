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
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Analyzer {
  /// <summary>
  /// An operator for analyzing the best solution of Automatic Software Repair Problems given in source code as string representation.
  /// </summary>
  [Item("BestASRSolutionAnalyzer", "An operator for analyzing the best solution of Automatic Software Repair Problems given in source code as string representation.")]
  [StorableClass]
  public sealed class BestASRSolutionAnalyzer : SingleSuccessorOperator, IAnalyzer, ISingleObjectiveOperator {
    private const string ProblemInstanceParameterName = "ProblemInstance";
    private const string QualityParameterName = "Quality";
    private const string BestSolutionParameterName = "BestSolution";
    private const string ResultsParameterName = "Results";
    private const string BestKnownQualityParameterName = "BestKnownQuality";
    private const string BestKnownSolutionParameterName = "BestKnownSolution";
    private const string ASRSolutionParameterName = "ASRSolution";

    public bool EnabledByDefault {
      get { return true; }
    }

    public ILookupParameter<IASRProblemInstance> ProblemInstanceParameter {
      get { return (LookupParameter<IASRProblemInstance>)Parameters[ProblemInstanceParameterName]; }
    }
    public ScopeTreeLookupParameter<IASREncoding> ASRSolutionParameter {
      get { return (ScopeTreeLookupParameter<IASREncoding>)Parameters[ASRSolutionParameterName]; }
    }
    public ScopeTreeLookupParameter<DoubleValue> QualityParameter {
      get { return (ScopeTreeLookupParameter<DoubleValue>) Parameters[QualityParameterName]; }
    }
    public LookupParameter<ASRSolution> BestSolutionParameter {
      get { return (LookupParameter<ASRSolution>)Parameters[BestSolutionParameterName]; }
    }
    public ValueLookupParameter<ResultCollection> ResultsParameter {
      get { return (ValueLookupParameter<ResultCollection>)Parameters[ResultsParameterName]; }
    }
    public LookupParameter<DoubleValue> BestKnownQualityParameter {
      get { return (LookupParameter<DoubleValue>) Parameters[BestKnownQualityParameterName]; }
    }
    public LookupParameter<IASREncoding> BestKnownSolutionParameter {
      get { return (LookupParameter<IASREncoding>) Parameters[BestKnownSolutionParameterName]; }
    }

    [StorableConstructor]
    private BestASRSolutionAnalyzer(bool deserializing) : base(deserializing) { }
    private BestASRSolutionAnalyzer(BestASRSolutionAnalyzer original, Cloner cloner) : base(original, cloner) { }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new BestASRSolutionAnalyzer(this, cloner);
    }
    public BestASRSolutionAnalyzer()
      : base() {
      Parameters.Add(new LookupParameter<IASRProblemInstance>(ProblemInstanceParameterName, "The initial buggy production source code."));
      Parameters.Add(new ScopeTreeLookupParameter<IASREncoding>(ASRSolutionParameterName, "The ASR solutions given in source code as string representation from which the best solution should be analyzed."));
      Parameters.Add(new ScopeTreeLookupParameter<DoubleValue>(QualityParameterName, "The qualities of the ASR solutions which should be analyzed."));
      Parameters.Add(new LookupParameter<ASRSolution>(BestSolutionParameterName, "The best ASR solution."));
      Parameters.Add(new ValueLookupParameter<ResultCollection>(ResultsParameterName, "The result collection where the best ASR solution should be stored."));
      Parameters.Add(new LookupParameter<DoubleValue>(BestKnownQualityParameterName, "The quality of the best known solution of this ASR instance."));
      Parameters.Add(new LookupParameter<IASREncoding>(BestKnownSolutionParameterName, "The best known solution of this ASR instance."));

      ProblemInstanceParameter.Hidden = true;
      ASRSolutionParameter.Hidden = true;
      QualityParameter.Hidden = true;
      BestSolutionParameter.Hidden = true;
      ResultsParameter.Hidden = true;
      BestKnownQualityParameter.Hidden = true;
      BestKnownSolutionParameter.Hidden = true;
    }

    public override IOperation Apply() {
      var problemInstance = ProblemInstanceParameter.ActualValue;
      var asrSolution = ASRSolutionParameter.ActualValue;
      var results = ResultsParameter.ActualValue;

      var qualities = QualityParameter.ActualValue;

      var i = qualities.Select((x, index) => new { index, x.Value }).OrderByDescending(x => x.Value).First().index;

      var best = asrSolution[i].Clone() as IASREncoding;
      var solution = BestSolutionParameter.ActualValue;
      if (solution == null) {
        solution = new ASRSolution(problemInstance, best.Clone() as IASREncoding, new DoubleValue(qualities[i].Value));
        BestSolutionParameter.ActualValue = solution;
        results.Add(new Result("CurrentBestSolutionProgram", solution));
      } else {
        if (solution.Quality.Value < qualities[i].Value) {
          solution.ProblemInstance = problemInstance;
          solution.Solution = (IASREncoding)asrSolution[i].Clone();
          solution.Quality.Value = qualities[i].Value;
        }
      }
      return base.Apply();
    }
  }
}
