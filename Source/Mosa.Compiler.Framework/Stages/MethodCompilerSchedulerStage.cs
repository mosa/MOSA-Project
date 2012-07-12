/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Schedules compilation of types/methods.
	/// </summary>
	public class MethodCompilerSchedulerStage : BaseCompilerStage, ICompilerStage, IPipelineStage
	{

		#region Data Members

		#endregion // Data Members

		public MethodCompilerSchedulerStage()
		{
		}
		//		newScheduler = new CompilationScheduler(typeSystem);
		#region ICompilerStage members

		void ICompilerStage.Run()
		{
			while (true)
			{
				var method = compiler.Scheduler.GetMethodToCompile();

				if (method == null)
					break;

				compiler.CompileMethod(method);
			}
						
		}

		#endregion // ICompilerStage members

	}
}
