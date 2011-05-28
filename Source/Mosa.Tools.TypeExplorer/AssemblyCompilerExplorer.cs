/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.InternalLog;
using Mosa.Runtime.Linker;
using Mosa.Compiler.Linker;

using x86 = Mosa.Platform.x86;

namespace Mosa.Tools.TypeExplorer
{

	class AssemblyCompilerExplorer : AssemblyCompiler
	{
		private AssemblyCompilerExplorer(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalLog internalLog) :
			base(architecture, typeSystem, typeLayout, internalLog)
		{
			var linker = new LinkerStub();

			// Build the assembly compiler pipeline
			Pipeline.AddRange(new IAssemblyCompilerStage[] {
				new AssemblyMemberCompilationSchedulerStage(),
				new MethodCompilerSchedulerStage(),
				new TypeLayoutStage(),
				linker
			});

			architecture.ExtendAssemblyCompilerPipeline(Pipeline);
		}

		public override IMethodCompiler CreateMethodCompiler(ICompilationSchedulerStage schedulerStage, RuntimeType type, RuntimeMethod method)
		{
			IMethodCompiler mc = new MethodCompilerExplorer(this, Architecture, schedulerStage, type, method, internalLog);
			Architecture.ExtendMethodCompilerPipeline(mc.Pipeline);
			return mc;
		}

		public static void Compile(ITypeSystem typeSystem)
		{
			IArchitecture architecture = x86.Architecture.CreateArchitecture(x86.ArchitectureFeatureFlags.AutoDetect);

			// FIXME: get from architecture
			TypeLayout typeLayout = new TypeLayout(typeSystem, 4, 4);

			AssemblyCompilerExplorer compiler = new AssemblyCompilerExplorer(architecture, typeSystem, typeLayout, null);

			compiler.Compile();
		}

	}
}
