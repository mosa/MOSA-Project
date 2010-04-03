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
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class MemToMemConversionStage : BaseTransformationStage, IMethodCompilerStage, IPlatformStage, IPipelineStage
	{

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"X86.MemToMemConversionStage"; } }

		#endregion // IPipelineStage Members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public override void Run()
		{
            foreach (BasicBlock block in BasicBlocks)
            {
                for (Context ctx = CreateContext(block); !ctx.EndOfInstruction; ctx.GotoNext())
                {
                    if (ctx.Instruction != null)
                    {
                        if (!ctx.Ignore && ctx.Instruction is CPUx86.IX86Instruction)
                        {
                            if (ctx.Result is MemoryOperand && ctx.Operand1 is MemoryOperand)
                            {
                                this.HandleMemoryToMemoryOperation(ctx);
                            }
                        }
                    }
                }
            }
		}

		#endregion // IMethodCompilerStage Members

		private void HandleMemoryToMemoryOperation(Context ctx)
		{
			Operand destination = ctx.Result;
			Operand source = ctx.Operand1;

			Debug.Assert(destination is MemoryOperand && source is MemoryOperand);

            RegisterOperand temporaryRegister = this.AllocateRegister(destination.Type);
            ctx.Operand1 = temporaryRegister;

            IInstruction moveInstruction = this.GetMoveInstruction(destination.Type);

            Context before = ctx.InsertBefore();
            before.SetInstruction(moveInstruction, temporaryRegister, source);
		}

        private IInstruction GetMoveInstruction(SigType sigType)
        {
            IInstruction moveInstruction;
            if (this.RequiresSseOperation(sigType) == false)
            {
                moveInstruction = CPUx86.Instruction.MovInstruction;
            }
            else if (sigType.Type == CilElementType.R8)
            {
                moveInstruction = CPUx86.Instruction.MovsdInstruction;
            }
            else
            {
                moveInstruction = CPUx86.Instruction.MovssInstruction;
            }

            return moveInstruction;
        }

        private RegisterOperand AllocateRegister(SigType sigType)
        {
            if (RequiresSseOperation(sigType))
            {
                return new RegisterOperand(sigType, SSE2Register.XMM0);
            }
            else
            {
                return new RegisterOperand(sigType, GeneralPurposeRegister.EDX);
            }
        }

        private bool RequiresSseOperation(SigType sigType)
        {
            return sigType.Type == CilElementType.R4 
                || sigType.Type == CilElementType.R8;
        }
	}
}
