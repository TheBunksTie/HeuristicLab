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
using HeuristicLab.PluginInfrastructure;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Analyzer;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Creators;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Evaluators;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;
using HeuristicLab.Problems.AutomaticSoftwareRepair.ProblemInstances;
using HeuristicLab.Problems.Instances;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair {

  [Item ("Automatic Software Repair Problem (ASR)", "Represents a generic automatic software repair problem.")]
  [Creatable (CreatableAttribute.Categories.GeneticProgrammingProblems, Priority = 100)]
  [StorableClass]
  public class AutomaticSoftwareRepairProblem : SingleObjectiveHeuristicOptimizationProblem<IASREvaluator, IASRCreator>, IStorableContent, IProblemInstanceConsumer<ASRData> {
    private const string ProblemInstanceParameterName = "ProblemInstance";
    private const string CreationModificationProbabilityParameterName = "CreationModificationProbability";

    public string Filename { get; set; }

    #region Parameter Properties
    public ValueParameter<IASRProblemInstance> ProblemInstanceParameter {
      get { return (ValueParameter<IASRProblemInstance>) Parameters[ProblemInstanceParameterName]; }
    }

    #endregion

    #region Properties
    public IASRProblemInstance ProblemInstance {
      get { return ProblemInstanceParameter.Value; }
      set { ProblemInstanceParameter.Value = value; }
    }

    #endregion

    [StorableConstructor]
    private AutomaticSoftwareRepairProblem (bool deserializing) : base (deserializing) { }
    private AutomaticSoftwareRepairProblem (AutomaticSoftwareRepairProblem original, Cloner cloner)
      : base (original, cloner) {
    }

    public override IDeepCloneable Clone (Cloner cloner) {
      return new AutomaticSoftwareRepairProblem (this, cloner);
    }

    public AutomaticSoftwareRepairProblem ()
      : base (new ASRNUnitBasedEvaluator (), new ModifyingProductionCodeSolutionCreator ()) {
      Parameters.Add (new ValueParameter<IASRProblemInstance> (ProblemInstanceParameterName, "The ASR problem instance."));
      Parameters.Add (new ValueParameter<PercentValue> (CreationModificationProbabilityParameterName, "The probability of modifications to the initial solution candidates", new PercentValue (0.5)));

      Maximization.Value = true;
      MaximizationParameter.Hidden = true;
    }

    private void InitializeOperators () {
      Operators.Clear ();

      if (ProblemInstance != null) {
        Operators.AddRange (ProblemInstance.Operators.Concat (ApplicationManager.Manager.GetInstances<IASROperator> ().Cast<IOperator> ()).OrderBy (op => op.Name));
      }

      Operators.Add (new BestASRSolutionAnalyzer ());
      Operators.Add (new OperatorPerformanceASRSolutionAnalyzer ());

      ParameterizeOperators ();
    }

    private void ParameterizeOperators () {
      foreach (var op in Operators.OfType<IOperator> ()) {
        if (op is IMultiASROperator) {
          (op as IMultiASROperator).SetOperators (Operators.OfType<IOperator> ());
        }
      }
    }

    public void Load (ASRData data) {     
      BestKnownQuality = new DoubleValue (data.BestKnownQuality);

      InitializeProblemInstance (data);
      InitializeOperators ();
    }

    private void InitializeProblemInstance (ASRData data)
    {
      var asrProblemInstance = new SimpleASRProblemInstance {
                                                                ProductionCode = { Value = data.ProductionCode },
                                                                CorrectnessSpecification = { Value = data.CorrectnessSpecification },
                                                                CorrectSolution = { Value = data.CorrectSolution }
                                                            };

      asrProblemInstance.PassingTestsParameter.Value = new ItemArray<StringValue> (data.PassingTests.Select (s => new StringValue (s)));
      asrProblemInstance.FailingTestsParameter.Value = new ItemArray<StringValue> (data.FailingTests.Select (s => new StringValue (s)));

      ProblemInstance = asrProblemInstance;
    }
  }
}