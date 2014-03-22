/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Scott Balmos <sbalmos@fastmail.fm>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class AddressModeConversionStage : BaseTransformationStage, IPipelineStage
	{
		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public override void Execute()
		{
			foreach (BasicBlock block in BasicBlocks)
				for (Context ctx = CreateContext(block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
					if (!ctx.IsEmpty)
						if (ctx.OperandCount == 2 && ctx.ResultCount == 1)
							ThreeTwoAddressConversion(ctx);
		}

		#endregion IMethodCompilerStage Members

		/// <summary>
		/// Converts the given instruction from three address format to a two address format.
		/// </summary>
		/// <param name="context">The conversion context.</param>
		private void ThreeTwoAddressConversion(Context context)
		{
			if (!(context.Instruction is X86Instruction))
				return;

			if (!(context.OperandCount >= 1 && context.ResultCount >= 1 && context.Result != context.Operand1))
				return;

			Operand result = context.Result;
			Operand operand1 = context.Operand1;

			context.Operand1 = result;

			context.InsertBefore().SetInstruction(GetMove(result, operand1), result, operand1);

			return;
		}
	}
}