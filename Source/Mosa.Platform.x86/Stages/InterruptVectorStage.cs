/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

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

		#endregion // ICompilerStage Members

		#region Internal

		/// <summary>
		/// Creates the interrupt service routine (ISR) methods.
		/// </summary>
		private void CreateInterruptVectors()
		{
			RuntimeType runtimeType = typeSystem.GetType(@"Mosa.Kernel.x86.IDT");

			if (runtimeType == null)
				return;

			RuntimeMethod runtimeMethod = runtimeType.FindMethod(@"ProcessInterrupt");

			if (runtimeMethod == null)
				return;

			Operand interruptMethod = Operand.CreateSymbolFromMethod(runtimeMethod);

			Operand esp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ESP);

			for (int i = 0; i <= 255; i++)
			{
				InstructionSet instructionSet = new InstructionSet(100);
				Context ctx = new Context(instructionSet);

				ctx.AppendInstruction(X86.Cli);
				if (i <= 7 || i >= 16 | i == 9) // For IRQ 8, 10, 11, 12, 13, 14 the cpu will automatically pushed the error code
					ctx.AppendInstruction(X86.Push, null, Operand.CreateConstant(BuiltInSigType.SByte, 0x0));
				ctx.AppendInstruction(X86.Push, null, Operand.CreateConstant(BuiltInSigType.SByte, (byte)i));
				ctx.AppendInstruction(X86.Pushad);
				ctx.AppendInstruction(X86.Call, null, interruptMethod);
				ctx.AppendInstruction(X86.Popad);
				ctx.AppendInstruction(X86.Add, esp, Operand.CreateConstant(BuiltInSigType.Int32, 0x08));
				ctx.AppendInstruction(X86.Sti);
				ctx.AppendInstruction(X86.IRetd);

				LinkTimeCodeGenerator.Compile(this.compiler, @"InterruptISR" + i.ToString(), instructionSet, typeSystem);
			}
		}

		#endregion Internal
	}
}
