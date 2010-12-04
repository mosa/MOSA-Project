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

namespace Mosa.Platform.x86
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
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"X86.ExceptionHeaderPreprocessingStage"; } }

		/// <summary>
		/// Runs the specified method compiler.
		/// </summary>
		public void Run()
		{
			ProcessExceptionClauseLabels();
			// Iterate all blocks and preprocess them
			foreach (BasicBlock block in basicBlocks)
				ProcessBlock(block);
		}

		/// <summary>
		/// 
		/// </summary>
		private void ProcessExceptionClauseLabels()
		{
			this.labelMapping.Clear();

			foreach (EhClause clause in this.methodCompiler.Method.ExceptionClauseHeader.Clauses)
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
			AddClauseLabel(clause, clause.TryOffset);
			AddClauseLabel(clause, clause.TryEnd);
			AddClauseLabel(clause, clause.HandlerOffset);
			AddClauseLabel(clause, clause.HandlerEnd);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="clause"></param>
		/// <param name="label"></param>
		private void AddClauseLabel(EhClause clause, int label)
		{
			this.labelMapping[label] = clause;
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
					EhClause clause = this.labelMapping[context.Label];
					if (clause.TryOffset == context.Label || clause.HandlerOffset == context.Label)
					{

						this.labelMapping.Remove(context.Label);
					}
				}
			}
		}
	}
}
