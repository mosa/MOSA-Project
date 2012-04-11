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
	public class AssemblyMemberCompilationSchedulerStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, IPipelineStage
	{
		#region Data members

		private ICompilationSchedulerStage scheduler;

		#endregion // Data members

		#region IAssemblyCompilerStage members

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);

			scheduler = compiler.Pipeline.FindFirst<ICompilationSchedulerStage>();

			if (scheduler == null)
				throw new InvalidOperationException(@"No compilation scheduler found in the assembly compiler pipeline.");
		}

		void IAssemblyCompilerStage.Run()
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

				scheduler.ScheduleTypeForCompilation(type);
			}
		}

		#endregion IAssemblyCompilerStage members

	}
}
