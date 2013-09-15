/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.TypeSystem;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	/// <summary>
	///
	/// </summary>
	public class SimCompiler : BaseCompiler
	{
		protected ISimAdapter simAdapter;

		/// <summary>
		/// Prevents a default instance of the <see cref="SimCompiler" /> class from being created.
		/// </summary>
		/// <param name="architecture">The compiler target architecture.</param>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="typeLayout">The type layout.</param>
		/// <param name="linker">The linker.</param>
		/// <param name="compilerOptions">The compiler options.</param>
		/// <param name="internalTrace">The internal trace.</param>
		/// <param name="simAdapter">The sim adapter.</param>
		public SimCompiler(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout, ILinker linker, CompilerOptions compilerOptions, IInternalTrace internalTrace, ISimAdapter simAdapter) :
			base(architecture, typeSystem, typeLayout, new CompilationScheduler(typeSystem, true), internalTrace, linker, compilerOptions)
		{
			this.simAdapter = simAdapter;

			// Build the assembly compiler pipeline
			Pipeline.AddRange(new ICompilerStage[] {
				new PlugStage(),
				new MethodCompilerSchedulerStage(),
				new TypeInitializerSchedulerStage(),
				new SimPowerUpStage(),
				new TypeLayoutStage(),
				new MetadataStage(),
				new LinkerFinalizationStage(),
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
			return new SimMethodCompiler(this, method, simAdapter, basicBlocks, instructionSet);
		}

		/// <summary>
		/// Compiles the specified type system.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="typeLayout">The type layout.</param>
		/// <param name="internalTrace">The internal trace.</param>
		/// <param name="enabledSSA">if set to <c>true</c> [enabled ssa].</param>
		/// <param name="architecture">The architecture.</param>
		/// <param name="simAdapter">The sim adapter.</param>
		/// <param name="linker">The linker.</param>
		public static void Compile(ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, bool enabledSSA, IArchitecture architecture, ISimAdapter simAdapter, ILinker linker)
		{
			CompilerOptions compilerOptions = new CompilerOptions();
			compilerOptions.EnableSSA = enabledSSA;
			compilerOptions.EnableSSAOptimizations = enabledSSA;

			SimCompiler compiler = new SimCompiler(architecture, typeSystem, typeLayout, linker, compilerOptions, internalTrace, simAdapter);

			compiler.Compile();
		}
	}
}