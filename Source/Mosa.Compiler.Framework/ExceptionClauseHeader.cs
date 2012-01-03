/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class ExceptionClauseHeader
	{
		/// <summary>
		/// 
		/// </summary>
		private List<ExceptionHandlingClause> clauses = new List<ExceptionHandlingClause>();

		/// <summary>
		/// Gets the clauses.
		/// </summary>
		/// <value>The clauses.</value>
		public List<ExceptionHandlingClause> Clauses
		{
			get { return this.clauses; }
		}

		/// <summary>
		/// Adds the clause.
		/// </summary>
		/// <param name="clause">The clause.</param>
		public void AddClause(ExceptionHandlingClause clause)
		{
			this.Clauses.Add(clause);
		}

	}
}
