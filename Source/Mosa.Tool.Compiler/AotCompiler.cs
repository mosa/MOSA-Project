/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <sharpos@michaelruck.de>
 */

using System.Collections.Generic;
using System.IO;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using Mosa.Tool.Compiler.Stages;

namespace Mosa.Tool.Compiler
{
	public class AotCompiler : BaseCompiler
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="AotCompiler"/> class.
		/// </summary>
		/// <param name="architecture">The architecture.</param>
		/// <param name="linker">The linker.</param>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="typeLayout">The type layout.</param>
		/// <param name="internalTrace">The internal trace.</param>
		/// <param name="compilerOptions">The compiler options.</param>
		public AotCompiler(IArchitecture architecture, ILinker linker, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, CompilerOptions compilerOptions)
			: base(architecture, typeSystem, typeLayout, new CompilationScheduler(typeSystem, true), internalTrace, compilerOptions)
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
		/// <returns>
		/// An instance of a MethodCompilerBase for the given type/method pair.
		/// </returns>
		public override BaseMethodCompiler CreateMethodCompiler(RuntimeMethod method)
		{
			return new AotMethodCompiler(this, method);
		}

		/// <summary>
		/// Gets a list of input file names.
		/// </summary>
		private static IEnumerable<string> GetInputFileNames(List<FileInfo> inputFiles)
		{
			foreach (FileInfo file in inputFiles)
				yield return file.FullName;
		}

		public static void Compile(CompilerOptions compilerOptions, List<FileInfo> inputFiles)
		{
			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			assemblyLoader.InitializePrivatePaths(GetInputFileNames(inputFiles));

			foreach (string file in GetInputFileNames(inputFiles))
			{
				assemblyLoader.LoadModule(file);
			}

			ITypeSystem typeSystem = new TypeSystem();
			typeSystem.LoadModules(assemblyLoader.Modules);

			int nativePointerSize;
			int nativePointerAlignment;

			compilerOptions.Architecture.GetTypeRequirements(BuiltInSigType.IntPtr, out nativePointerSize, out nativePointerAlignment);

			TypeLayout typeLayout = new TypeLayout(typeSystem, nativePointerSize, nativePointerAlignment);

			IInternalTrace internalTrace = new BasicInternalTrace();

			AotCompiler aot = new AotCompiler(compilerOptions.Architecture, compilerOptions.Linker, typeSystem, typeLayout, internalTrace, compilerOptions);

			aot.Pipeline.AddRange(new ICompilerStage[] {
				compilerOptions.BootCompilerStage,
				new MethodPipelineExportStage(),
				new PlugStage(),
				new MethodCompilerSchedulerStage(),
				new TypeInitializerSchedulerStage(),
				compilerOptions.BootCompilerStage,
				new TypeLayoutStage(),
				new MetadataStage(),
				new ObjectFileLayoutStage(),
				(ICompilerStage)compilerOptions.Linker,
				compilerOptions.MapFile != null ? new MapFileGenerationStage() : null
			});

			aot.Run();
		}
	}
}
