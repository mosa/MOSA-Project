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

		#endregion ICompilerStage Members

		#region Internal

		/// <summary>
		/// Creates the ISR methods.
		/// </summary>
		private void CreateExceptionVector()
		{
			var type = typeSystem.GetTypeByName("Mosa.Kernel.x86", "IDT");

			if (type == null)
				return;

			var method = TypeSystem.GetMethodByName(type, "ExceptionHandlerType");

			if (method == null)
				return;

			Operand exceptionMethod = Operand.CreateSymbolFromMethod(typeSystem, method);

			Operand esp = Operand.CreateCPURegister(typeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);

			BasicBlocks basicBlocks = new BasicBlocks();
			InstructionSet instructionSet = new InstructionSet(25);
			Context ctx = instructionSet.CreateNewBlock(basicBlocks);
			basicBlocks.AddHeaderBlock(ctx.BasicBlock);

			// TODO - setup stack for call to the managed exception handler

			//1.
			//2.

			//3. Call the managed exception handler
			ctx.AppendInstruction(X86.Call, null, exceptionMethod);

			var vectorMethod = compiler.CreateLinkerMethod("ExceptionVector");

			compiler.CompileMethod(vectorMethod, basicBlocks, instructionSet);
		}

		#endregion Internal
	}
}