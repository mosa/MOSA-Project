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
using Mosa.Compiler.Linker;
using Mosa.Compiler.Framework;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.TypeSystem;
using Mosa.Tools.Compiler.Stages;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Tools.Compiler
{
	public class AotAssemblyCompiler : AssemblyCompiler
	{
		//IAssemblyLinker linker;

		public AotAssemblyCompiler(IArchitecture architecture, IAssemblyLinker linker, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalLog, CompilerOptions compilerOptions)
			: base(architecture, typeSystem, typeLayout, internalLog, compilerOptions)
		{
			//this.linker = linker;

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
			IMethodCompiler mc = new AotMethodCompiler(this, compilationScheduler, type, method, internalTrace);
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

			IArchitecture architecture = compilerOptions.Architecture;
			architecture.GetTypeRequirements(BuiltInSigType.IntPtr, out nativePointerSize, out nativePointerAlignment);

			TypeLayout typeLayout = new TypeLayout(typeSystem, nativePointerSize, nativePointerAlignment);

			IInternalTrace internalLog = new BasicInternalTrace();

			IAssemblyLinker linker = compilerOptions.Linker;

			using (AotAssemblyCompiler aot = new AotAssemblyCompiler(architecture, linker, typeSystem, typeLayout, internalLog, compilerOptions))
			{
				aot.Pipeline.AddRange(new IAssemblyCompilerStage[] 
				{
					compilerOptions.BootCompilerStage,
					new AssemblyMemberCompilationSchedulerStage(), 
					new MethodCompilerSchedulerStage(),
					new TypeInitializerSchedulerStage(),
					new TypeLayoutStage(),
					new MethodTableBuilderStage(),
					//new MetadataStage(),
					compilerOptions.BootCompilerStage,
					new CilHeaderBuilderStage(),
					new ObjectFileLayoutStage(),
					(IAssemblyCompilerStage)compilerOptions.Linker,
					compilerOptions.MapFile != null ? new MapFileGenerationStage() : null
				});

				aot.Run();
			}
		}
	}
}
