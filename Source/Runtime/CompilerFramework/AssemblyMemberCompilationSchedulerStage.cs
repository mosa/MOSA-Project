/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Runtime.CompilerFramework
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;

	using Mosa.Runtime.Vm;

	/// <summary>
	/// Schedules all types of an assembly for compilation.
	/// </summary>
	public class AssemblyMemberCompilationSchedulerStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, IPipelineStage
	{
		#region Data members

		private ICompilationSchedulerStage scheduler;

		#endregion // Data members

		#region IPipelineStage members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"AssemblyMemberCompilationSchedulerStage"; } }

		#endregion // IPipelineStage

		#region IAssemblyCompilerStage members

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);

			ICompilationSchedulerStage scheduler = compiler.Pipeline.FindFirst<ICompilationSchedulerStage>();

			if (scheduler == null)
				throw new InvalidOperationException(@"No compilation scheduler found in the assembly compiler pipeline.");

			this.scheduler = scheduler;
		}

		void IAssemblyCompilerStage.Run()
		{
			foreach (RuntimeType type in typeSystem.GetCompiledTypes())
			{
				if (type.ContainsGenericParameters)
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
