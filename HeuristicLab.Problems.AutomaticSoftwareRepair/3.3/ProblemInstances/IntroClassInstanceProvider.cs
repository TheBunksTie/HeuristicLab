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
  public class IntroClassInstanceProvider : SectionStructuredFileBasedInstanceProvider {
    public override string Name {
      get { return "IntroClass for C#"; }
    }

    public override string Description {
      get { return "Manual C# port of subset of IntroClass for Java buggy programs"; }
    }

    public override Uri WebLink {
      get { return new Uri("https://github.com/Spirals-Team/IntroClassJava"); }
    }

    public override string ReferencePublication {
      get { return "Durieux, Monperrus, 2016 \"IntroClassJava: A Benchmark of  297 Small and Buggy Java Programs\", in Technical Report, University Lille 1"; }
    }

    protected override string FileName {
      get { return "IntroClass"; }
    }
  }
}