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
		#region Data members

		#endregion // Data members

		#region ICompilerStage members

		void ICompilerStage.Setup(BaseCompiler compiler)
		{
			base.Setup(compiler);
		}

		void ICompilerStage.Run()
		{
			foreach (RuntimeType type in typeSystem.GetAllTypes())
			{
				if (type.ContainsOpenGenericParameters)
					continue;

				// Do not schedule generic types, they're scheduled on demand.
				if (type.IsGeneric)
					continue;

				if (type.IsModule)
					continue;

				compiler.Scheduler.ScheduleTypeForCompilation(type);
			}
		}

		#endregion ICompilerStage members

	}
}
