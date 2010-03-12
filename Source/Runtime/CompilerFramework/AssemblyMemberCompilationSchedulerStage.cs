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
	public class AssemblyMemberCompilationSchedulerStage : IAssemblyCompilerStage, IPipelineStage
    {	
		private AssemblyCompiler compiler;
		
		private ICompilationSchedulerStage scheduler;
		
		public string Name 
		{ 
			get 
			{ 
				return @"Method Compiler Builder"; 
			} 
		}
		
		public void Setup(AssemblyCompiler compiler)
		{
			ICompilationSchedulerStage scheduler = compiler.Pipeline.FindFirst<ICompilationSchedulerStage>();
			if (scheduler == null)
				throw new InvalidOperationException(@"No compilation scheduler found in the assembly compiler pipeline.");
			
			this.compiler = compiler;
			this.scheduler = scheduler;
		}

		public void Run()
        {		
            ReadOnlyRuntimeTypeListView types = RuntimeBase.Instance.TypeLoader.GetTypesFromModule(this.compiler.Assembly);
            foreach (RuntimeType type in types)
            {
                // Do not schedule generic types, they're scheduled on demand.
                if (type.IsGeneric)
                    continue;
				
				this.scheduler.ScheduleTypeForCompilation(type);
            }
        }
    }
}
