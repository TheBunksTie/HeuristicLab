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
  [Item("BestASRSolutionAnalyzer", "An operator for analyzing the best solution of Automatic Software Repair Problems given in source code as string representation.")]
  [StorableClass]
  public sealed class BestASRSolutionAnalyzer : ASROperator, IASRAnalyzer {
    private const string QualityParameterName = "Quality";
    private const string BestSolutionParameterName = "BestSolution";
    private const string ResultsParameterName = "Results";
    private const string BestKnownQualityParameterName = "BestKnownQuality";
    private const string ASRSolutionParameterName = "ASRSolution";
    private const string CurrentBestsolutionProgramResultName = "CurrentBestSolutionProgram";
    private const string EvaluatedSolutionsParameterName = "EvaluatedSolutions";

    public bool EnabledByDefault {
      get { return true; }
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
    public ILookupParameter<IntValue> EvaluatedSolutionsParameter {
      get { return (LookupParameter<IntValue>)Parameters[EvaluatedSolutionsParameterName]; }
    }

    [StorableConstructor]
    private BestASRSolutionAnalyzer(bool deserializing) : base(deserializing) { }
    private BestASRSolutionAnalyzer(BestASRSolutionAnalyzer original, Cloner cloner) : base(original, cloner) { }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new BestASRSolutionAnalyzer(this, cloner);
    }
    public BestASRSolutionAnalyzer()
      : base() {
      Parameters.Add(new ScopeTreeLookupParameter<IASREncoding>(ASRSolutionParameterName, "The ASR solutions given in source code as string representation from which the best solution should be analyzed."));
      Parameters.Add(new ScopeTreeLookupParameter<DoubleValue>(QualityParameterName, "The qualities of the ASR solutions which should be analyzed."));
      Parameters.Add(new LookupParameter<ASRSolution>(BestSolutionParameterName, "The best ASR solution."));
      Parameters.Add(new ValueLookupParameter<ResultCollection>(ResultsParameterName, "The result collection where the best ASR solution should be stored."));
      Parameters.Add (new LookupParameter<DoubleValue> (BestKnownQualityParameterName, "The quality of the best known solution of this ASR instance."));
      Parameters.Add (new LookupParameter<IntValue> (EvaluatedSolutionsParameterName, "The current number of evaluated solutions."));

      ProblemInstanceParameter.Hidden = true;
      ASRSolutionParameter.Hidden = true;
      QualityParameter.Hidden = true;
      BestSolutionParameter.Hidden = true;
      ResultsParameter.Hidden = true;
      BestKnownQualityParameter.Hidden = true;
      EvaluatedSolutionsParameter.Hidden = true;
    }

    public override IOperation InstrumentedApply() {
      var problemInstance = ProblemInstanceParameter.ActualValue;
      var asrSolution = ASRSolutionParameter.ActualValue;
      var results = ResultsParameter.ActualValue;
      var qualities = QualityParameter.ActualValue;

      var bestQualityIndex = qualities.Select((x, index) => new { index, x.Value }).OrderByDescending(x => x.Value).First().index;

      var bestSolutionCurrentPopulation = asrSolution[bestQualityIndex].Clone() as IASREncoding;
      var bestSolution = BestSolutionParameter.ActualValue;
      if (bestSolution == null) {
        bestSolution = new ASRSolution(problemInstance, bestSolutionCurrentPopulation.Clone() as IASREncoding, new DoubleValue(qualities[bestQualityIndex].Value), EvaluatedSolutionsParameter.ActualValue);
        BestSolutionParameter.ActualValue = bestSolution;
        results.Add(new Result(CurrentBestsolutionProgramResultName, bestSolution));
      } else {
        if (bestSolution.Quality.Value < qualities[bestQualityIndex].Value) {
          bestSolution.ProblemInstance = problemInstance;
          bestSolution.Solution = bestSolutionCurrentPopulation.Clone() as IASREncoding;
          bestSolution.Quality.Value = qualities[bestQualityIndex].Value;
          bestSolution.EvaluatedSolutions = EvaluatedSolutionsParameter.ActualValue;
        }
      }

      return base.InstrumentedApply();
    }
  }
}
