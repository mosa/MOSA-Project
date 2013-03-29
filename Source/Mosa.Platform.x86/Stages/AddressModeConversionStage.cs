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
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Metadata;
using Mosa.Platform.x86.Instructions;

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
		public override void Run()
		{
			foreach (BasicBlock block in basicBlocks)
				for (Context ctx = CreateContext(block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
					if (!ctx.IsEmpty)
						if (ctx.OperandCount == 2 && ctx.ResultCount == 1)
							ThreeTwoAddressConversion(ctx);
		}

		#endregion // IMethodCompilerStage Members

		/// <summary>
		/// Converts the given instruction from three address format to a two address format.
		/// </summary>
		/// <param name="ctx">The conversion context.</param>
		private void ThreeTwoAddressConversion(Context ctx)
		{
			if (!(ctx.Instruction is X86Instruction))
				return;

			if (!(ctx.OperandCount >= 1 && ctx.ResultCount >= 1 && ctx.Result != ctx.Operand1))
				return;

			Operand result = ctx.Result;
			Operand op1 = ctx.Operand1;

			ctx.InsertBefore().SetInstruction(X86.Mov, result, op1);
			ctx.Operand1 = result;

			return;
		}

	}
}
