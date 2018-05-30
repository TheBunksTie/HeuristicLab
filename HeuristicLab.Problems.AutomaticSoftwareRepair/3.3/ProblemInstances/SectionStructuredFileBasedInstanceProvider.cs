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
using System.IO;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.ProblemInstances {
  public abstract class SectionStructuredFileBasedInstanceProvider : ASRInstanceProvider<ASRData> {
    private const string CorrectnessSpecificationSectionName = "<CorrectnessSpecificationSection>";
    private const string ProductionCodeSectionName = "<ProductionCodeSection>";

    protected override ASRData LoadData(Stream stream) {
      using (var reader = new StreamReader(stream)) {
        var fileContent = reader.ReadToEnd();
        return CreateInstanceFromFileContent(fileContent);
      }
    }

    public override bool CanImportData {
      get { return false; }
    }

    public override ASRData ImportData(string path) {
      var fileContent = File.ReadAllText(path);
      return CreateInstanceFromFileContent(fileContent);
    }

    private static ASRData CreateInstanceFromFileContent (string fileContent) {
      var structuredFileContent = fileContent.Split (
          new[] { CorrectnessSpecificationSectionName, ProductionCodeSectionName },
          StringSplitOptions.RemoveEmptyEntries);

      var instance = new ASRData {
                                     CorrectnessSpecification = structuredFileContent[0],
                                     ProductionCode = structuredFileContent[1]
                                 };

      return instance;
    }
  }
}