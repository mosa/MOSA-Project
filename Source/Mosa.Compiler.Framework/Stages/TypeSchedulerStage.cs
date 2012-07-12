/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Schedules all types of an assembly for compilation.
	/// </summary>
	public class TypeSchedulerStage : BaseCompilerStage, ICompilerStage, IPipelineStage
	{
		
		#region ICompilerStage members

		void ICompilerStage.Run()
		{
			//foreach (RuntimeType type in typeSystem.GetAllTypes())
			//{
			//    if (type.ContainsOpenGenericParameters)
			//        continue;

			//    compiler.Scheduler.TrackTypeAllocated(type);
			//}
		}

		#endregion ICompilerStage members

	}
}
