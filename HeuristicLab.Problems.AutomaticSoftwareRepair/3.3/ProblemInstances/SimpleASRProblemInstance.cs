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
using System.Collections.Generic;
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.PluginInfrastructure;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Creators;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Evaluators;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.ProblemInstances
{
  [Item("SimpleASRProblemInstance", "Represents a small single source file based ASR problem instance.")]
  [StorableClass]
  public sealed class SimpleASRProblemInstance : ASRProblemInstance {

    protected override IEnumerable<IOperator> GetOperators() {
      return ApplicationManager.Manager.GetInstances<IASROperator>().Cast<IOperator>();
    }

    protected override IEnumerable<IOperator> GetAnalyzers () {
      return new IOperator[0];
    }

    protected override IASREvaluator Evaluator {
      get {
        return new ASRNUnitBasedEvaluator();
      }
    }

    protected override IASRCreator Creator {
      get { return new ProductionCodeVariableBasedSolutionCreator(); }
    }

    [StorableConstructor]
    protected SimpleASRProblemInstance(bool deserializing) : base(deserializing) { }

    public SimpleASRProblemInstance() {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new SimpleASRProblemInstance(this, cloner);
    }

    protected SimpleASRProblemInstance(SimpleASRProblemInstance original, Cloner cloner)
        : base(original, cloner) {
    }

  
  }
}