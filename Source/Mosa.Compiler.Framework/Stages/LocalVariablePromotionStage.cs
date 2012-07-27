/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Promotes a local variable to virtual register (or temp stack operand).
	/// </summary>
	public class LocalVariablePromotionStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{

		private List<Operand> localVariablesInExceptions = new List<Operand>();

		#region ICompilerStage members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			CollectVariablesInExceptions();

			List<Operand> stack = new List<Operand>(methodCompiler.StackLayout.Stack);

			foreach (var local in stack)
			{
				if (local.IsLocalVariable && local.Type.Type != CilElementType.ValueType)
					if (!localVariablesInExceptions.Contains(local))
						local.Replace(methodCompiler.StackLayout.AllocateStackOperand(local.Type, false), instructionSet);
			}

			localVariablesInExceptions = null;
		}

		/// <summary>
		/// Collects the variables in exceptions.
		/// </summary>
		private void CollectVariablesInExceptions()
		{
			if (basicBlocks.HeadBlocks.Count == 1)
				return;

			foreach (var head in basicBlocks.HeadBlocks)
			{
				if (head == basicBlocks.PrologueBlock)
					continue;

				foreach (BasicBlock block in basicBlocks.GetConnectedBlocksStartingAtHead(head))
				{
					for (Context ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
					{
						if (ctx.IsEmpty)
							continue;

						foreach (var operand in ctx.Operands)
							if (operand.IsLocalVariable)
								localVariablesInExceptions.AddIfNew(operand);

						if (ctx.Result != null)
							if (ctx.Result.IsLocalVariable)
								localVariablesInExceptions.AddIfNew(ctx.Result);
					}

					// quick out
					if (localVariablesInExceptions.Count == methodCompiler.StackLayout.LocalVariableCount)
						return;
				}
			}
		}
		#endregion ICompilerStage members

	}
}
