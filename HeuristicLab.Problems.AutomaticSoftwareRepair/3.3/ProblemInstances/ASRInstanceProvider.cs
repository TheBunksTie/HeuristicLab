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
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using HeuristicLab.Common;
using HeuristicLab.Problems.AutomaticSoftwareRepair.Interfaces;
using HeuristicLab.Problems.Instances;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.ProblemInstances {

  public abstract class ASRInstanceProvider<TData> : ProblemInstanceProvider<TData>, IASRInstanceProvider<TData> where TData : IASRData {

      protected abstract string FileName { get; }

      public override IEnumerable<IDataDescriptor> GetDataDescriptors() {
        var instanceArchiveName = GetResourceName (FileName + @"\.zip");
        if (string.IsNullOrEmpty (instanceArchiveName)) yield break;

        using (var instanceStream = new ZipArchive(GetType().Assembly.GetManifestResourceStream(instanceArchiveName), ZipArchiveMode.Read)) {
          foreach (var entry in instanceStream.Entries.Select (x => x.Name).OrderBy (x => x, new NaturalStringComparer())) {
            yield return new ASRDataDescriptor (Path.GetFileNameWithoutExtension (entry), GetInstanceDescription(), entry);
          }
        }
      }

      public override TData LoadData(IDataDescriptor id) {
        var descriptor = (ASRDataDescriptor)id;
        var instanceArchiveName = GetResourceName(FileName + @"\.zip");
        using (var instancesZipFile = new ZipArchive (GetType().Assembly.GetManifestResourceStream (instanceArchiveName))) {
          var entry = instancesZipFile.GetEntry (descriptor.InstanceIdentifier);
          using (var stream = entry.Open()) {
            var instance = LoadData (stream);
            if (string.IsNullOrEmpty (instance.Name))
              instance.Name = Path.GetFileNameWithoutExtension (entry.ToString());
            return instance;
          }
        }
      }

    #region IASRInstanceProvider
    public TData Import(string asrFile) {
      var data = ImportData(asrFile);
      return data;
    }

    public void Export(TData instance, string path) {
      ExportData(instance, path);
    }
    #endregion

    protected abstract TData LoadData (Stream stream);

    #region Helpers
    protected virtual string GetResourceName(string fileName) {
      return Assembly.GetExecutingAssembly().GetManifestResourceNames()
          .SingleOrDefault(x => Regex.Match(x, @".*\.Data\." + fileName).Success);
    }

    protected virtual string GetInstanceDescription() {
      return "Embedded instance of plugin version " + Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true).Cast<AssemblyFileVersionAttribute>().First().Version + ".";
    }
    #endregion
  }
}
