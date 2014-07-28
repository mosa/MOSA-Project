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
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.Aot
{
	public class AotCompiler : BaseCompiler
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AotCompiler" /> class.
		/// </summary>
		/// <param name="architecture">The architecture.</param>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="typeLayout">The type layout.</param>
		/// <param name="compilerTrace">The internal trace.</param>
		/// <param name="compilerOptions">The compiler options.</param>
		public AotCompiler(BaseArchitecture architecture, TypeSystem typeSystem, MosaTypeLayout typeLayout, CompilerTrace compilerTrace, CompilerOptions compilerOptions)
			: base(architecture, typeSystem, typeLayout, new CompilationScheduler(typeSystem, true), compilerTrace, null, compilerOptions)
		{
		}

		/// <summary>
		/// Runs this instance.
		/// </summary>
		public void Run()
		{
			// Build the default compiler pipeline
			Architecture.ExtendCompilerPipeline(this.Pipeline);

			// Run the compiler
			Compile();
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
		public override BaseMethodCompiler CreateMethodCompiler(MosaMethod method, BasicBlocks basicBlocks, InstructionSet instructionSet)
		{
			return new AotMethodCompiler(this, method, basicBlocks, instructionSet);
		}

		/// <summary>
		/// Gets a list of input file names.
		/// </summary>
		private static IEnumerable<string> GetInputFileNames(List<FileInfo> inputFiles)
		{
			foreach (FileInfo file in inputFiles)
				yield return file.FullName;
		}


		public static void Compile(CompilerOptions compilerOptions, List<FileInfo> inputFiles, CompilerTrace compilerTrace)
		{
			var moduleLoader = new MosaModuleLoader();

			moduleLoader.AddPrivatePath(GetInputFileNames(inputFiles));

			foreach (string file in GetInputFileNames(inputFiles))
			{
				moduleLoader.LoadModuleFromFile(file);
			}

			var typeSystem = TypeSystem.Load(moduleLoader.CreateMetadata());
			MosaTypeLayout typeLayout = new MosaTypeLayout(typeSystem, compilerOptions.Architecture.NativePointerSize, compilerOptions.Architecture.NativeAlignment);

			AotCompiler aot = new AotCompiler(compilerOptions.Architecture, typeSystem, typeLayout, compilerTrace, compilerOptions);

			var bootStage = compilerOptions.BootStageFactory != null ? compilerOptions.BootStageFactory() : null;

			aot.Pipeline.Add(new ICompilerStage[] {
				bootStage,
				compilerOptions.MethodPipelineExportDirectory != null ?  new MethodPipelineExportStage(): null,
				new PlugStage(),
				new MethodCompilerSchedulerStage(),
				new TypeInitializerSchedulerStage(),
				bootStage,
				new MetadataStage(),
				new LinkerFinalizationStage(),
				compilerOptions.MapFile != null ? new MapFileGenerationStage() : null
			});

			aot.Run();
		}
	}
}