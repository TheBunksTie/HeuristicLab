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
using HeuristicLab.Core.Views;
using HeuristicLab.MainForm;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Analyzer;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.View {
  /// <summary>
  /// The base class for visual representations of a solution program for an ASR problem.
  /// </summary>
  [View("ASRSolution View")]
  [Content(typeof(ASRSolution), true)]
  public partial class ASRSolutionView : ItemView {
    public new ASRSolution Content {
      get { return (ASRSolution)base.Content; }
      set { base.Content = value; }
    }

    public ASRSolutionView() {
      InitializeComponent();
    }

    protected override void DeregisterContentEvents() {
      Content.SolutionChanged -= Content_SolutionChanged;
      base.DeregisterContentEvents();
    }
    protected override void RegisterContentEvents() {
      base.RegisterContentEvents();
      Content.SolutionChanged += Content_SolutionChanged;
    }

    private void UpdateContentView() {
      if (Content != null && Content.Solution != null) {
        solutionCodeTextBox.Text = Content.Solution.GetSolutionCode();
        evaluatedSolutionsTextBox.Text = Content.EvaluatedSolutions.Value.ToString();
      }
    }

    protected override void OnContentChanged() {
      base.OnContentChanged();
      if (Content != null) {
        UpdateContentView();
      }
    }

    private void Content_SolutionChanged(object sender, EventArgs e) {
      if (InvokeRequired)
        Invoke(new EventHandler(Content_SolutionChanged), sender, e);
      else {
        UpdateContentView();
      }
    }

    protected override void OnReadOnlyChanged() {
      base.OnReadOnlyChanged();
      SetEnabledStateOfControls();
    }

    protected override void SetEnabledStateOfControls() {
      base.SetEnabledStateOfControls();
      tabControl.Enabled = Content != null;
    }
  }
}
