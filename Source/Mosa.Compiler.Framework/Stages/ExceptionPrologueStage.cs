/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
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
	/// This stage inserts the ExceptionPrologue IR instruction at the beginning of each exception block.
	/// </summary>
	public class ExceptionPrologueStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			// Handler Code
			foreach (var clause in MethodCompiler.Method.ExceptionBlocks)
			{
				var block = BasicBlocks.GetByLabel(clause.HandlerOffset);

				var context = new Context(InstructionSet, block).InsertBefore();

				if (clause.HandlerType == ExceptionHandlerType.Exception)
				{
					var exceptionObject = MethodCompiler.CreateVirtualRegister(clause.Type);

					context.SetInstruction(IRInstruction.ExceptionPrologue, exceptionObject);
				}
				else if (clause.HandlerType == ExceptionHandlerType.Finally)
				{
					var exceptionObject = MethodCompiler.CreateVirtualRegister(clause.Type);

					context.SetInstruction(IRInstruction.FinallyPrologue, exceptionObject);
				}
			}
		}
	}
}