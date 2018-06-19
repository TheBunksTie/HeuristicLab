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
using HeuristicLab.MainForm;
using HeuristicLab.Optimization.Views;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.View {
  [View("Automatic Software Repair Problem View")]
  [Content(typeof(AutomaticSoftwareRepairProblem), true)]
  public abstract partial class ASRProblemView : ProblemView {
    public new AutomaticSoftwareRepairProblem Content {
      get { return (AutomaticSoftwareRepairProblem)base.Content; }
      set { base.Content = value; }
    }

   /// <summary>
    /// Initializes a new instance of <see cref="ASRProblemView"/>.
    /// </summary>
    public ASRProblemView() {
      InitializeComponent();
    }

    protected override void OnContentChanged() {
      base.OnContentChanged();
      if (Content == null) {
        asrSolutionView.Content = null;
      }
    }

    protected override void SetEnabledStateOfControls() {
      base.SetEnabledStateOfControls();
      asrSolutionView.Enabled = Content != null;
    }
  } 
}
