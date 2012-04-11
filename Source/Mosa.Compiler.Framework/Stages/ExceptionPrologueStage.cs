/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System.Collections;
using System.Collections.Generic;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.Framework;

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
			foreach (ExceptionHandlingClause clause in methodCompiler.ExceptionClauseHeader.Clauses)
			{
				if (clause.ExceptionHandler == ExceptionHandlerType.Exception)
				{
					var typeToken = new Token(clause.ClassToken);

					RuntimeType type = methodCompiler.Method.Module.GetType(typeToken);

					var block = FindBlock(clause.HandlerOffset);

					var context = new Context(instructionSet, block).InsertBefore();

					SigType sigType = new ClassSigType(typeToken);
					Operand exceptionObject = methodCompiler.CreateTemporary(sigType);

					context.SetInstruction(IR.Instruction.ExceptionPrologueInstruction, exceptionObject);
				}	
			}
		}

	}
}
