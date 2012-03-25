/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */


using Mosa.Compiler.Framework;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.TypeSystem;
using AVR32 = Mosa.Platform.AVR32;
using x86 = Mosa.Platform.x86;

namespace Mosa.Tool.TypeExplorer
{

	class ExplorerAssemblyCompiler : AssemblyCompiler
	{
		private ExplorerAssemblyCompiler(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, CompilerOptions compilerOptions) :
			base(architecture, typeSystem, typeLayout, internalTrace, compilerOptions)
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
			IMethodCompiler mc = new ExplorerMethodCompiler(this, schedulerStage, type, method, CompilerOptions);
			Architecture.ExtendMethodCompilerPipeline(mc.Pipeline);
			return mc;
		}

		public static void Compile(ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, string platform, bool enabledSSA)
		{
			IArchitecture architecture;

			switch (platform.ToLower())
			{
				case "x86": architecture = x86.Architecture.CreateArchitecture(x86.ArchitectureFeatureFlags.AutoDetect); break;
				case "avr32": architecture = AVR32.Architecture.CreateArchitecture(AVR32.ArchitectureFeatureFlags.AutoDetect); break;
				default:
					architecture = x86.Architecture.CreateArchitecture(x86.ArchitectureFeatureFlags.AutoDetect); break;
			}

			CompilerOptions compilerOptions = new CompilerOptions();
			compilerOptions.EnableSSA = enabledSSA;

			ExplorerAssemblyCompiler compiler = new ExplorerAssemblyCompiler(architecture, typeSystem, typeLayout, internalTrace, compilerOptions);

			compiler.Compile();
		}

	}
}
