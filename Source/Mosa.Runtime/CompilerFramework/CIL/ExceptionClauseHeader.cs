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
using System.Linq;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class ExceptionClauseHeader
	{
		/// <summary>
		/// 
		/// </summary>
		private List<EhClause> clauses = new List<EhClause>();

		/// <summary>
		/// 
		/// </summary>
		public List<EhClause> Clauses
		{
			get { return this.clauses; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="clause"></param>
		public void AddClause(EhClause clause)
		{
			this.Clauses.Add(clause);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public void LinkBlockToClause(Context context, BasicBlock block)
		{
			foreach (EhClause clause in this.Clauses)
			{
				if (clause.LinkBlockToClause(context, block))
					return;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Sort()
		{
			this.Clauses.Sort(delegate(EhClause left, EhClause right) 
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
