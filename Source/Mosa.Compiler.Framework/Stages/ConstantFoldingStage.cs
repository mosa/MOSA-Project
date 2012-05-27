/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Compiler.Framework.Stages
{
	public sealed class ConstantFoldingStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			foreach (var block in basicBlocks)
			{
				for (var context = new Context(instructionSet, block); !context.EndOfInstruction; context.GotoNext())
				{
					if (!IsFoldableInstruction(context))
						continue;
					if (!HasFoldableArguments(context))
						continue;
					FoldInstruction(context);
				}
			}
		}

		/// <summary>
		/// Folds the instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void FoldInstruction(Context context)
		{
			if (context.Instruction is AddSigned)
				FoldAddSInstruction(context);
			else if (context.Instruction is MulSigned)
				FoldMulSInstruction(context);
		}

		/// <summary>
		/// Folds the addition instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void FoldAddSInstruction(Context context)
		{
			var cA = LoadSignedInteger(context.Operand1);
			var cB = LoadSignedInteger(context.Operand2);

			context.SetInstruction(IRInstruction.Move, context.Result, new ConstantOperand(context.Result.Type, cA + cB));
		}

		/// <summary>
		/// Folds the multiply instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void FoldMulSInstruction(Context context)
		{
			var cA = LoadSignedInteger(context.Operand1);
			var cB = LoadSignedInteger(context.Operand2);

			context.SetInstruction(IRInstruction.Move, context.Result, new ConstantOperand(context.Result.Type, cA * cB));
		}

		/// <summary>
		/// Determines whether [has foldable arguments] [the specified context].
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		///   <c>true</c> if [has foldable arguments] [the specified context]; otherwise, <c>false</c>.
		/// </returns>
		private bool HasFoldableArguments(Context context)
		{
			return context.Operand1 is ConstantOperand && context.Operand2 is ConstantOperand;
		}

		/// <summary>
		/// Determines whether [is foldable instruction] [the specified context].
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		///   <c>true</c> if [is foldable instruction] [the specified context]; otherwise, <c>false</c>.
		/// </returns>
		private bool IsFoldableInstruction(Context context)
		{
			var instruction = context.Instruction;
			return instruction is AddSigned ||
				instruction is AddUnsigned ||
				instruction is MulSigned ||
				instruction is MulUnsigned;
		}

		/// <summary>
		/// Loads the signed integer.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		private int LoadSignedInteger(Operand operand)
		{
			var cop = operand as ConstantOperand;
			if (cop.Value is int)
				return (int)(operand as ConstantOperand).Value;
			if (cop.Value is short)
				return (int)(short)(operand as ConstantOperand).Value;
			if (cop.Value is sbyte)
				return (int)(sbyte)(operand as ConstantOperand).Value;
			return 0;
		}

	}
}