/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class InterruptVectorStage : BaseCompilerStage, ICompilerStage, IPipelineStage
	{
		#region ICompilerStage Members

		void ICompilerStage.Setup(BaseCompiler compiler)
		{
			base.Setup(compiler);
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void ICompilerStage.Run()
		{
			CreateInterruptVectors();
		}

		#endregion ICompilerStage Members

		#region Internal

		/// <summary>
		/// Creates the interrupt service routine (ISR) methods.
		/// </summary>
		private void CreateInterruptVectors()
		{
			var type = typeSystem.GetTypeByName("Mosa.Kernel.x86", "IDT");

			if (type == null)
				return;

			var method = TypeSystem.GetMethodByName(type, "ProcessInterrupt");

			if (method == null)
				return;

			Operand interrupt = Operand.CreateSymbolFromMethod(typeSystem, method);

			Operand esp = Operand.CreateCPURegister(typeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);

			for (int i = 0; i <= 255; i++)
			{
				BasicBlocks basicBlocks = new BasicBlocks();
				InstructionSet instructionSet = new InstructionSet(25);
				Context ctx = instructionSet.CreateNewBlock(basicBlocks);
				basicBlocks.AddHeaderBlock(ctx.BasicBlock);

				ctx.AppendInstruction(X86.Cli);
				if (i <= 7 || i >= 16 | i == 9) // For IRQ 8, 10, 11, 12, 13, 14 the cpu will automatically pushed the error code
					ctx.AppendInstruction(X86.Push, null, Operand.CreateConstantUnsignedInt(typeSystem, 0));
				ctx.AppendInstruction(X86.Push, null, Operand.CreateConstantUnsignedInt(typeSystem, (uint)i));
				ctx.AppendInstruction(X86.Pushad);
				ctx.AppendInstruction(X86.Call, null, interrupt);
				ctx.AppendInstruction(X86.Popad);
				ctx.AppendInstruction(X86.Add, esp, esp, Operand.CreateConstantUnsignedInt(typeSystem, 8));
				ctx.AppendInstruction(X86.Sti);
				ctx.AppendInstruction(X86.IRetd);

				var interruptMethod = compiler.CreateLinkerMethod("InterruptISR" + i.ToString());
				compiler.CompileMethod(interruptMethod, basicBlocks, instructionSet);
			}
		}

		#endregion Internal
	}
}