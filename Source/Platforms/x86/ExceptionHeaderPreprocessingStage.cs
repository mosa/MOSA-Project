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
using System.Diagnostics;
using System.Text;
using Mosa.Runtime.CompilerFramework.CIL;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	public sealed class ExceptionHeaderPreprocessingStage : BaseMethodCompilerStage, IMethodCompilerStage, IPlatformStage, IPipelineStage
	{
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<int, EhClause> labelMapping = new Dictionary<int, EhClause>();
		/// <summary>
		/// Runs the specified method compiler.
		/// </summary>
		public void Run()
		{
			ProcessExceptionClauseLabels();
			// Iterate all blocks and preprocess them
			foreach (BasicBlock block in BasicBlocks)
				ProcessBlock(block);
		}

		/// <summary>
		/// 
		/// </summary>
		private void ProcessExceptionClauseLabels()
		{
			this.labelMapping.Clear();

			foreach (EhClause clause in this.MethodCompiler.Method.ExceptionClauseHeader.Clauses)
			{
				AddClauseLabels(clause);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="clause"></param>
		private void AddClauseLabels(EhClause clause)
		{
			this.labelMapping.Add(clause.TryOffset, clause);
			this.labelMapping.Add(clause.TryEnd, clause);
			this.labelMapping.Add(clause.HandlerOffset, clause);
			this.labelMapping.Add(clause.HandlerEnd, clause);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="block"></param>
		private void ProcessBlock(BasicBlock block)
		{
			for (Context context = CreateContext(block); !context.EndOfInstruction; context.GotoNext())
			{
				if (this.labelMapping.ContainsKey(context.Label))
				{
					// TODO:
					// Add patch to method
				}
			}
		}
	}
}
