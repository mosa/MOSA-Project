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
using Mosa.Runtime.InternalTrace;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Options;
using Mosa.Compiler.Linker;

using x86 = Mosa.Platform.x86;

namespace Mosa.Tools.TypeExplorer
{

	class ExplorerAssemblyCompiler : AssemblyCompiler
	{
		private ExplorerAssemblyCompiler(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, CompilerOptionSet compilerOptionSet) :
			base(architecture, typeSystem, typeLayout, internalTrace, compilerOptionSet)
		{
			var linker = new ExplorerLinker();

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
			IMethodCompiler mc = new ExplorerMethodCompiler(this, Architecture, schedulerStage, type, method, internalTrace);
			Architecture.ExtendMethodCompilerPipeline(mc.Pipeline);
			return mc;
		}

		public static void Compile(ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace)
		{
			IArchitecture architecture = x86.Architecture.CreateArchitecture(x86.ArchitectureFeatureFlags.AutoDetect);

			CompilerOptionSet compilerOptionSet = new CompilerOptionSet();

			ExplorerAssemblyCompiler compiler = new ExplorerAssemblyCompiler(architecture, typeSystem, typeLayout, internalTrace, compilerOptionSet);

			compiler.Compile();
		}

	}
}
