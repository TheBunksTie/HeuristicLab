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

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.ProblemInstances
{
  public class QuixBugInstanceProvider : SectionStructuredFileBasedInstanceProvider {
    public override string Name {
      get { return "Quix Bugs for C#"; }
    }

    public override string Description {
      get { return "Manual C# port of subset of Quix Bugs from buggy Java programs"; }
    }

    public override Uri WebLink {
      get { return new Uri("https://github.com/jkoppel/QuixBugs"); }
    }

    public override string ReferencePublication {
      get { return "Lin et al. 2017 \"QuixBugs: A Multi-Lingual Program Repair Benchmark Set Based on the Quixey Challenge\", in Proceedings of SPLASH (Companion), pp 55-56"; }
    }

    protected override string FileName {
      get { return "Quix"; }
    }
  }
}