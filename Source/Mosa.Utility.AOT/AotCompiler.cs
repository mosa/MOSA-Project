// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CompilerStages;
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

			CompilePipeline.Add(new ICompilerStage[] {
				bootStage,
				new PlugStage(),
				new TypeInitializerSchedulerStage(),
				new MethodLookupTableStage(),
				new MethodExceptionLookupTableStage(),
				new MetadataStage(),
				new LinkerFinalizationStage(),
				CompilerOptions.MapFile != null ? new MapFileGenerationStage() : null,
				CompilerOptions.DebugFile != null ? new DebugFileGenerationStage() : null
			});
		}

		/// <summary>
		/// Creates the method compiler.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="threadID"></param>
		/// <returns>
		/// An instance of a MethodCompilerBase for the given type/method pair.
		/// </returns>
		protected override BaseMethodCompiler CreateMethodCompiler(MosaMethod method, BasicBlocks basicBlocks, int threadID)
		{
			return new AotMethodCompiler(this, method, basicBlocks, threadID);
		}
	}
}
