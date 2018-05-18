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
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.General;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.Simple {
  [Item ("SimpleEncoding", "Represents a simple encoding of ASR solutions. It is implemented in a prototypical way loosely based on Le Goues et. al (2012): GenProg:...")]
  [StorableClass]
  public class SimpleEncoding : SolutionProgramEncoding {
    public SimpleEncoding(IASRProblemInstance instance)
        : base(instance) {
    }

    [StorableConstructor]
    protected SimpleEncoding(bool serializing)
        : base(serializing) {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new SimpleEncoding(this, cloner);
    }

    protected SimpleEncoding(SimpleEncoding original, Cloner cloner)
        : base(original, cloner) {
    }
  }
}