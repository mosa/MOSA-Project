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
	/// This stage inserts the ExceptionStart IR instruction at the beginning of each exception block.
	/// </summary>
	public class ExceptionPrologueStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			foreach (var clause in MethodCompiler.Method.ExceptionHandlers)
			{
				if (clause.HandlerType == ExceptionHandlerType.Exception)
				{
					var tryHandler = BasicBlocks.GetByLabel(clause.HandlerStart);

					var context = new Context(InstructionSet, tryHandler);

					var exceptionObject = MethodCompiler.CreateVirtualRegister(clause.Type);

					context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);
				}
			}
		}
	}
}
