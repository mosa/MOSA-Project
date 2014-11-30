/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Utility.Aot
{
	public class AotCompiler : BaseCompiler
	{
		/// <summary>
		/// Extends the compiler setup.
		/// </summary>
		public override void ExtendCompilerSetup()
		{
			var bootStage = CompilerOptions.BootStageFactory != null ? CompilerOptions.BootStageFactory() : null;

			PreCompilePipeline.Add(new ICompilerStage[] {
				bootStage,
				new PlugStage(),
				//new MethodCompilerSchedulerStage(),
			});

			PostCompilePipeline.Add(new ICompilerStage[] {
				new TypeInitializerSchedulerStage(),
				bootStage,
				new MethodLookupTableStage(),
				new MethodExceptionLookupTableStage(),
				new MetadataStage(),
				new LinkerFinalizationStage(),
				CompilerOptions.MapFile != null ? new MapFileGenerationStage() : null
			});
		}

		/// <summary>
		/// Creates the method compiler.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="instructionSet">The instruction set.</param>
		/// <returns>
		/// An instance of a MethodCompilerBase for the given type/method pair.
		/// </returns>
		protected override BaseMethodCompiler CreateMethodCompiler(MosaMethod method, BasicBlocks basicBlocks, InstructionSet instructionSet, int threadID)
		{
			return new AotMethodCompiler(this, method, basicBlocks, instructionSet, threadID);
		}
	}
}