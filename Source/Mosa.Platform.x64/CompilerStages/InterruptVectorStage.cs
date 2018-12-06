// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x64.CompilerStages
{
	/// <summary>
	/// Interrupt Vector Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public sealed class InterruptVectorStage : BaseCompilerStage
	{
		protected override void RunPreCompile()
		{
			CreateInterruptVectors();
		}

		/// <summary>
		/// Creates the interrupt service routine (ISR) methods.
		/// </summary>
		private void CreateInterruptVectors()
		{
			var type = TypeSystem.GetTypeByName("Mosa.Kernel.x64", "IDT");

			if (type == null)
				return;

			var method = type.FindMethodByName("ProcessInterrupt");

			if (method == null)
				return;

			var interrupt = Operand.CreateSymbolFromMethod(method, TypeSystem);

			var esp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);

			for (int i = 0; i <= 255; i++)
			{
				var basicBlocks = new BasicBlocks();
				var block = basicBlocks.CreateBlock(BasicBlock.PrologueLabel);
				basicBlocks.AddHeadBlock(block);
				var ctx = new Context(block);

				ctx.AppendInstruction(X64.Cli);
				if (i <= 7 || (i >= 16 | i == 9)) // For IRQ 8, 10, 11, 12, 13, 14 the cpu will automatically pushed the error code
				{
					ctx.AppendInstruction(X64.Push64, null, CreateConstant(0));
				}
				ctx.AppendInstruction(X64.Push64, null, CreateConstant(i));
				ctx.AppendInstruction(X64.Pushad);
				ctx.AppendInstruction(X64.Push64, null, esp);
				ctx.AppendInstruction(X64.Call, null, interrupt);
				ctx.AppendInstruction(X64.Pop64, esp);
				ctx.AppendInstruction(X64.Popad);
				ctx.AppendInstruction(X64.Add64, esp, esp, CreateConstant(8));
				ctx.AppendInstruction(X64.Sti);
				ctx.AppendInstruction(X64.IRetd);

				var interruptMethod = Compiler.CreateLinkerMethod("InterruptISR" + i.ToString());
				Compiler.CompileMethod(interruptMethod, basicBlocks);
			}
		}
	}
}
