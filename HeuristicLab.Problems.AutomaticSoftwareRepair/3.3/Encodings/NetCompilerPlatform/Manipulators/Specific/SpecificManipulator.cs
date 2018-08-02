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
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Analyzer;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Manipulators.Specific 
{
  [Item ("SpecificManipulator", "A base class for specific syntax tree manipulations, which are collecting performance data of the manipulation operation.")]
  [StorableClass]
  public abstract class SpecificManipulator : SyntaxTreeManipulator {
    private const string OperatorPerformanceParameterName = "OperatorPerformance";

    public ILookupParameter<OperatorPerformanceResultsCollection> OperatorPerformanceParameter {
      get { return (LookupParameter<OperatorPerformanceResultsCollection>)Parameters[OperatorPerformanceParameterName]; }
    }

    [StorableConstructor]
    protected SpecificManipulator (bool deserializing)
        : base (deserializing) {
    }

    public SpecificManipulator ()
        : base() {
      Parameters.Add (new LookupParameter<OperatorPerformanceResultsCollection> (OperatorPerformanceParameterName, "The performance data of each solution current mutation operation."));
    }

    protected SpecificManipulator (SpecificManipulator original, Cloner cloner)
        : base (original, cloner) {
    }

  }
}