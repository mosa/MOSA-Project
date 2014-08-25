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
				var tryBlock = BasicBlocks.GetByLabel(clause.TryOffset);

				var context = new Context(InstructionSet, tryBlock);

				context.AppendInstruction(IRInstruction.TryStart);

				// find handler block
				var handlerBlock = BasicBlocks.GetByLabel(clause.HandlerOffset);

				context = new Context(InstructionSet, handlerBlock);

				if (clause.HandlerType == ExceptionHandlerType.Exception)
				{
					var exceptionObject = MethodCompiler.CreateVirtualRegister(clause.Type);

					context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);
				}
				else if (clause.HandlerType == ExceptionHandlerType.Finally)
				{
					context.AppendInstruction(IRInstruction.FinallyStart);
				}
			}

		}
	}
}