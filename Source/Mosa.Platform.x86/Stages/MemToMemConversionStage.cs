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
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class MemToMemConversionStage : BaseTransformationStage, IMethodCompilerStage, IPlatformStage
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
						if (!ctx.Ignore && ctx.Instruction is Instructions.X86Instruction)
						{
							if (ctx.Operand1 is MemoryOperand)
							{
								if (ctx.Result is MemoryOperand)
								{
									HandleMemoryToMemoryOperation(ctx);
								}
								if (ctx.Result == null && ctx.Operand2 is MemoryOperand)
								{
									HandleMemoryToMemoryOperation2(ctx);
								}
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

			SigType destinationSigType = destination.Type;

			if (RequiresSseOperation(destinationSigType))
			{
				IInstruction moveInstruction = GetMoveInstruction(destinationSigType);
				RegisterOperand destinationRegister = AllocateRegister(destinationSigType);

				ctx.Result = destinationRegister;
				ctx.AppendInstruction(moveInstruction, destination, destinationRegister);
			}
			else
			{
				SigType sourceSigType = source.Type;
				IInstruction moveInstruction = GetMoveInstruction(sourceSigType);
				RegisterOperand sourceRegister = AllocateRegister(sourceSigType);

				ctx.Operand1 = sourceRegister;

				ctx.InsertBefore().SetInstruction(moveInstruction, sourceRegister, source);
			}
		}

		private void HandleMemoryToMemoryOperation2(Context ctx)
		{
			Operand destination = ctx.Operand1;
			Operand source = ctx.Operand2;

			Debug.Assert(destination is MemoryOperand && source is MemoryOperand);

			SigType destinationSigType = destination.Type;

			if (RequiresSseOperation(destinationSigType))
			{
				IInstruction moveInstruction = GetMoveInstruction(destinationSigType);
				RegisterOperand destinationRegister = AllocateRegister(destinationSigType);

				ctx.Operand1 = destinationRegister;
				ctx.AppendInstruction(moveInstruction, destination, destinationRegister);
			}
			else
			{
				SigType sourceSigType = source.Type;
				IInstruction moveInstruction = GetMoveInstruction(sourceSigType);
				RegisterOperand sourceRegister = AllocateRegister(sourceSigType);

				ctx.Operand2 = sourceRegister;
				ctx.InsertBefore().SetInstruction(moveInstruction, sourceRegister, source);
			}
		}

		private IInstruction GetMoveInstruction(SigType sigType)
		{
			IInstruction moveInstruction;
			if (RequiresSseOperation(sigType) == false)
			{
				if (MustSignExtendOnLoad(sigType.Type) == true)
				{
					moveInstruction = X86.Movsx;
				}
				else if (MustZeroExtendOnLoad(sigType.Type) == true)
				{
					moveInstruction = X86.Movzx;
				}
				else
				{
					moveInstruction = X86.Mov;
				}
			}
			else if (sigType.Type == CilElementType.R8)
			{
				moveInstruction = X86.Movsd;
			}
			else
			{
				moveInstruction = X86.Movss;
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
