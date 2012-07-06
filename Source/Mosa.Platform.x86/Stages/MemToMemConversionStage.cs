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
					if (ctx.IsEmpty) 
						continue;

					if (!(ctx.Instruction is X86Instruction))
						continue;

					if (ctx.Operand1 == null)
						continue;

					if (!ctx.Operand1.IsMemoryAddress)
						continue;

					if (ctx.Result != null)
					{
						if (ctx.Result.IsMemoryAddress)
						{
							HandleMemoryToMemoryOperation(ctx);
						}
					}
					else if (ctx.Operand2 != null && ctx.Operand2.IsMemoryAddress)
					{
						HandleMemoryToMemoryOperation2(ctx);
					}
				}
			}
		}

		#endregion // IMethodCompilerStage Members

		private void HandleMemoryToMemoryOperation(Context ctx)
		{
			Operand destination = ctx.Result;
			Operand source = ctx.Operand1;

			Debug.Assert(destination.IsMemoryAddress && source.IsMemoryAddress);

			SigType destinationSigType = destination.Type;

			if (RequiresSseOperation(destinationSigType))
			{
				BaseInstruction moveInstruction = GetMoveInstruction(destinationSigType);
				Operand destinationRegister = AllocateRegister(destinationSigType);

				ctx.Result = destinationRegister;
				ctx.AppendInstruction(moveInstruction, destination, destinationRegister);
			}
			else
			{
				SigType sourceSigType = source.Type;
				BaseInstruction moveInstruction = GetMoveInstruction(sourceSigType);
				Operand sourceRegister = AllocateRegister(sourceSigType);

				ctx.Operand1 = sourceRegister;

				ctx.InsertBefore().SetInstruction(moveInstruction, sourceRegister, source);
			}
		}

		private void HandleMemoryToMemoryOperation2(Context ctx)
		{
			Operand destination = ctx.Operand1;
			Operand source = ctx.Operand2;

			Debug.Assert(destination.IsMemoryAddress && source.IsMemoryAddress);

			SigType destinationSigType = destination.Type;

			if (RequiresSseOperation(destinationSigType))
			{
				BaseInstruction moveInstruction = GetMoveInstruction(destinationSigType);
				Operand destinationRegister = AllocateRegister(destinationSigType);

				ctx.Operand1 = destinationRegister;
				ctx.AppendInstruction(moveInstruction, destination, destinationRegister);
			}
			else
			{
				SigType sourceSigType = source.Type;
				BaseInstruction moveInstruction = GetMoveInstruction(sourceSigType);
				Operand sourceRegister = AllocateRegister(sourceSigType);

				ctx.Operand2 = sourceRegister;
				ctx.InsertBefore().SetInstruction(moveInstruction, sourceRegister, source);
			}
		}

		private BaseInstruction GetMoveInstruction(SigType sigType)
		{
			BaseInstruction moveInstruction;
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

		private Operand AllocateRegister(SigType sigType)
		{
			if (RequiresSseOperation(sigType))
			{
				return Operand.CreateCPURegister(sigType, SSE2Register.XMM6);
			}
			else
			{
				return Operand.CreateCPURegister(sigType, GeneralPurposeRegister.EAX);
			}
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
