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

using System;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

using IR = Mosa.Runtime.CompilerFramework.IR;
using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class AddressModeConversionStage : BaseTransformationStage, IMethodCompilerStage, IPlatformTransformationStage, IPipelineStage
	{

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"X86.AddressModeConversionStage"; } }

		private static PipelineStageOrder[] _pipelineOrder = new PipelineStageOrder[] {
				new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(LongOperandTransformationStage)),
				new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(CILTransformationStage))
			};

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder { get { return _pipelineOrder; } }

		#endregion // IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public override void Run()
		{
			foreach (BasicBlock block in BasicBlocks)
				for (Context ctx = CreateContext( block); !ctx.EndOfInstruction; ctx.GotoNext())
					if (ctx.Instruction != null)
						if (!ctx.Ignore && ctx.OperandCount == 2 && ctx.ResultCount == 1)
							if (ctx.Instruction is CIL.ArithmeticInstruction || ctx.Instruction is IR.ThreeOperandInstruction)
								ThreeTwoAddressConversion(ctx);
		}

		/// <summary>
		/// Converts the given instruction from three address format to a two address format.
		/// </summary>
		/// <param name="ctx">The conversion context.</param>
		private static void ThreeTwoAddressConversion(Context ctx)
		{
			Operand result = ctx.Result;
			Operand op1 = ctx.Operand1;
			Operand op2 = ctx.Operand2;

            if (ctx.Instruction is IR.FloatingPointCompareInstruction)
                return;

            if (ctx.Instruction is CIL.MulInstruction /*|| ctx.Instruction is CIL.DivInstruction*/)
                if (!(op1 is ConstantOperand) && (op2 is ConstantOperand))
                {
                    Operand temp = op1;
                    op1 = op2;
                    op2 = temp;
                }

			// Create registers for different data types
            RegisterOperand eax = new RegisterOperand(op1.Type, op1.StackType == StackTypeCode.F ? (Register)SSE2Register.XMM0 : GeneralPurposeRegister.EAX);
            RegisterOperand storeOperand = new RegisterOperand(result.Type, result.StackType == StackTypeCode.F ? (Register)SSE2Register.XMM0 : GeneralPurposeRegister.EAX);
			//    RegisterOperand eaxL = new RegisterOperand(op1.Type, GeneralPurposeRegister.EAX);

            ctx.Result = storeOperand;
			ctx.Operand1 = op2;
			ctx.Operand2 = null;
			ctx.OperandCount = 1;

			//    // Check if we have to sign-extend the operand that's being loaded
			//    if (IsSigned(op1) && !(op1 is ConstantOperand)) {
			//        // Sign extend it
			//        ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovsxInstruction, eaxL, op1);
			//    }
			//    // Check if the operand has to be zero-extended
			//    else if (IsUnsigned(op1) && !(op1 is ConstantOperand) && op1.StackType != StackTypeCode.F) {
			//        ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovzxInstruction, eaxL, op1);
			//    }
			//    // In any other case just load it
			//    else

            if (/*!Is32Bit(op1) &&*/ !(op1.StackType == StackTypeCode.F))
            {
                if (IsSigned(op1))
                    ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovsxInstruction, eax, op1);
                else if (IsUnsigned(op1))
                    ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovzxInstruction, eax, op1);
                else
                    ctx.InsertBefore().SetInstruction(IR.Instruction.MoveInstruction, eax, op1);
            }
            else
            {
                if (op1.Type.Type == CilElementType.R4)
                {
                    if (op1 is ConstantOperand)
                    {
                        Context before = ctx.InsertBefore();
                        before.SetInstruction(IR.Instruction.MoveInstruction, eax, op1);
                        before.AppendInstruction(CPUx86.Instruction.Cvtss2sdInstruction, eax, eax);
                    }
                    else
                        ctx.InsertBefore().SetInstruction(CPUx86.Instruction.Cvtss2sdInstruction, eax, op1);
                }
                else
                    ctx.InsertBefore().SetInstruction(IR.Instruction.MoveInstruction, eax, op1);
            }
            ctx.AppendInstruction(IR.Instruction.MoveInstruction, result, eax);
		}

	}
}
