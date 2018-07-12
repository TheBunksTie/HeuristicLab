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
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General {
  [Item("ASRCreator", "Creates an ASR solution.")]
  [StorableClass]
  public abstract class ASRCreator : ASROperator, IASRCreator {
    private const string SolutionParameterName = "ASRSolution";

    public ILookupParameter<IASREncoding> ASRSolutionParameter {
      get { return (ILookupParameter<IASREncoding>) Parameters[SolutionParameterName]; }
    }
 
    [StorableConstructor]
    protected ASRCreator(bool deserializing) : base(deserializing) { }

    public ASRCreator()
        : base() {
      Parameters.Add(new LookupParameter<IASREncoding>(SolutionParameterName, "The ASR solution to be created."));
    }

    protected ASRCreator(ASRCreator original, Cloner cloner)
        : base(original, cloner) {
    } 
  }
}