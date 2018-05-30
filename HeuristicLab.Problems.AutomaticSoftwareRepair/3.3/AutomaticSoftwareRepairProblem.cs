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
using System.IO;
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.Simple.Creators;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Evaluators;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;
using HeuristicLab.Problems.AutomaticSoftwareRepair.ProblemInstances;
using HeuristicLab.Problems.Instances;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair {

  [Item("Automatic Software Repair Problem (ASR)", "Represents a generic automatic software repair problem.")]
  [Creatable(CreatableAttribute.Categories.GeneticProgrammingProblems, Priority = 900)]
  [StorableClass]
  public class AutomaticSoftwareRepairProblem: SingleObjectiveHeuristicOptimizationProblem<IASREvaluator, IASRCreator>, IStorableContent, IProblemInstanceConsumer<ASRData> {

    public string Filename { get; set; }

    #region Parameter Properties
    public ValueParameter<StringValue> CorrectnessSpecificationParameter {
      get { return (ValueParameter<StringValue>)Parameters["CorrectnessSpecification"]; }
    }
    public ValueParameter<StringValue> ProductionCodeParameter {
      get { return (ValueParameter<StringValue>)Parameters["ProductionCode"]; }
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
    #endregion

    [StorableConstructor]
    private AutomaticSoftwareRepairProblem(bool deserializing) : base(deserializing) { }
    private AutomaticSoftwareRepairProblem(AutomaticSoftwareRepairProblem original, Cloner cloner)
      : base(original, cloner) {
      RegisterEventHandlers();
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new AutomaticSoftwareRepairProblem(this, cloner);
    }

    public AutomaticSoftwareRepairProblem()
      : base(new ASRNUnitBasedEvaluator(), new TextBasedSolutionCreator()) {
      Parameters.Add(new ValueParameter<StringValue>("CorrectnessSpecification", "The correctness specification for the production code."));
      Parameters.Add(new ValueParameter<StringValue>("ProductionCode", "The buggy production code to be repaired."));
     
      Maximization.Value = true;
      MaximizationParameter.Hidden = true; 
      Evaluator.QualityParameter.ActualName = "ASRProgramSolutionQuality";

      InitializeOperators();
      RegisterEventHandlers();
    }

    #region Events
    protected override void OnSolutionCreatorChanged() {
      base.OnSolutionCreatorChanged();
      //SolutionCreator.PermutationParameter.ActualNameChanged += new EventHandler(SolutionCreator_PermutationParameter_ActualNameChanged);
      ParameterizeAnalyzers();
      ParameterizeOperators();
    }
    protected override void OnEvaluatorChanged() {
      base.OnEvaluatorChanged();
      Evaluator.QualityParameter.ActualNameChanged += new EventHandler(Evaluator_QualityParameter_ActualNameChanged);
      UpdateMoveEvaluators();
      ParameterizeAnalyzers();
      //if (Evaluator is ITSPCoordinatesPathEvaluator && Coordinates != null)
      //  ClearDistanceMatrix();
    }
    private void CoordinatesParameter_ValueChanged(object sender, EventArgs e) {
      //if (Coordinates != null) {
      //  Coordinates.ItemChanged += new EventHandler<EventArgs<int, int>>(Coordinates_ItemChanged);
      //  Coordinates.Reset += new EventHandler(Coordinates_Reset);
      //}
      //if (Evaluator is ITSPCoordinatesPathEvaluator) {
      //  ParameterizeSolutionCreator();
      //  ClearDistanceMatrix();
      //}
    }
    private void Coordinates_ItemChanged(object sender, EventArgs<int, int> e) {
      //if (Evaluator is ITSPCoordinatesPathEvaluator) {
      //  ClearDistanceMatrix();
      //}
    }
    private void Coordinates_Reset(object sender, EventArgs e) {
      //if (Evaluator is ITSPCoordinatesPathEvaluator) {
      //  ParameterizeSolutionCreator();
      //  ClearDistanceMatrix();
      //}
    }
    private void SolutionCreator_PermutationParameter_ActualNameChanged(object sender, EventArgs e) {
      ParameterizeAnalyzers();
      ParameterizeOperators();
    }
    private void Evaluator_QualityParameter_ActualNameChanged(object sender, EventArgs e) {
      ParameterizeAnalyzers();
    }
    #endregion

    #region Helpers
    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      // BackwardsCompatibility3.3
      #region Backwards compatible code (remove with 3.4)
      //OptionalValueParameter<DoubleMatrix> oldDistanceMatrixParameter = Parameters["DistanceMatrix"] as OptionalValueParameter<DoubleMatrix>;
      //if (oldDistanceMatrixParameter != null) {
      //  Parameters.Remove(oldDistanceMatrixParameter);
      //  Parameters.Add(new OptionalValueParameter<DistanceMatrix>("DistanceMatrix", "The matrix which contains the distances between the cities."));
      //  DistanceMatrixParameter.GetsCollected = oldDistanceMatrixParameter.GetsCollected;
      //  DistanceMatrixParameter.ReactOnValueToStringChangedAndValueItemImageChanged = false;
      //  if (oldDistanceMatrixParameter.Value != null) {
      //    DoubleMatrix oldDM = oldDistanceMatrixParameter.Value;
      //    DistanceMatrix newDM = new DistanceMatrix(oldDM.Rows, oldDM.Columns, oldDM.ColumnNames, oldDM.RowNames);
      //    newDM.SortableView = oldDM.SortableView;
      //    for (int i = 0; i < newDM.Rows; i++)
      //      for (int j = 0; j < newDM.Columns; j++)
      //        newDM[i, j] = oldDM[i, j];
      //    DistanceMatrixParameter.Value = (DistanceMatrix)newDM.AsReadOnly();
      //  }
      //}

      //ValueParameter<DoubleMatrix> oldCoordinates = (Parameters["Coordinates"] as ValueParameter<DoubleMatrix>);
      //if (oldCoordinates != null) {
      //  Parameters.Remove(oldCoordinates);
      //  Parameters.Add(new OptionalValueParameter<DoubleMatrix>("Coordinates", "The x- and y-Coordinates of the cities.", oldCoordinates.Value, oldCoordinates.GetsCollected));
      //}

      if (Operators.Count == 0) InitializeOperators();
      #endregion
      RegisterEventHandlers();
    }

    private void RegisterEventHandlers() {
      //CoordinatesParameter.ValueChanged += new EventHandler(CoordinatesParameter_ValueChanged);
      //if (Coordinates != null) {
      //  Coordinates.ItemChanged += new EventHandler<EventArgs<int, int>>(Coordinates_ItemChanged);
      //  Coordinates.Reset += new EventHandler(Coordinates_Reset);
      //}
      //SolutionCreator.PermutationParameter.ActualNameChanged += new EventHandler(SolutionCreator_PermutationParameter_ActualNameChanged);
      Evaluator.QualityParameter.ActualNameChanged += new EventHandler(Evaluator_QualityParameter_ActualNameChanged);
    }

    private void InitializeOperators() {
      //Operators.Add(new TSPImprovementOperator());
      //Operators.Add(new TSPMultipleGuidesPathRelinker());
      //Operators.Add(new TSPPathRelinker());
      //Operators.Add(new TSPSimultaneousPathRelinker());

      //Operators.Add(new HammingSimilarityCalculator());
      //Operators.Add(new QualitySimilarityCalculator());
      //Operators.Add(new PopulationSimilarityAnalyzer(Operators.OfType<ISolutionSimilarityCalculator>()));

      //Operators.Add(new BestTSPSolutionAnalyzer());
      //Operators.Add(new TSPAlleleFrequencyAnalyzer());
      //ParameterizeAnalyzers();
      //var operators = new HashSet<IPermutationOperator>(new IPermutationOperator[] {
      //  new OrderCrossover2(),
      //  new InversionManipulator(),
      //  new StochasticInversionMultiMoveGenerator()
      //}, new TypeEqualityComparer<IPermutationOperator>());
      //foreach (var op in ApplicationManager.Manager.GetInstances<IPermutationOperator>())
      //  operators.Add(op);
      //Operators.AddRange(operators);
      //ParameterizeOperators();
      //UpdateMoveEvaluators();
    }
    private void UpdateMoveEvaluators() {
      //Operators.RemoveAll(x => x is ISingleObjectiveMoveEvaluator);
      //foreach (var op in ApplicationManager.Manager.GetInstances<ITSPMoveEvaluator>())
      //  if (op.EvaluatorType == Evaluator.GetType()) {
      //    Operators.Add(op);
      //  }
      //ParameterizeOperators();
      //OnOperatorsChanged();
    }
  
    private void ParameterizeAnalyzers() {
      //if (BestTSPSolutionAnalyzer != null) {
      //  BestTSPSolutionAnalyzer.QualityParameter.ActualName = Evaluator.QualityParameter.ActualName;
      //  BestTSPSolutionAnalyzer.CoordinatesParameter.ActualName = CoordinatesParameter.Name;
      //  BestTSPSolutionAnalyzer.PermutationParameter.ActualName = SolutionCreator.PermutationParameter.ActualName;
      //  BestTSPSolutionAnalyzer.ResultsParameter.ActualName = "Results";
      //  BestTSPSolutionAnalyzer.BestKnownQualityParameter.ActualName = BestKnownQualityParameter.Name;
      //  BestTSPSolutionAnalyzer.BestKnownSolutionParameter.ActualName = BestKnownSolutionParameter.Name;
      //  BestTSPSolutionAnalyzer.MaximizationParameter.ActualName = MaximizationParameter.Name;
      //}

      //if (TSPAlleleFrequencyAnalyzer != null) {
      //  TSPAlleleFrequencyAnalyzer.MaximizationParameter.ActualName = MaximizationParameter.Name;
      //  TSPAlleleFrequencyAnalyzer.CoordinatesParameter.ActualName = CoordinatesParameter.Name;
      //  TSPAlleleFrequencyAnalyzer.DistanceMatrixParameter.ActualName = DistanceMatrixParameter.Name;
      //  TSPAlleleFrequencyAnalyzer.SolutionParameter.ActualName = SolutionCreator.PermutationParameter.ActualName;
      //  TSPAlleleFrequencyAnalyzer.QualityParameter.ActualName = Evaluator.QualityParameter.ActualName;
      //  TSPAlleleFrequencyAnalyzer.BestKnownSolutionParameter.ActualName = BestKnownSolutionParameter.Name;
      //  TSPAlleleFrequencyAnalyzer.ResultsParameter.ActualName = "Results";
      //}
    }
    private void ParameterizeOperators() {
      //foreach (IPermutationCrossover op in Operators.OfType<IPermutationCrossover>()) {
      //  op.ParentsParameter.ActualName = SolutionCreator.PermutationParameter.ActualName;
      //  op.ParentsParameter.Hidden = true;
      //  op.ChildParameter.ActualName = SolutionCreator.PermutationParameter.ActualName;
      //  op.ChildParameter.Hidden = true;
      //}
      //foreach (IPermutationManipulator op in Operators.OfType<IPermutationManipulator>()) {
      //  op.PermutationParameter.ActualName = SolutionCreator.PermutationParameter.ActualName;
      //  op.PermutationParameter.Hidden = true;
      //}
      //foreach (IPermutationMoveOperator op in Operators.OfType<IPermutationMoveOperator>()) {
      //  op.PermutationParameter.ActualName = SolutionCreator.PermutationParameter.ActualName;
      //  op.PermutationParameter.Hidden = true;
      //}
      //foreach (ITSPPathMoveEvaluator op in Operators.OfType<ITSPPathMoveEvaluator>()) {
      //  op.CoordinatesParameter.ActualName = CoordinatesParameter.Name;
      //  op.CoordinatesParameter.Hidden = true;
      //  op.DistanceMatrixParameter.ActualName = DistanceMatrixParameter.Name;
      //  op.DistanceMatrixParameter.Hidden = true;
      //  op.UseDistanceMatrixParameter.ActualName = UseDistanceMatrixParameter.Name;
      //  op.UseDistanceMatrixParameter.Hidden = true;
      //  op.QualityParameter.ActualName = Evaluator.QualityParameter.ActualName;
      //  op.QualityParameter.Hidden = true;
      //  op.PermutationParameter.ActualName = SolutionCreator.PermutationParameter.ActualName;
      //  op.PermutationParameter.Hidden = true;
      //}
      //foreach (IPermutationMultiNeighborhoodShakingOperator op in Operators.OfType<IPermutationMultiNeighborhoodShakingOperator>()) {
      //  op.PermutationParameter.ActualName = SolutionCreator.PermutationParameter.ActualName;
      //  op.PermutationParameter.Hidden = true;
      //}
      //foreach (ISingleObjectiveImprovementOperator op in Operators.OfType<ISingleObjectiveImprovementOperator>()) {
      //  op.SolutionParameter.ActualName = SolutionCreator.PermutationParameter.ActualName;
      //  op.SolutionParameter.Hidden = true;
      //}
      //foreach (ISingleObjectivePathRelinker op in Operators.OfType<ISingleObjectivePathRelinker>()) {
      //  op.ParentsParameter.ActualName = SolutionCreator.PermutationParameter.ActualName;
      //  op.ParentsParameter.Hidden = true;
      //}
      //foreach (ISolutionSimilarityCalculator op in Operators.OfType<ISolutionSimilarityCalculator>()) {
      //  op.SolutionVariableName = SolutionCreator.PermutationParameter.ActualName;
      //  op.QualityVariableName = Evaluator.QualityParameter.ActualName;
      //}
    }
    #endregion

    public void Load(TSPData data) {
      //if (data.Coordinates == null && data.Distances == null)
      //  throw new System.IO.InvalidDataException("The given instance specifies neither coordinates nor distances!");
      //if (data.Dimension > DistanceMatrixSizeLimit && (data.DistanceMeasure == DistanceMeasure.Att
      //  || data.DistanceMeasure == DistanceMeasure.Manhattan
      //  || data.DistanceMeasure == DistanceMeasure.Maximum))
      //  throw new System.IO.InvalidDataException("The given instance uses an unsupported distance measure and is too large for using a distance matrix.");
      //if (data.Coordinates != null && data.Coordinates.GetLength(1) != 2)
      //  throw new System.IO.InvalidDataException("The coordinates of the given instance are not in the right format, there need to be one row for each customer and two columns for the x and y coordinates.");

      //Name = data.Name;
      //Description = data.Description;

      //bool clearCoordinates = false, clearDistanceMatrix = false;
      //if (data.Coordinates != null && data.Coordinates.GetLength(0) > 0)
      //  Coordinates = new DoubleMatrix(data.Coordinates);
      //else clearCoordinates = true;

      //TSPEvaluator evaluator;
      //if (data.DistanceMeasure == DistanceMeasure.Att
      //  || data.DistanceMeasure == DistanceMeasure.Manhattan
      //  || data.DistanceMeasure == DistanceMeasure.Maximum) {
      //  evaluator = new TSPDistanceMatrixEvaluator();
      //  UseDistanceMatrix = new BoolValue(true);
      //  DistanceMatrix = new DistanceMatrix(data.GetDistanceMatrix());
      //} else if (data.DistanceMeasure == DistanceMeasure.Direct && data.Distances != null) {
      //  evaluator = new TSPDistanceMatrixEvaluator();
      //  UseDistanceMatrix = new BoolValue(true);
      //  DistanceMatrix = new DistanceMatrix(data.Distances);
      //} else {
      //  clearDistanceMatrix = true;
      //  UseDistanceMatrix = new BoolValue(data.Dimension <= DistanceMatrixSizeLimit);
      //  switch (data.DistanceMeasure) {
      //    case DistanceMeasure.Euclidean:
      //      evaluator = new TSPEuclideanPathEvaluator();
      //      break;
      //    case DistanceMeasure.RoundedEuclidean:
      //      evaluator = new TSPRoundedEuclideanPathEvaluator();
      //      break;
      //    case DistanceMeasure.UpperEuclidean:
      //      evaluator = new TSPUpperEuclideanPathEvaluator();
      //      break;
      //    case DistanceMeasure.Geo:
      //      evaluator = new TSPGeoPathEvaluator();
      //      break;
      //    default:
      //      throw new InvalidDataException("An unknown distance measure is given in the instance!");
      //  }
      //}
      //evaluator.QualityParameter.ActualName = "TSPTourLength";
      //Evaluator = evaluator;

      //// reset them after assigning the evaluator
      //if (clearCoordinates) Coordinates = null;
      //if (clearDistanceMatrix) DistanceMatrix = null;

      //BestKnownSolution = null;
      //BestKnownQuality = null;

      //if (data.BestKnownTour != null) {
      //  try {
      //    EvaluateAndLoadTour(data.BestKnownTour);
      //  } catch (InvalidOperationException) {
      //    if (data.BestKnownQuality.HasValue)
      //      BestKnownQuality = new DoubleValue(data.BestKnownQuality.Value);
      //  }
      //} else if (data.BestKnownQuality.HasValue) {
      //  BestKnownQuality = new DoubleValue(data.BestKnownQuality.Value);
      //}
      //OnReset();
    }

    public void EvaluateAndLoadTour(int[] tour) {
      //var route = new Permutation(PermutationTypes.RelativeUndirected, tour);
      //BestKnownSolution = route;

      //double quality;
      //if (Evaluator is ITSPDistanceMatrixEvaluator) {
      //  quality = TSPDistanceMatrixEvaluator.Apply(DistanceMatrix, route);
      //} else if (Evaluator is ITSPCoordinatesPathEvaluator) {
      //  quality = TSPCoordinatesPathEvaluator.Apply((TSPCoordinatesPathEvaluator)Evaluator, Coordinates, route);
      //} else {
      //  throw new InvalidOperationException("Cannot calculate solution quality, evaluator type is unknown.");
      //}
      //BestKnownQuality = new DoubleValue(quality);
    }

    public void Load (ASRData data) {
      CorrectnessSpecification.Value = data.CorrectnessSpecification;
      ProductionCode.Value = data.ProductionCode;
    }
  }
}