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

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using IR = Mosa.Runtime.CompilerFramework.IR;
using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class AddressModeConversionStage : BaseTransformationStage, IPipelineStage
	{

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"x86.AddressModeConversionStage"; } }

		#endregion // IPipelineStage Members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public override void Run()
		{
			foreach (BasicBlock block in basicBlocks)
				for (Context ctx = CreateContext(block); !ctx.EndOfInstruction; ctx.GotoNext())
					if (ctx.Instruction != null)
						if (!ctx.Ignore && ctx.OperandCount == 2 && ctx.ResultCount == 1)
							if (ctx.Instruction is CIL.ArithmeticInstruction || ctx.Instruction is IR.ThreeOperandInstruction)
								ThreeTwoAddressConversion(ctx);
		}

		#endregion // IMethodCompilerStage Members

		/// <summary>
		/// Converts the given instruction from three address format to a two address format.
		/// </summary>
		/// <param name="ctx">The conversion context.</param>
		private static void ThreeTwoAddressConversion(Context ctx)
		{
			Operand result = ctx.Result;
			Operand op1 = ctx.Operand1;
			Operand op2 = ctx.Operand2;

			if (ctx.Instruction is IR.IntegerCompareInstruction
				|| ctx.Instruction is IR.FloatingPointCompareInstruction
				|| ctx.Instruction is IR.LoadInstruction
				|| ctx.Instruction is IR.StoreInstruction)
			{
				return;
			}

			if (ctx.Instruction is CIL.MulInstruction)
			{
				if (!(op1 is ConstantOperand) && (op2 is ConstantOperand))
				{
					Operand temp = op1;
					op1 = op2;
					op2 = temp;
				}
			}

			// Create registers for different data types
			RegisterOperand eax = new RegisterOperand(op1.Type, op1.StackType == StackTypeCode.F ? (Register)SSE2Register.XMM0 : GeneralPurposeRegister.EAX);
			RegisterOperand storeOperand = new RegisterOperand(result.Type, result.StackType == StackTypeCode.F ? (Register)SSE2Register.XMM0 : GeneralPurposeRegister.EAX);
			//    RegisterOperand eaxL = new RegisterOperand(op1.Type, GeneralPurposeRegister.EAX);

			ctx.Result = storeOperand;
			ctx.Operand1 = op2;
			ctx.Operand2 = null;
			ctx.OperandCount = 1;

			if (op1.StackType != StackTypeCode.F)
			{
				if (IsSigned(op1) && !(op1 is ConstantOperand))
					ctx.InsertBefore().SetInstruction(IR.Instruction.SignExtendedMoveInstruction, eax, op1);
				else if (IsUnsigned(op1) && !(op1 is ConstantOperand))
					ctx.InsertBefore().SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, eax, op1);
				else
					ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovInstruction, eax, op1);
			}
			else
			{
				if (op1.Type.Type == CilElementType.R4)
				{
					if (op1 is ConstantOperand)
					{
						Context before = ctx.InsertBefore();
						before.SetInstruction(CPUx86.Instruction.MovInstruction, eax, op1);
						before.AppendInstruction(CPUx86.Instruction.Cvtss2sdInstruction, eax, eax);
					}
					else
					{
						ctx.InsertBefore().SetInstruction(CPUx86.Instruction.Cvtss2sdInstruction, eax, op1);
					}
				}
				else
				{
					ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovInstruction, eax, op1);
				}
			}

			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, result, eax);
		}

	}
}
