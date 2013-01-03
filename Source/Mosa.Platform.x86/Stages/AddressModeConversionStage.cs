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
				for (Context ctx = CreateContext(block); !ctx.IsLastInstruction; ctx.GotoNext())
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
			if (!(ctx.Instruction is BaseIRInstruction))
				return;

			if (ctx.Instruction is IntegerCompare
				|| ctx.Instruction is FloatCompare
				|| ctx.Instruction is Load
				|| ctx.Instruction is LoadZeroExtended
				|| ctx.Instruction is LoadSignExtended
				|| ctx.Instruction is Store
				|| ctx.Instruction is Call
				|| ctx.Instruction is ZeroExtendedMove
				|| ctx.Instruction is SignExtendedMove
				|| ctx.Instruction is DivSigned
				|| ctx.Instruction is DivFloat
				|| ctx.Instruction is DivUnsigned
				|| ctx.Instruction is RemSigned
				|| ctx.Instruction is RemFloat
				|| ctx.Instruction is RemUnsigned
				)
				return;

			Operand result = ctx.Result;
			Operand op1 = ctx.Operand1;
			Operand op2 = ctx.Operand2;

			// Create registers for different data types			
			Operand v1 = AllocateVirtualRegister(op1.Type);
			Operand v2 = AllocateVirtualRegister(result.Type);

			ctx.Result = v2;
			ctx.Operand1 = op2;
			ctx.Operand2 = null;
			ctx.OperandCount = 1;

			if (op1.StackType != StackTypeCode.F)
			{
				if (IsSigned(op1) && !(op1.IsConstant))
					ctx.InsertBefore().SetInstruction(IRInstruction.SignExtendedMove, v1, op1);
				else if (IsUnsigned(op1) && !(op1.IsConstant))
					ctx.InsertBefore().SetInstruction(IRInstruction.ZeroExtendedMove, v1, op1);
				else
					ctx.InsertBefore().SetInstruction(X86.Mov, v1, op1);
			}
			else
			{
				if (op1.Type.Type == CilElementType.R4)
				{
					if (op1.IsConstant)
					{
						Context before = ctx.InsertBefore();
						before.SetInstruction(X86.Mov, v1, op1);
						before.AppendInstruction(X86.Cvtss2sd, v1, v1);
					}
					else
					{
						ctx.InsertBefore().SetInstruction(X86.Cvtss2sd, v1, op1);
					}
				}
				else
				{
					ctx.InsertBefore().SetInstruction(X86.Mov, v1, op1);
				}
			}

			ctx.AppendInstruction(X86.Mov, result, v1);
		}

	}
}
