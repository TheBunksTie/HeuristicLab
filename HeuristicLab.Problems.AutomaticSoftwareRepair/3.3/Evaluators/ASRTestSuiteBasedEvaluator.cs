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

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Evaluators {

  /// <summary>
  /// A base class for operators which evaluate ASR solutions against a given test suite.
  /// </summary>
  [StorableClass]
  [Item("ASRTestSuiteBasedEvaluator", "A base class for operators which evaluate ASR solutions agains a given test suite.")]
  public abstract class ASRTestSuiteBasedEvaluator : ASREvaluator {
    private const string ProductionCodeParameterName = "ProductionCode";
    private const string CorrectnesSpecificationParameterName = "CorrectnessSpecification";

    public ILookupParameter<StringValue> ProductionCode {
      get { return (ILookupParameter<StringValue>)Parameters[ProductionCodeParameterName]; }
    }
    public ILookupParameter<StringValue> TestCode {
      get { return (ILookupParameter<StringValue>)Parameters[CorrectnesSpecificationParameterName]; }
    }

    [StorableConstructor]
    protected ASRTestSuiteBasedEvaluator(bool deserializing) : base(deserializing) { }
    protected ASRTestSuiteBasedEvaluator(ASRTestSuiteBasedEvaluator original, Cloner cloner) : base(original, cloner) { }
    protected ASRTestSuiteBasedEvaluator() {
      Parameters.Add(new LookupParameter<StringValue>(ProductionCodeParameterName, "The ASR solution given in string representation which should be evaluated."));
      Parameters.Add(new LookupParameter<StringValue>(CorrectnesSpecificationParameterName, "The test suite acting as an orcale for correctness."));
    }

    //[StorableHook(HookType.AfterDeserialization)]
    //private void AfterDeserialization() {
    //  // BackwardsCompatibility3.3
    //  #region Backwards compatible code (remove with 3.4)
    //  LookupParameter<DoubleMatrix> oldDistanceMatrixParameter = Parameters["DistanceMatrix"] as LookupParameter<DoubleMatrix>;
    //  if (oldDistanceMatrixParameter != null) {
    //    Parameters.Remove(oldDistanceMatrixParameter);
    //    //Parameters.Add(new LookupParameter<DistanceMatrix>("DistanceMatrix", "The matrix which contains the distances between the cities."));
    //    //DistanceMatrixParameter.ActualName = oldDistanceMatrixParameter.ActualName;
    //  }
    //  #endregion
    //}

    public override IOperation InstrumentedApply () {

      var qualityValue = Evaluate(ProductionCode.ActualValue.Value, TestCode.ActualValue.Value);

      QualityParameter.ActualValue = new DoubleValue(qualityValue);

      return base.InstrumentedApply();
    }

    //public static double Apply(ASRTestSuiteBasedEvaluator evaluator, string productionCode, string testCode) {

    //  var quality = evaluator.Evaluate(productionCode, testCode);
    //  return quality;
    //}

    /// <summary>
    /// Calculates the quality of production code.
    /// </summary>
    /// <param name="productionCode">The ASR solution candidate given in string representation which should be evaluated</param>
    /// <param name="testCode">The test suite acting as an orcale for correctness.</param>
    /// <returns>The calculated quality measurement.</returns>
    protected abstract double Evaluate (string productionCode, string testCode);
  }
} 