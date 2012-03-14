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
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using Mosa.Tool.Compiler.Stages;

namespace Mosa.Tool.Compiler
{
	public class AotAssemblyCompiler : AssemblyCompiler
	{

		public AotAssemblyCompiler(IArchitecture architecture, IAssemblyLinker linker, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, CompilerOptions compilerOptions)
			: base(architecture, typeSystem, typeLayout, internalTrace, compilerOptions)
		{

		}

		public void Run()
		{
			// Build the default assembly compiler pipeline
			Architecture.ExtendAssemblyCompilerPipeline(this.Pipeline);

			// Run the compiler
			Compile();
		}

		/// <summary>
		/// Creates the method compiler.
		/// </summary>
		/// <param name="compilationScheduler">The compilation scheduler.</param>
		/// <param name="type">The type.</param>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		public override IMethodCompiler CreateMethodCompiler(ICompilationSchedulerStage compilationScheduler, RuntimeType type, RuntimeMethod method)
		{
			IMethodCompiler mc = new AotMethodCompiler(this, compilationScheduler, type, method, CompilerOptions);
			this.Architecture.ExtendMethodCompilerPipeline(mc.Pipeline);
			return mc;
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

			IInternalTrace internalLog = new BasicInternalTrace();

			using (AotAssemblyCompiler aot = new AotAssemblyCompiler(compilerOptions.Architecture, compilerOptions.Linker, typeSystem, typeLayout, internalLog, compilerOptions))
			{
				aot.Pipeline.AddRange(new IAssemblyCompilerStage[] 
				{
					compilerOptions.BootCompilerStage,
					new MethodPipelineExportStage(),
					new DelegateTypePatchStage(),
					new PlugStage(),
					new AssemblyMemberCompilationSchedulerStage(), 
					new MethodCompilerSchedulerStage(),
					new TypeInitializerSchedulerStage(),
					new TypeLayoutStage(),
					new MetadataStage(),
					compilerOptions.BootCompilerStage,
					new ObjectFileLayoutStage(),
					(IAssemblyCompilerStage)compilerOptions.Linker,
					compilerOptions.MapFile != null ? new MapFileGenerationStage() : null
				});

				aot.Run();
			}
		}
	}
}
