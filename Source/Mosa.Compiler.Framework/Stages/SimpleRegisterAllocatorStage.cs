/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */

using System.Collections;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class SimpleRegisterAllocatorStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{

		#region Data members

		private CPURegister[] cpuRegister = new CPURegister[64];

		private BitArray traversed;
		private BitArray analyzed;

		#endregion // Data members

		private class CPURegister
		{
			public bool Used = false;					// is used
			public bool Locked = false;					// is locked (unable to be spilled)

			public int VirtualRegisterIndex = -1;		// virtual register index (-1=none)
			public bool VirtualRegisterDirty = false;	// is virtual register dirty? yes/no
		}

		/// <summary>
		/// Runs the specified method compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			if (methodCompiler.Compiler.PlugSystem.GetPlugMethod(methodCompiler.Method) != null)
				return;

			traversed = new BitArray(basicBlocks.Count);
			analyzed = new BitArray(basicBlocks.Count);

			RegisterBitmap start = new RegisterBitmap();

			// Main Code
			start.Set(architecture.StackFrameRegister);
			start.Set(architecture.StackPointerRegister);
			start.Set(architecture.CallingConvention.CalleeSavedRegisters);

			ProcesBlockChain(basicBlocks.PrologueBlock, start);

			// Handler Code
			foreach (ExceptionHandlingClause clause in methodCompiler.ExceptionClauseHeader.Clauses)
			{
				RegisterBitmap exceptionStart = new RegisterBitmap();
				start.Set(architecture.StackFrameRegister);
				start.Set(architecture.StackPointerRegister);

				// TODO: Set the exception register

				ProcesBlockChain(basicBlocks.GetByLabel(clause.HandlerOffset), exceptionStart);
			}
		}

		private void ProcesBlockChain(BasicBlock block, RegisterBitmap start)
		{
			if (analyzed.Get(block.Sequence))
				return;

			RegisterBitmap end = ProcesBlock(block, start);

			foreach (BasicBlock nextBlock in block.NextBlocks)
			{
				ProcesBlockChain(nextBlock, end);
			}
		}

		private RegisterBitmap ProcesBlock(BasicBlock block, RegisterBitmap start)
		{
			analyzed.Set(block.Sequence, true);

			RegisterBitmap current = start;

			for (Context ctx = CreateContext(block); !ctx.EndOfInstruction; ctx.GotoNext())
			{
				;

				// at the end of the loop

				RegisterBitmap inputRegisters = new RegisterBitmap();
				RegisterBitmap outputRegisters = new RegisterBitmap();

				GetRegisterUsage(ctx, ref inputRegisters, ref outputRegisters);


			}

			return current;
		}



		// Copied from RegisterUsageAnaluzerStage.cs
		private void GetRegisterUsage(Context context, ref RegisterBitmap inputRegisters, ref RegisterBitmap outputRegisters)
		{
			BasePlatformInstruction instruction = context.Instruction as BasePlatformInstruction;

			if (instruction == null)
				return;

			IRegisterUsage usage = instruction as IRegisterUsage;

			if (usage == null)
				return;

			outputRegisters = usage.GetOutputRegisters(context);
			inputRegisters = usage.GetInputRegisters(context);
		}
	}
}
