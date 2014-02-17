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
	public class ExceptionPrologueStage : BaseMethodCompilerStage, IMethodCompilerStage
	{
		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			// Handler Code
			foreach (var clause in methodCompiler.Method.ExceptionBlocks)
			{
				if (clause.HandlerType == ExceptionHandlerType.Exception)
				{
					var block = basicBlocks.GetByLabel(clause.HandlerOffset);

					var context = new Context(instructionSet, block).InsertBefore();

					Operand exceptionObject = methodCompiler.CreateVirtualRegister(clause.Type);

					context.SetInstruction(IRInstruction.ExceptionPrologue, exceptionObject);
				}
			}
		}
	}
}