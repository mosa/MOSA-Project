/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class ExceptionClauseHeader
	{
		/// <summary>
		/// 
		/// </summary>
		private List<ExceptionClause> clauses = new List<ExceptionClause>();

		/// <summary>
		/// Gets the clauses.
		/// </summary>
		/// <value>The clauses.</value>
		public List<ExceptionClause> Clauses
		{
			get { return this.clauses; }
		}

		/// <summary>
		/// Adds the clause.
		/// </summary>
		/// <param name="clause">The clause.</param>
		public void AddClause(ExceptionClause clause)
		{
			this.Clauses.Add(clause);
		}

		/// <summary>
		/// Sorts this instance.
		/// </summary>
		public void Sort()
		{
			this.Clauses.Sort(delegate(ExceptionClause left, ExceptionClause right) 
			{
				if (left.HandlerEnd < right.HandlerOffset)
					return -1;
				if (left.HandlerOffset > right.HandlerLength && left.HandlerOffset < right.HandlerEnd)
					return -1;
				return 1;
			});
		}
	}
}
