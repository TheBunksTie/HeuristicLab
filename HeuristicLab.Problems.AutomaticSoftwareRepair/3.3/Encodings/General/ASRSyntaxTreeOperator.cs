//#region License Information
///* HeuristicLab
// * Copyright (C) 2002-2018 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
// *
// * This file is part of HeuristicLab.
// *
// * HeuristicLab is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * HeuristicLab is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
// */
//#endregion

//using System;
//using HeuristicLab.Common;
//using HeuristicLab.Core;
//using HeuristicLab.Optimization;
//using HeuristicLab.Parameters;
//using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
//using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

//namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General {
//  /// <summary>
//  /// A base class for operators for automatic software repair expression trees.
//  /// </summary>
//  [Item("ASRSyntaxTreeOperator", " A base class for operators for automatic software repair expression trees.")]
//  [StorableClass]
//  public abstract class ASRSyntaxTreeOperator : ASROperator, IASRSyntaxTreeOperator, IStochasticOperator {
//    private const string RandomParameterName = "Random";
//    private const string SyntaxTreeParameterName = "SyntaxTreeParameter";

//    public override bool CanChangeName {
//      get { return false; }
//    }

//    #region Parameter Properties
  //    public ILookupParameter<IRandom> RandomParameter {
  //      get { return (LookupParameter<IRandom>)Parameters[RandomParameterName]; }
  //    }
//    public ILookupParameter<IASRSyntaxTree> SyntaxTreeParameter {
//      get { return (ILookupParameter<IASRSyntaxTree>)Parameters[SyntaxTreeParameterName]; }
//    }
//    #endregion

//    [StorableConstructor]
//    protected ASRSyntaxTreeOperator(bool deserializing) : base(deserializing) { }
//    protected ASRSyntaxTreeOperator(ASRSyntaxTreeOperator original, Cloner cloner) : base(original, cloner) { }
//    protected ASRSyntaxTreeOperator()
//        : base() {
//      Parameters.Add(new LookupParameter<IRandom>(RandomParameterName, "The pseudo random number generator which should be used for symbolic expression tree operators."));
//      Parameters.Add(new LookupParameter<IASRSyntaxTree>(SyntaxTreeParameterName, "The syntax tree on which the operator should be applied."));
//    }



//  }
//}