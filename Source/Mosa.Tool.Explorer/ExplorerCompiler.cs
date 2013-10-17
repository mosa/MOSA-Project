/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
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

namespace Mosa.Tool.Explorer
{
	internal class ExplorerCompiler : BaseCompiler
	{
		private bool emitBinary;

		/// <summary>
		/// Prevents a default instance of the <see cref="ExplorerCompiler" /> class from being created.
		/// </summary>
		/// <param name="architecture">The compiler target architecture.</param>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="typeLayout">The type layout.</param>
		/// <param name="internalTrace">The internal trace.</param>
		/// <param name="compilerOptions">The compiler options.</param>
		public ExplorerCompiler(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, CompilerOptions compilerOptions, bool emitBinary) :
			base(architecture, typeSystem, typeLayout, new CompilationScheduler(typeSystem, true), internalTrace, new ExplorerLinker(), compilerOptions)
		{
			this.emitBinary = emitBinary;

			// Build the assembly compiler pipeline
			Pipeline.AddRange(new ICompilerStage[] {
				new PlugStage(),
				new MethodCompilerSchedulerStage(),
				new TypeLayoutStage(),

				//new LinkerFinalizationStage(),
			});

			architecture.ExtendCompilerPipeline(Pipeline);
		}

		/// <summary>
		/// Creates a method compiler
		/// </summary>
		/// <param name="method">The method to compile.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="instructionSet">The instruction set.</param>
		/// <returns>
		/// An instance of a MethodCompilerBase for the given type/method pair.
		/// </returns>
		public override BaseMethodCompiler CreateMethodCompiler(RuntimeMethod method, BasicBlocks basicBlocks, InstructionSet instructionSet)
		{
			return new ExplorerMethodCompiler(this, method, basicBlocks, instructionSet, emitBinary);
		}

		/// <summary>
		/// Compiles the specified type system.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="typeLayout">The type layout.</param>
		/// <param name="internalTrace">The internal trace.</param>
		/// <param name="platform">The platform.</param>
		/// <param name="enabledSSA">if set to <c>true</c> [enabled ssa].</param>
		/// <param name="emitBinary">if set to <c>true</c> [emit binary].</param>
		public static void Compile(ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, string platform, bool enabledSSA, bool emitBinary)
		{
			IArchitecture architecture;

			switch (platform.ToLower())
			{
				case "x86": architecture = Mosa.Platform.x86.Architecture.CreateArchitecture(Mosa.Platform.x86.ArchitectureFeatureFlags.AutoDetect); break;
				case "avr32": architecture = Mosa.Platform.AVR32.Architecture.CreateArchitecture(Mosa.Platform.AVR32.ArchitectureFeatureFlags.AutoDetect); break;
				default:
					architecture = Mosa.Platform.x86.Architecture.CreateArchitecture(Mosa.Platform.x86.ArchitectureFeatureFlags.AutoDetect); break;
			}

			CompilerOptions compilerOptions = new CompilerOptions();
			compilerOptions.EnableSSA = enabledSSA;
			compilerOptions.EnableSSAOptimizations = enabledSSA && enabledSSA;

			ExplorerCompiler compiler = new ExplorerCompiler(architecture, typeSystem, typeLayout, internalTrace, compilerOptions, emitBinary);

			compiler.Compile();
		}
	}
}