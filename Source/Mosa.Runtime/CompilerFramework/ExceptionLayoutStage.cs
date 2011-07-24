/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	public sealed class ExceptionLayoutStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		#region Data members

		private List<Pair> sortedClauses;

		#endregion // Data members

		#region IPipelineStage members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value></value>
		string IPipelineStage.Name { get { return @"ExceptionLayoutStage"; } }

		#endregion // IPipelineStage Members

		#region IMethodCompilerStage members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			BuildSort();
		}

		#endregion // IMethodCompilerStage members

		#region Sort Class and Methods
		
		private class Pair : IComparable<Pair>
		{
			public ExceptionClause Clause { get; private set; }

			public int Start { get { return Clause.TryOffset; } }
			public int Length { get { return Clause.TryLength; } }

			public Pair Parent;
			public List<Pair> Children;

			public Pair(ExceptionClause clause)
			{
				Clause = clause;
			}

			public bool IsInside(Pair pair)
			{
				return this.Start > pair.Start && (this.Start + this.Length < pair.Start + pair.Length);
			}

			public int CompareTo(Pair other)
			{
				return this.Start.CompareTo(other.Start);
			}

		}

		private void BuildSort()
		{
			sortedClauses = new List<Pair>(methodCompiler.ExceptionClauseHeader.Clauses.Count);

			foreach (ExceptionClause clause in methodCompiler.ExceptionClauseHeader.Clauses)
				sortedClauses.Add(new Pair(clause));
		}

		private List<Pair> Sort(List<Pair> listToSort)
		{
			var result = new List<Pair>();
			listToSort.Sort();
			foreach (var pair in listToSort)
			{
				pair.Children = this.GetChildren(pair, listToSort);
				pair.Children.Sort();
			}

			var root = listToSort[0];
			result.Add(root);
			InsertChildren(result, root);
			return result;
		}

		private void InsertChildren(List<Pair> result, Pair root)
		{
			result.InsertRange(result.IndexOf(root), root.Children);
			foreach (var pair in root.Children)
				InsertChildren(result, pair);
		}

		private List<Pair> GetChildren(Pair pair, List<Pair> list)
		{
			var children = new List<Pair>();
			foreach (var other in list)
			{
				if (other.IsInside(pair))
					children.Add(other);
			}

			var result = new List<Pair>(children);
			foreach (var x in children)
			{
				foreach (var y in children)
				{
					if (x.IsInside(y))
						result.Remove(x);
				}
			}

			foreach (var x in result)
				x.Parent = pair;

			return result;
		}

		#endregion

	}
}
