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
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HeuristicLab.Problems.AutomaticSoftwareRepair.Encodings.NetCompilerPlatform.Util {
  public static class StatementExtractionUtility {
    public static StatementSyntax[] GetAllStatements (SyntaxNode rootNode, Func<StatementSyntax, bool> whereCondition = null) {
      var statements = rootNode.DescendantNodesAndSelf()
          .OfType<StatementSyntax>()
          .SelectMany (s => GetAllStatementsInternal (s))
          .Distinct();

      if (whereCondition != null)
        statements = statements.Where (whereCondition);

      return statements.ToArray();
    }

    private static IList<StatementSyntax> GetSubStatements<T> (T statementSyntax, Func<T, StatementSyntax> statementFunc)
      where T: StatementSyntax {

      var subStatements = GetAllStatementsInternal (statementFunc (statementSyntax));
      return subStatements;
    }

    private static IList<StatementSyntax> GetSubStatements<T> (T statementSyntax, IEnumerable<StatementSyntax> statements)
      where T: StatementSyntax {
      
      var subStatements = statements.SelectMany(s => GetAllStatementsInternal(s)).ToList();
      return subStatements;
    }

    private static IList<StatementSyntax> GetAllStatementsInternal (StatementSyntax statementSyntax) {

      var blockSyntax = statementSyntax as BlockSyntax;
      if (blockSyntax != null)
        return GetSubStatements(blockSyntax, blockSyntax.Statements);

      var checkedStatementSyntax = statementSyntax as CheckedStatementSyntax;
      if (checkedStatementSyntax != null)
        return GetSubStatements(checkedStatementSyntax, s => s.Block);

      var doStatementSyntax = statementSyntax as DoStatementSyntax;
      if (doStatementSyntax != null)
        return GetSubStatements (doStatementSyntax, s => s.Statement);

      var fixedStatementSyntax = statementSyntax as FixedStatementSyntax;
      if (fixedStatementSyntax != null)
        return GetSubStatements (fixedStatementSyntax, s => s.Statement);

      var forStatementSyntax = statementSyntax as ForStatementSyntax;
      if (forStatementSyntax != null)
        return GetSubStatements (forStatementSyntax, s => s.Statement);

      var forEachStatementSyntax = statementSyntax as ForEachStatementSyntax;
      if (forEachStatementSyntax != null)
        return GetSubStatements (forEachStatementSyntax, s => s.Statement);
     
      var ifStatementSyntax = statementSyntax as IfStatementSyntax;
      if (ifStatementSyntax != null)
        return GetSubStatements (ifStatementSyntax, s => s.Statement);

      var labeledStatementSyntax = statementSyntax as LabeledStatementSyntax;
      if (labeledStatementSyntax != null)
        return GetSubStatements (labeledStatementSyntax, s => s.Statement);

      var lockStatementSyntax = statementSyntax as LockStatementSyntax;
      if (lockStatementSyntax != null)
        return GetSubStatements (lockStatementSyntax, s => s.Statement);

      var tryStatementSyntax = statementSyntax as TryStatementSyntax;
      if (tryStatementSyntax != null)
        return GetSubStatements (tryStatementSyntax, s => s.Block);

      var unsafeStatementSyntax = statementSyntax as UnsafeStatementSyntax;
      if (unsafeStatementSyntax != null)
        return GetSubStatements (unsafeStatementSyntax, s => s.Block);

      var usingStatementSyntax = statementSyntax as UsingStatementSyntax;
      if (usingStatementSyntax != null)
        return GetSubStatements (usingStatementSyntax, s => s.Statement);

      var whileStatementSyntax = statementSyntax as WhileStatementSyntax;
      if (whileStatementSyntax != null)
        return GetSubStatements (whileStatementSyntax, s => s.Statement);

      return new[] { statementSyntax };
    }
  }
}