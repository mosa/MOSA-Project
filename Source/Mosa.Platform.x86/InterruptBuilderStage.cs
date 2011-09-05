/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;


namespace Mosa.Platform.x86
{

	/// <summary>
	/// 
	/// </summary>
	public sealed class InterruptBuilderStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, IPipelineStage
	{
		#region Data Members

		#endregion // Data Members

		#region IAssemblyCompilerStage Members

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);

			//linker = RetrieveAssemblyLinkerFromCompiler();
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IAssemblyCompilerStage.Run()
		{
			CreateISRMethods();
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
		private void CreateISRMethods()
		{
			// Get RuntimeMethod for the Mosa.Kernel.x86.IDT.InterruptHandler
			RuntimeType rt = typeSystem.GetType(@"Mosa.Kernel.x86.IDT");
			if (rt == null)
			{
				return;
			}

			RuntimeMethod InterruptMethod = FindMethod(rt, "ProcessInterrupt");
			if (InterruptMethod == null)
			{
				return;
			}

			SymbolOperand interruptMethod = SymbolOperand.FromMethod(InterruptMethod);

			SigType I1 = new SigType(CilElementType.I1);
			SigType I4 = new SigType(CilElementType.I4);

			RegisterOperand esp = new RegisterOperand(I4, GeneralPurposeRegister.ESP);

			for (int i = 0; i <= 255; i++)
			{
				InstructionSet instructionSet = new InstructionSet(100);
				Context ctx = new Context(instructionSet);

				ctx.AppendInstruction(CPUx86.Instruction.CliInstruction);
				if (i <= 7 || i >= 16 | i == 9) // For IRQ 8, 10, 11, 12, 13, 14 the cpu will automatically pushed the error code
					ctx.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new ConstantOperand(I1, 0x0));
				ctx.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new ConstantOperand(I1, (byte)i));
				ctx.AppendInstruction(CPUx86.Instruction.PushadInstruction);
				ctx.AppendInstruction(CPUx86.Instruction.CallInstruction, null, interruptMethod);
				ctx.AppendInstruction(CPUx86.Instruction.PopadInstruction);
				ctx.AppendInstruction(CPUx86.Instruction.AddInstruction, esp, new ConstantOperand(I4, 0x08));
				ctx.AppendInstruction(CPUx86.Instruction.StiInstruction);
				ctx.AppendInstruction(CPUx86.Instruction.IRetdInstruction);

				LinkTimeCodeGenerator.Compile(this.compiler, @"InterruptISR" + i.ToString(), instructionSet, typeSystem);
			}
		}

		#endregion Internal
	}
}
