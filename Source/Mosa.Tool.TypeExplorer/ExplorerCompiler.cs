/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.TypeSystem;
using AVR32 = Mosa.Platform.AVR32;
using Null = Mosa.Platform.Null;
using x86 = Mosa.Platform.x86;

namespace Mosa.Tool.TypeExplorer
{

	class ExplorerCompiler : BaseCompiler
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="ExplorerCompiler"/> class from being created.
		/// </summary>
		/// <param name="architecture">The compiler target architecture.</param>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="typeLayout"></param>
		/// <param name="internalTrace"></param>
		/// <param name="compilerOptions"></param>
		private ExplorerCompiler(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, CompilerOptions compilerOptions) :
			base(architecture, typeSystem, typeLayout, new MethodCompilerSchedulerStage(), internalTrace, compilerOptions)
		{
			var linker = new ExplorerLinker();

			// Build the assembly compiler pipeline
			Pipeline.AddRange(new ICompilerStage[] {
				new TypeSchedulerStage(),
				(MethodCompilerSchedulerStage)base.Scheduler, // HACK
				new TypeLayoutStage(),
				linker
			});

			architecture.ExtendCompilerPipeline(Pipeline);
		}

		/// <summary>
		/// Creates a method compiler
		/// </summary>
		/// <param name="method">The method to compile.</param>
		/// <returns>
		/// An instance of a MethodCompilerBase for the given type/method pair.
		/// </returns>
		public override BaseMethodCompiler CreateMethodCompiler(RuntimeMethod method)
		{
			BaseMethodCompiler mc = new ExplorerMethodCompiler(this, method);
			Architecture.ExtendMethodCompilerPipeline(mc.Pipeline);
			return mc;
		}

		public static void Compile(ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, string platform, bool enabledSSA)
		{
			IArchitecture architecture;

			switch (platform.ToLower())
			{
				case "null": architecture = Null.Architecture.CreateArchitecture(Null.ArchitectureFeatureFlags.AutoDetect); break;
				case "x86": architecture = x86.Architecture.CreateArchitecture(x86.ArchitectureFeatureFlags.AutoDetect); break;
				case "avr32": architecture = AVR32.Architecture.CreateArchitecture(AVR32.ArchitectureFeatureFlags.AutoDetect); break;
				default:
					architecture = x86.Architecture.CreateArchitecture(x86.ArchitectureFeatureFlags.AutoDetect); break;
			}

			CompilerOptions compilerOptions = new CompilerOptions();
			compilerOptions.EnableSSA = enabledSSA;
			compilerOptions.EnableSSAOptimizations = enabledSSA && enabledSSA;

			ExplorerCompiler compiler = new ExplorerCompiler(architecture, typeSystem, typeLayout, internalTrace, compilerOptions);

			compiler.Compile();
		}

	}
}
