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
				for (Context ctx = CreateContext(block); !ctx.EndOfInstruction; ctx.GotoNext())
					if (!ctx.IsEmpty)
						if (ctx.OperandCount == 2 && ctx.ResultCount == 1)
							ThreeTwoAddressConversion(ctx);
		}

		#endregion // IMethodCompilerStage Members

		/// <summary>
		/// Converts the given instruction from three address format to a two address format.
		/// </summary>
		/// <param name="ctx">The conversion context.</param>
		private static void ThreeTwoAddressConversion(Context ctx)
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
				|| ctx.Instruction is SignExtendedMove)
				return;

			Operand result = ctx.Result;
			Operand op1 = ctx.Operand1;
			Operand op2 = ctx.Operand2;

			// Create registers for different data types
			Operand eax = Operand.CreateCPURegister(op1.Type, op1.StackType == StackTypeCode.F ? (Register)SSE2Register.XMM0 : GeneralPurposeRegister.EAX);
			Operand storeOperand = Operand.CreateCPURegister(result.Type, result.StackType == StackTypeCode.F ? (Register)SSE2Register.XMM0 : GeneralPurposeRegister.EAX);

			ctx.Result = storeOperand;
			ctx.Operand1 = op2;
			ctx.Operand2 = null;
			ctx.OperandCount = 1;

			if (op1.StackType != StackTypeCode.F)
			{
				if (IsSigned(op1) && !(op1.IsConstant))
					ctx.InsertBefore().SetInstruction(IRInstruction.SignExtendedMove, eax, op1);
				else if (IsUnsigned(op1) && !(op1.IsConstant))
					ctx.InsertBefore().SetInstruction(IRInstruction.ZeroExtendedMove, eax, op1);
				else
					ctx.InsertBefore().SetInstruction(X86.Mov, eax, op1);
			}
			else
			{
				if (op1.Type.Type == CilElementType.R4)
				{
					if (op1.IsConstant)
					{
						Context before = ctx.InsertBefore();
						before.SetInstruction(X86.Mov, eax, op1);
						before.AppendInstruction(X86.Cvtss2sd, eax, eax);
					}
					else
					{
						ctx.InsertBefore().SetInstruction(X86.Cvtss2sd, eax, op1);
					}
				}
				else
				{
					ctx.InsertBefore().SetInstruction(X86.Mov, eax, op1);
				}
			}

			ctx.AppendInstruction(X86.Mov, result, eax);
		}

	}
}
