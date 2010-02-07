/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Tools.Compiler.LinkTimeCodeGeneration;
using Mosa.Runtime.Linker;

using Mosa.Platforms.x86;
using IR = Mosa.Runtime.CompilerFramework.IR;
using CPUx86 = Mosa.Platforms.x86.CPUx86;

namespace Mosa.Tools.Compiler.x86
{

	/// <summary>
	/// 
	/// </summary>
	public sealed class InterruptBuilderStage : IAssemblyCompilerStage, IPipelineStage
	{
		#region Data Members

		IAssemblyLinker _linker;

		#endregion // Data Members

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"Interrupt Stage"; } }

		#endregion // IPipelineStage Members

		#region IAssemblyCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(AssemblyCompiler compiler)
		{
			_linker = compiler.Pipeline.FindFirst<IAssemblyLinker>();

			CreateISRMethods(compiler);
		}

		#endregion // IAssemblyCompilerStage Members

		#region Internal

		/// <summary>
		/// Finds the method.
		/// </summary>
		/// <param name="rt">The runtime type.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private RuntimeMethod FindMethod(RuntimeType rt, string name)
		{
			foreach (RuntimeMethod method in rt.Methods)
				if (name == method.Name)
					return method;

			throw new MissingMethodException(rt.Name, name);
		}

		/// <summary>
		/// Creates the ISR methods.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		private void CreateISRMethods(AssemblyCompiler compiler)
		{
			// Get RuntimeMethod for the Mosa.Kernel.X86.IDT.InterruptHandler
			RuntimeType rt = RuntimeBase.Instance.TypeLoader.GetType(@"Mosa.Kernel.X86.IDT");
			RuntimeMethod InterruptMethod = FindMethod(rt, "InterruptHandler");

			SigType I1 = new SigType(CilElementType.I1);
			SigType I2 = new SigType(CilElementType.I4);

			RegisterOperand ecx1 = new RegisterOperand(I1, GeneralPurposeRegister.ECX);
			RegisterOperand ecx2 = new RegisterOperand(I2, GeneralPurposeRegister.ECX);

			for (int i = 0; i <= 256; i++) {
				InstructionSet set = new InstructionSet(100);
				Context ctx = new Context(set, -1);

				ctx.AppendInstruction(CPUx86.Instruction.CliInstruction);
				ctx.AppendInstruction(CPUx86.Instruction.PushadInstruction);
				if ((i != 8) && (i < 10 || i > 14)) // For IRQ 8, 10, 11, 12, 13, 14 the cpu automatically pushed the error code
					ctx.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new ConstantOperand(I1, 0x0));
				ctx.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new ConstantOperand(I2, i));
				ctx.AppendInstruction(CPUx86.Instruction.CallInstruction, InterruptMethod);
				// TODO: Replace next two instructions with add esp, 5 ;Stack clearing
				ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, ecx2);
				ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, ecx1);
				ctx.AppendInstruction(CPUx86.Instruction.PopadInstruction);
				ctx.AppendInstruction(CPUx86.Instruction.StiInstruction);
				ctx.AppendInstruction(CPUx86.Instruction.IRetdInstruction);

				CompilerGeneratedMethod method = LinkTimeCodeGenerator.Compile(compiler, @"InterruptISR" + i.ToString(), set);
			}
		}

		#endregion Internal
	}
}
