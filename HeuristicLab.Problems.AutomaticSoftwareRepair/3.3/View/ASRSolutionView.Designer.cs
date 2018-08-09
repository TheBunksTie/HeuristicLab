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

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.View {
  partial class ASRSolutionView {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose (bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose ();
      }
      base.Dispose (disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent () {
      this.tabControl = new HeuristicLab.MainForm.WindowsForms.DragOverTabControl();
      this.currentBestSolutionTabPage = new System.Windows.Forms.TabPage();
      this.evaluatedSolutionsTabPage = new System.Windows.Forms.TabPage();
      this.solutionCodeTextBox = new System.Windows.Forms.TextBox();
      this.evaluatedSolutionsTextBox = new System.Windows.Forms.TextBox();
      this.tabControl.SuspendLayout();
      this.currentBestSolutionTabPage.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabControl
      // 
      this.tabControl.AllowDrop = true;
      this.tabControl.Controls.Add(this.currentBestSolutionTabPage);
      this.tabControl.Controls.Add(this.evaluatedSolutionsTabPage);
      this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl.Location = new System.Drawing.Point (0, 0);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size (468, 415);
      this.tabControl.TabIndex = 0;
      // 
      // currentBestSolutionTabPage
      // 
      this.currentBestSolutionTabPage.Controls.Add(this.solutionCodeTextBox);
      this.currentBestSolutionTabPage.Location = new System.Drawing.Point(4, 22);
      this.currentBestSolutionTabPage.Name = "currentBestSolutionTabPage";
      this.currentBestSolutionTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.currentBestSolutionTabPage.Size = new System.Drawing.Size(460, 389);
      this.currentBestSolutionTabPage.TabIndex = 1;
      this.currentBestSolutionTabPage.Text = "Current Best Solution Program";
      this.currentBestSolutionTabPage.UseVisualStyleBackColor = true;
      // 
      // evaluatedSolutionsTabPage
      // 
      this.evaluatedSolutionsTabPage.Controls.Add(this.evaluatedSolutionsTextBox);
      this.evaluatedSolutionsTabPage.Location = new System.Drawing.Point(4, 22);
      this.evaluatedSolutionsTabPage.Name = "evaluatedSolutionsTabPage";
      this.evaluatedSolutionsTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.evaluatedSolutionsTabPage.Size = new System.Drawing.Size(460, 389);
      this.evaluatedSolutionsTabPage.TabIndex = 2;
      this.evaluatedSolutionsTabPage.Text = "Evaluated solutions for current best";
      this.evaluatedSolutionsTabPage.UseVisualStyleBackColor = true;
      // 
      // solutionCodeTextBox
      // 
      this.solutionCodeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.solutionCodeTextBox.Location = new System.Drawing.Point(3, 3);
      this.solutionCodeTextBox.Multiline = true;
      this.solutionCodeTextBox.Name = "solutionCodeTextBox";
      this.solutionCodeTextBox.ReadOnly = true;
      this.solutionCodeTextBox.Size = new System.Drawing.Size(454, 383);
      this.solutionCodeTextBox.TabIndex = 0;
      // 
      // evaluatedSolutionsTextBox
      // 
      this.evaluatedSolutionsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.evaluatedSolutionsTextBox.Location = new System.Drawing.Point(3, 3);
      this.evaluatedSolutionsTextBox.Name = "evaluatedSolutionsTextBox";
      this.evaluatedSolutionsTextBox.ReadOnly = true;
      this.evaluatedSolutionsTextBox.Size = new System.Drawing.Size(454, 20);
      this.evaluatedSolutionsTextBox.TabIndex = 0;
      // 
      // ASRSolutionView
      // 
      this.Controls.Add(this.tabControl);
      this.Name = "ASRSolutionView";
      this.Size = new System.Drawing.Size(468, 415);
      this.tabControl.ResumeLayout(false);
      this.currentBestSolutionTabPage.ResumeLayout(false);
      this.currentBestSolutionTabPage.PerformLayout();
      this.evaluatedSolutionsTabPage.ResumeLayout(false);
      this.evaluatedSolutionsTabPage.PerformLayout();
      this.ResumeLayout(false);
    }

    #endregion

    private HeuristicLab.MainForm.WindowsForms.DragOverTabControl tabControl;
    private System.Windows.Forms.TabPage currentBestSolutionTabPage;
    private System.Windows.Forms.TabPage evaluatedSolutionsTabPage;
    private System.Windows.Forms.TextBox solutionCodeTextBox;
    private System.Windows.Forms.TextBox evaluatedSolutionsTextBox;
  }
}
