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
using HeuristicLab.Problems.AutomaticSoftwareRepair.Analyzer;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Evaluators {
  /// <summary>
  /// A base class for operators which evaluate ASR solutions.
  /// </summary>
  [Item ("ASREvaluator", "A base class for operators which evaluate ASR solutions.")]
  [StorableClass]
  public abstract class ASREvaluator : ASROperator, IASREvaluator {
    private const string SolutionParameterName = "ASRSolution";
    private const string QualityParameterName = "Quality";
    private const string SolutionSimilarityParameterName = "SolutionSimilarity";
    private const string OperatorPerformanceParameterName = "OperatorPerformance";

    public ILookupParameter<IASREncoding> ASRSolutionParameter {
      get { return (ILookupParameter<IASREncoding>) Parameters[SolutionParameterName]; }
    }

    public ILookupParameter<DoubleValue> SolutionSimilarityParameter {
      get { return (ILookupParameter<DoubleValue>) Parameters[SolutionSimilarityParameterName]; }
    }

    public ILookupParameter<OperatorPerformanceResultsCollection> OperatorPerformanceParameter {
      get { return (LookupParameter<OperatorPerformanceResultsCollection>)Parameters[OperatorPerformanceParameterName]; }
    }

    #region ISingleObjectiveEvaluator Members

    public ILookupParameter<DoubleValue> QualityParameter {
      get { return (ILookupParameter<DoubleValue>) Parameters[QualityParameterName]; }
    }

    #endregion

    public override bool CanChangeName {
      get { return false; }
    }

    [StorableConstructor]
    protected ASREvaluator (bool deserializing) : base (deserializing) { }
    protected ASREvaluator (ASREvaluator original, Cloner cloner) : base (original, cloner) { }
    protected ASREvaluator () {
      Parameters.Add (new LookupParameter<IASREncoding> (SolutionParameterName, "The ASR solution which should be evaluated."));
      Parameters.Add (new LookupParameter<DoubleValue> (QualityParameterName, "The evaluated quality of the ASR solution."));
      Parameters.Add (new LookupParameter<DoubleValue> (SolutionSimilarityParameterName, "The structural similarity of the current solution to an optional provided correct solution"));
      Parameters.Add (new LookupParameter<OperatorPerformanceResultsCollection> (OperatorPerformanceParameterName, "The performance data of operations on current solution."));
    }

    public override IOperation InstrumentedApply () {
      var currentSolutionProgram = ASRSolutionParameter.ActualValue.GetSolutionCode ();

      var qualityValue = Evaluate (currentSolutionProgram, ProblemInstance.CorrectnessSpecification.Value);
      QualityParameter.ActualValue = new DoubleValue (qualityValue);

      if (qualityValue < 0 && OperatorPerformanceParameter.ActualValue != null)
        OperatorPerformanceParameter.ActualValue.OperatorCompilable = false;

      return base.InstrumentedApply ();
    }

    /// <summary>
    /// Calculates the quality of production code.
    /// </summary>
    /// <param name="solutionCandidateProductionCode">The ASR solution candidate given in string representation which should be evaluated</param>
    /// <param name="correctessSpecification">The test suite acting as an orcale for correctness.</param>
    /// <returns>The calculated quality measurement.</returns>
    protected abstract double Evaluate (string solutionCandidateProductionCode, string correctessSpecification);
  }
}