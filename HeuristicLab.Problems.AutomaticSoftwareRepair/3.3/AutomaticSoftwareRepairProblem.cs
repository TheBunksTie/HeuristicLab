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
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Creators;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Evaluators;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;
using HeuristicLab.Problems.AutomaticSoftwareRepair.ProblemInstances;
using HeuristicLab.Problems.Instances;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair {

  [Item("Automatic Software Repair Problem (ASR)", "Represents a generic automatic software repair problem.")]
  [Creatable(CreatableAttribute.Categories.GeneticProgrammingProblems, Priority = 100)]
  [StorableClass]
  public class AutomaticSoftwareRepairProblem: SingleObjectiveHeuristicOptimizationProblem<IASREvaluator, IASRCreator>, IStorableContent, IProblemInstanceConsumer<ASRData> {
    private const string CorrectnessSpecificationParameterName = "CorrectnessSpecification";
    private const string ProductionCodeParameterName = "ProductionCode";
    private const string ProblemInstanceParameterName = "ProblemInstance";

    public string Filename { get; set; }

    #region Parameter Properties
    public ValueParameter<StringValue> CorrectnessSpecificationParameter {
      get { return (ValueParameter<StringValue>)Parameters[CorrectnessSpecificationParameterName]; }
    }
    public ValueParameter<StringValue> ProductionCodeParameter {
      get { return (ValueParameter<StringValue>)Parameters[ProductionCodeParameterName]; }
    }
    public ValueParameter<IASRProblemInstance> ProblemInstanceParameter {
      get { return (ValueParameter<IASRProblemInstance>)Parameters[ProblemInstanceParameterName]; }
    }

    #endregion

    #region Properties
    public StringValue CorrectnessSpecification {
      get { return CorrectnessSpecificationParameter.Value; }
      set { CorrectnessSpecificationParameter.Value = value; }
    }

    public StringValue ProductionCode {
      get { return ProductionCodeParameter.Value; }
      set { ProductionCodeParameter.Value = value; }
    }
    
    public IASRProblemInstance ProblemInstance {
      get { return ProblemInstanceParameter.Value; }
      set { ProblemInstanceParameter.Value = value; }
    }

    #endregion

    [StorableConstructor]
    private AutomaticSoftwareRepairProblem(bool deserializing) : base(deserializing) { }
    private AutomaticSoftwareRepairProblem(AutomaticSoftwareRepairProblem original, Cloner cloner)
      : base(original, cloner) {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new AutomaticSoftwareRepairProblem(this, cloner);
    }

    public AutomaticSoftwareRepairProblem()
      : base(new ASRNUnitBasedEvaluator(), new ProductionCodeVariableBasedSolutionCreator()) {
      Parameters.Add(new ValueParameter<StringValue>(CorrectnessSpecificationParameterName, "The correctness specification for the production code."));
      Parameters.Add(new ValueParameter<StringValue>(ProductionCodeParameterName, "The buggy production code to be repaired."));
      Parameters.Add(new ValueParameter<IASRProblemInstance>(ProblemInstanceParameterName, "The ASR problem instance."));
     
      Maximization.Value = true;
      MaximizationParameter.Hidden = true; 

      InitializeASRProblemInstance();
      InitializeOperators();
    }

    private void InitializeASRProblemInstance () {
      var asrProblemInstance = new SimpleASRProblemInstance();
      asrProblemInstance.ProductionCode.Value = ProductionCode.Value;

      ProblemInstance = asrProblemInstance;
    }

    #region Helpers
    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      // BackwardsCompatibility3.3
      #region Backwards compatible code (remove with 3.4)
      if (Operators.Count == 0) InitializeOperators();
      #endregion
    }

    private void InitializeOperators() {  
      Operators.Clear();

      if (ProblemInstance != null) {
        Operators.AddRange (ProblemInstance.Operators.Concat (ApplicationManager.Manager.GetInstances<IASROperator>().Cast<IOperator>()).OrderBy (op => op.Name));
      }
    }

    #endregion

    public void Load (ASRData data) {
      CorrectnessSpecification.Value = data.CorrectnessSpecification;
      ProductionCode.Value = data.ProductionCode;
    }
  }
}