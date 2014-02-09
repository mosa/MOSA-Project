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
	public class MethodCompilerSchedulerStage : BaseCompilerStage, ICompilerStage, IPipelineStage
	{
		#region Data Members

		#endregion Data Members

		public MethodCompilerSchedulerStage()
		{
		}

		#region ICompilerStage members

		void ICompilerStage.Run()
		{
			while (true)
			{
				var method = compiler.Scheduler.GetMethodToCompile();

				if (method == null)
					break;

				compiler.CompileMethod(method, null, null);
			}
		}

		#endregion ICompilerStage members
	}
}