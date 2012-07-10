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
	public sealed class ExceptionVectorStage : BaseCompilerStage, ICompilerStage, IPipelineStage
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
			CreateExceptionVector();
		}

		#endregion // ICompilerStage Members

		#region Internal


		/// <summary>
		/// Creates the ISR methods.
		/// </summary>
		private void CreateExceptionVector()
		{
			RuntimeType runtimeType = typeSystem.GetType(@"Mosa.Kernel.x86.IDT");

			if (runtimeType == null)
				return;

			RuntimeMethod runtimeMethod = runtimeType.FindMethod(@"ExceptionHandler");

			if (runtimeMethod == null)
				return;

			Operand exceptionMethod = Operand.CreateSymbolFromMethod(runtimeMethod);

			Operand esp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ESP);

			InstructionSet instructionSet = new InstructionSet(100);
			Context ctx = new Context(instructionSet);

			// TODO - setup stack for call to the managed exception handler

			//1. 
			//2. 
			
			//3. Call the managed exception handler
			ctx.AppendInstruction(X86.Call, null, exceptionMethod);			

			LinkTimeCodeGenerator.Compile(this.compiler, @"ExceptionVector", instructionSet, typeSystem);

		}

		#endregion Internal
	}
}
