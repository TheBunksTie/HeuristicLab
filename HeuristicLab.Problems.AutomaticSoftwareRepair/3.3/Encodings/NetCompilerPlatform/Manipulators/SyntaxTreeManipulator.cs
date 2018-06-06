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

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Manipulators
{
  [Item("SyntaxTreeManipulator", "A base class for manipulating syntax tree encoded automatic software repair solutions.")]
  [StorableClass]
  public abstract class SyntaxTreeManipulator : ASRManipulator {

    [StorableConstructor]
    protected SyntaxTreeManipulator(bool deserializing) : base(deserializing) { }

    public SyntaxTreeManipulator()
        : base() {
    }

    protected SyntaxTreeManipulator(SyntaxTreeManipulator original, Cloner cloner)
        : base(original, cloner) {
    }

    public override IOperation InstrumentedApply() {  
      Manipulate (RandomParameter.ActualValue, ASRSolutionParameter.ActualValue as SyntaxTreeEncoding);

      return base.InstrumentedApply();
    }

    protected abstract void Manipulate (IRandom random, SyntaxTreeEncoding individual);
  }
}