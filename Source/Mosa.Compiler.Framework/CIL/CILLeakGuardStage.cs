/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (aka Michael Ruck, grover) <sharpos@michaelruck.de>
 */


namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Logs CIL instruction, which leak past IR transformation.
	/// </summary>
	public class CILLeakGuardStage : BaseMethodCompilerStage, IMethodCompilerStage
	{
		public CILLeakGuardStage()
		{
			this.MustThrowCompilationException = false;
		}

		/// <summary>
		/// Determines if this stage throws a compilation exception, if a CIL instruction is detected.
		/// </summary>
		public bool MustThrowCompilationException { get; set; }

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			for (int index = 0; index < this.basicBlocks.Count; index++)
			{
				for (Context ctx = new Context(instructionSet, basicBlocks[index]); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					IInstruction instruction = ctx.Instruction;
					if (instruction is ICILInstruction)
					{
						this.ThrowCompilationException(ctx);
					}
				}
			}
		}

		/// <summary>
		/// Logs and optionally throws a compilation exception for the given context.
		/// </summary>
		/// <param name="context">The context to log and throw.</param>
		private void ThrowCompilationException(Context context)
		{
			string message = @"Leaking CIL instruction to late stages. Instruction " + context.Instruction.ToString(context) + @" at " + context.Label + @" in method " + this.methodCompiler.Method;

			Trace(InternalTrace.CompilerEvent.Error, message);

			if (this.MustThrowCompilationException == true)
			{
				throw new CompilationException(message);
			}
		}
	}
}
