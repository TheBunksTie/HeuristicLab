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
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.ProblemInstances {
  /// <summary>
  /// Describes instances of the Automatic Software Repair Problem (ASR).
  /// </summary>
  public class ASRData : IASRData {
    /// <summary>
    /// The name of the instance
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Optional! The description of the instance
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The buggy prodution code to be repaired.
    /// </summary>
    public string ProductionCode { get; set; }

    /// <summary>
    /// The correctness specification.
    /// </summary>
    public string CorrectnessSpecification { get; set; }

    /// <summary>
    /// The list of the initially failing tests.
    /// </summary>
    public IEnumerable<string> FailingTests { get; set; }
    
    /// <summary>
    /// The list of the initially passing tests.
    /// </summary>
    public IEnumerable<string> PassingTests { get; set; }

    /// <summary>
    /// The maximum quality to be achieved for the specific instance.
    /// </summary>
    public double BestKnownQuality { get; set; }
  }
}
