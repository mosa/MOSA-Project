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
	public sealed class ExceptionVectorStage : BaseCompilerStage
	{
		protected override void Run()
		{
			CreateExceptionVector();
		}

		#region Internal

		/// <summary>
		/// Creates the ISR methods.
		/// </summary>
		private void CreateExceptionVector()
		{
			var type = TypeSystem.GetTypeByName("Mosa.Kernel.x86", "IDT");

			if (type == null)
				return;

			var method = type.FindMethodByName("ExceptionHandlerType");

			if (method == null)
				return;

			Operand exceptionMethod = Operand.CreateSymbolFromMethod(TypeSystem, method);

			Operand esp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);

			BasicBlocks basicBlocks = new BasicBlocks();
			InstructionSet instructionSet = new InstructionSet(25);
			Context ctx = instructionSet.CreateNewBlock(basicBlocks);
			basicBlocks.AddHeaderBlock(ctx.BasicBlock);

			// TODO - setup stack for call to the managed exception handler

			//1.
			//2.

			//3. Call the managed exception handler
			ctx.AppendInstruction(X86.Call, null, exceptionMethod);

			var vectorMethod = Compiler.CreateLinkerMethod("ExceptionVector");

			Compiler.CompileMethod(vectorMethod, basicBlocks, instructionSet);
		}

		#endregion Internal
	}
}