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
using HeuristicLab.Data;
using HeuristicLab.Optimization;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Analyzer {
  [StorableClass]
  public class OperatorPerformanceResultsCollection : ResultCollection {
    
    #region result names

    protected const string OperatorNameResultName = "OperatorName";
    protected const string OperatorApplicableResultName = "OperatorApplicable";
    protected const string OperatorCompilableResultName = "OperatorCompilable";
   
    #endregion

    public OperatorPerformanceResultsCollection()
      : base() {

      Add(new Result(OperatorNameResultName, "The name of the manipulation operator whichs performance is described.", new StringValue()));
      Add(new Result(OperatorApplicableResultName, "True, if the manipulation was applicable to the syntax elements of the solutions candidate.", new BoolValue()));
      Add(new Result(OperatorCompilableResultName, "True, if the manipulation created a compilable version of the solution candidate.", new BoolValue()));

      Reset();
    }
    [StorableConstructor]
    protected OperatorPerformanceResultsCollection(bool deserializing)
      : base(deserializing) {
    }

    protected OperatorPerformanceResultsCollection(OperatorPerformanceResultsCollection original, Cloner cloner)
      : base(original, cloner) { }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new OperatorPerformanceResultsCollection(this, cloner);
    }

    #region result properties

    public string OperatorName {
      get { return ((StringValue)this[OperatorNameResultName].Value).Value; }
      set { ((StringValue)this[OperatorNameResultName].Value).Value = value; }
    }
    public bool OperatorApplicable {
      get { return ((BoolValue)this[OperatorApplicableResultName].Value).Value; }
      set { ((BoolValue)this[OperatorApplicableResultName].Value).Value = value; }
    }
    public bool OperatorCompilable {
      get { return ((BoolValue)this[OperatorCompilableResultName].Value).Value; }
      set { ((BoolValue)this[OperatorCompilableResultName].Value).Value = value; }
    }
 
    #endregion

    public void Reset() {
      OperatorApplicable = true;
      OperatorCompilable = true;
    }

    public override string ToString () {
      return string.Format("Operator: {0}, app: {1}, comp: {2}", OperatorName, OperatorApplicable, OperatorCompilable);
    }
  }
}
