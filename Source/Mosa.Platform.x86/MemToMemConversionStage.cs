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

using System.Diagnostics;

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class MemToMemConversionStage : BaseTransformationStage, IMethodCompilerStage, IPlatformStage, IPipelineStage
	{

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			foreach (BasicBlock block in basicBlocks)
			{
				for (Context ctx = CreateContext(block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					if (ctx.Instruction != null)
					{
						if (!ctx.Ignore && ctx.Instruction is Instructions.IX86Instruction)
						{
							if (IsMemoryOperand(ctx.Result) && IsMemoryOperand(ctx.Operand1))
							{
								this.HandleMemoryToMemoryOperation(ctx);
							}
						}
					}
				}
			}
		}

		#endregion // IMethodCompilerStage Members

		private bool IsMemoryOperand(Operand op)
		{
			return op is MemoryOperand;
		}

		private void HandleMemoryToMemoryOperation(Context ctx)
		{
			Operand destination = ctx.Result;
			Operand source = ctx.Operand1;

			Debug.Assert(destination is MemoryOperand && source is MemoryOperand);

			SigType destinationSigType = destination.Type;

			if (this.RequiresSseOperation(destinationSigType))
			{
				IInstruction moveInstruction = this.GetMoveInstruction(destinationSigType);
				RegisterOperand destinationRegister = this.AllocateRegister(destinationSigType);

				ctx.Result = destinationRegister;
				ctx.AppendInstruction(moveInstruction, destination, destinationRegister);
			}
			else
			{
				SigType sourceSigType = ctx.Operand1.Type;
				IInstruction moveInstruction = this.GetMoveInstruction(sourceSigType);
				RegisterOperand sourceRegister = this.AllocateRegister(sourceSigType);

				ctx.Operand1 = sourceRegister;

				ctx.InsertBefore().SetInstruction(moveInstruction, sourceRegister, source);
			}
		}

		private IInstruction GetMoveInstruction(SigType sigType)
		{
			IInstruction moveInstruction;
			if (this.RequiresSseOperation(sigType) == false)
			{
				if (MustSignExtendOnLoad(sigType.Type) == true)
				{
					moveInstruction = Instruction.MovsxInstruction;
				}
				else if (MustZeroExtendOnLoad(sigType.Type) == true)
				{
					moveInstruction = Instruction.MovzxInstruction;
				}
				else
				{
					moveInstruction = Instruction.MovInstruction;
				}
			}
			else if (sigType.Type == CilElementType.R8)
			{
				moveInstruction = Instruction.MovsdInstruction;
			}
			else
			{
				moveInstruction = Instruction.MovssInstruction;
			}

			return moveInstruction;
		}

		private RegisterOperand AllocateRegister(SigType sigType)
		{
			RegisterOperand result;
			if (RequiresSseOperation(sigType))
			{
				result = new RegisterOperand(sigType, SSE2Register.XMM6);
			}
			else
			{
				result = new RegisterOperand(sigType, GeneralPurposeRegister.EAX);
			}

			return result;
		}

		private bool RequiresSseOperation(SigType sigType)
		{
			return sigType.Type == CilElementType.R4
				|| sigType.Type == CilElementType.R8;
		}

		private static bool MustSignExtendOnLoad(CilElementType elementType)
		{
			return (elementType == CilElementType.I1 || elementType == CilElementType.I2);
		}

		private static bool MustZeroExtendOnLoad(CilElementType elementType)
		{
			return (elementType == CilElementType.U1 || elementType == CilElementType.U2 || elementType == CilElementType.Char);
		}
	}
}
