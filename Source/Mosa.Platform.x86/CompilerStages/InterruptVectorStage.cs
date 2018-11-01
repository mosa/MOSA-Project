// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.CompilerStages
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
			var type = TypeSystem.GetTypeByName("Mosa.Kernel.x86", "IDT");

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

				ctx.AppendInstruction(X86.Cli);
				if (i <= 7 || (i >= 16 | i == 9)) // For IRQ 8, 10, 11, 12, 13, 14 the cpu will automatically pushed the error code
				{
					ctx.AppendInstruction(X86.PushConst32, null, CreateConstant(0));
				}
				ctx.AppendInstruction(X86.PushConst32, null, CreateConstant(i));
				ctx.AppendInstruction(X86.Pushad);
				ctx.AppendInstruction(X86.Push32, null, esp);
				ctx.AppendInstruction(X86.CallStatic, null, interrupt);
				ctx.AppendInstruction(X86.Pop32, esp);
				ctx.AppendInstruction(X86.Popad);
				ctx.AppendInstruction(X86.AddConst32, esp, esp, CreateConstant(8));
				ctx.AppendInstruction(X86.Sti);
				ctx.AppendInstruction(X86.IRetd);

				var interruptMethod = Compiler.CreateLinkerMethod("InterruptISR" + i.ToString());
				Compiler.CompileMethod(interruptMethod, basicBlocks);
			}
		}
	}
}
