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
using System.IO;
using System.Linq;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.ProblemInstances {
  public abstract class SectionStructuredFileBasedInstanceProvider : ASRInstanceProvider<ASRData> {
    private const string PassingTestsSectionName = "//<PassingTestsSection>";
    private const string FailingTestsSectionName = "//<FailingTestsSection>";
    private const string BestKnownQualitySectionName = "//<BestKnownQualitySection>";
    private const string CorrectnessSpecificationSectionName = "//<CorrectnessSpecificationSection>";
    private const string ProductionCodeSectionName = "//<ProductionCodeSection>";
    private const string CorrectSolutionSectionName = "//<CorrectSolutionSection>";

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
          new[] {
                    PassingTestsSectionName,
                    FailingTestsSectionName,
                    BestKnownQualitySectionName,
                    CorrectnessSpecificationSectionName,
                    ProductionCodeSectionName,
                    CorrectSolutionSectionName
                },
          StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();

      var instance = new ASRData {
                                     PassingTests = ExtractTests(structuredFileContent[0]),
                                     FailingTests = ExtractTests(structuredFileContent[1]),
                                     BestKnownQuality = double.Parse(structuredFileContent[2]),
                                     CorrectnessSpecification = structuredFileContent[3],
                                     ProductionCode = structuredFileContent[4],
                                     CorrectSolution = structuredFileContent[5],
                                 };

      return instance;
    }

    private static IEnumerable<string> ExtractTests (string testString) {
      if (testString == "-")
        return new string[0];
      return testString.Split(new [] {','},StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
    }
  }
}