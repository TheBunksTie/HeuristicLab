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
      this.tabControl = new HeuristicLab.MainForm.WindowsForms.DragOverTabControl ();
      this.tabPage = new System.Windows.Forms.TabPage ();
      this.solutionCodeTextBox = new System.Windows.Forms.TextBox ();
      this.tabControl.SuspendLayout ();
      this.tabPage.SuspendLayout ();
      this.SuspendLayout ();
      // 
      // tabControl
      // 
      this.tabControl.Controls.Add (this.tabPage);
      this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl.Location = new System.Drawing.Point (0, 0);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size (468, 415);
      this.tabControl.TabIndex = 0;
      // 
      // tabPage
      // 
      this.tabPage.Controls.Add (solutionCodeTextBox);
      this.tabPage.Location = new System.Drawing.Point (4, 22);
      this.tabPage.Name = "tabPage";
      this.tabPage.Padding = new System.Windows.Forms.Padding (3);
      this.tabPage.Size = new System.Drawing.Size (460, 389);
      this.tabPage.TabIndex = 1;
      this.tabPage.Text = "Current Best Solution Program";
      this.tabPage.UseVisualStyleBackColor = true; 
      // 
      // solutionCodeTextBox
      // 
      this.solutionCodeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.solutionCodeTextBox.Location = new System.Drawing.Point (3, 16);
      this.solutionCodeTextBox.Multiline = true;
      this.solutionCodeTextBox.Name = "solutionCodeTextBox";
      this.solutionCodeTextBox.Size = new System.Drawing.Size (403, 507);
      this.solutionCodeTextBox.TabIndex = 0;
      this.solutionCodeTextBox.ReadOnly = true;
      // 
      // ASRSolutionView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
      this.Controls.Add (this.tabControl);
      this.Name = "ASRSolutionView";
      this.Size = new System.Drawing.Size (468, 415);
      this.tabControl.ResumeLayout (false);
      this.tabPage.ResumeLayout (false);
      this.ResumeLayout (false);
    }

    #endregion

    private HeuristicLab.MainForm.WindowsForms.DragOverTabControl tabControl;
    private System.Windows.Forms.TabPage tabPage;
    private System.Windows.Forms.TextBox solutionCodeTextBox;
  }
}
