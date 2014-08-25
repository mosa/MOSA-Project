/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using Mosa.Compiler.Framework.IR;

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stage inserts IR instructions related to protected blocks.
	/// </summary>
	public class ProtectedBlockStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			MethodCompiler.CreateExceptionReturnOperands();
			
			// Handler Code
			foreach (var clause in MethodCompiler.Method.ExceptionBlocks)
			{
				var block = BasicBlocks.GetByLabel(clause.TryOffset);

				//

			}
			
			// Handler Code
			foreach (var clause in MethodCompiler.Method.ExceptionBlocks)
			{
				var block = BasicBlocks.GetByLabel(clause.HandlerOffset);

				var context = new Context(InstructionSet, block);

				if (clause.HandlerType == ExceptionHandlerType.Exception)
				{
					var exceptionObject = MethodCompiler.CreateVirtualRegister(clause.Type);

					context.AppendInstruction(IRInstruction.StartException, exceptionObject);
				}
				else if (clause.HandlerType == ExceptionHandlerType.Finally)
				{
					context.AppendInstruction(IRInstruction.StartFinally);
				}
			}


		}
	}
}