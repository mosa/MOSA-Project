/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Schedules compilation of types/methods.
	/// </summary>
	public class MethodCompilerSchedulerStage : BaseCompilerStage
	{
		protected override void Run()
		{
			while (true)
			{
				var method = CompilationScheduler.GetMethodToCompile();

				if (method == null)
					break;

				Compiler.CompileMethod(method, null, null);

				CompilerTrace.CompilerEventListener.SubmitMethodStatus(
					CompilationScheduler.TotalMethods,
					CompilationScheduler.TotalMethods - CompilationScheduler.TotalQueuedMethods
				);
			}
		}
	}
}