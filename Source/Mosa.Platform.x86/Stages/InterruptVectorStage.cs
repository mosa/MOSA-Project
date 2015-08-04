// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class InterruptVectorStage : BaseCompilerStage
	{
		protected override void Run()
		{
			CreateInterruptVectors();
		}

		#region Internal

		/// <summary>
		/// Creates the interrupt service routine (ISR) methods.
		/// </summary>
		private void CreateInterruptVectors()
		{
			var type = TypeSystem.GetTypeByName("Mosa.Kernel.x86", "IDT");

			if (type == null)
				return;

			var method = type.FindMethodByName("ProcessInterrupt");

			if (method == null)
				return;

			Operand interrupt = Operand.CreateSymbolFromMethod(TypeSystem, method);

			Operand esp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);

			for (int i = 0; i <= 255; i++)
			{
				var basicBlocks = new BasicBlocks();
				var block = basicBlocks.CreateBlock();
				basicBlocks.AddHeaderBlock(block);
				var ctx = new Context(block);

				ctx.AppendInstruction(X86.Cli);
				if (i <= 7 || i >= 16 | i == 9) // For IRQ 8, 10, 11, 12, 13, 14 the cpu will automatically pushed the error code
					ctx.AppendInstruction(X86.Push, null, Operand.CreateConstant(TypeSystem, 0));
				ctx.AppendInstruction(X86.Push, null, Operand.CreateConstant(TypeSystem, i));
				ctx.AppendInstruction(X86.Pushad);
				ctx.AppendInstruction(X86.Push, null, esp);
				ctx.AppendInstruction(X86.Call, null, interrupt);
				ctx.AppendInstruction(X86.Pop, esp);
				ctx.AppendInstruction(X86.Popad);
				ctx.AppendInstruction(X86.Add, esp, esp, Operand.CreateConstant(TypeSystem, 8));
				ctx.AppendInstruction(X86.Sti);
				ctx.AppendInstruction(X86.IRetd);

				var interruptMethod = Compiler.CreateLinkerMethod("InterruptISR" + i.ToString());
				Compiler.CompileMethod(interruptMethod, basicBlocks, 0);
			}
		}

		#endregion Internal
	}
}
