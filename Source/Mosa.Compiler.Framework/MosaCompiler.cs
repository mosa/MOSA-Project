// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Trace;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Framework
{
	public class MosaCompiler : IDisposable
	{
		protected Compiler Compiler { get; private set; }

		public CompilerOptions CompilerOptions { get; set; } = new CompilerOptions();

		public CompilerTrace CompilerTrace { get; } = new CompilerTrace();

		protected MosaModuleLoader ModuleLoader { get; } = new MosaModuleLoader();

		public TypeSystem TypeSystem { get; private set; }

		public MosaTypeLayout TypeLayout { get; private set; }

		public CompilationScheduler CompilationScheduler { get; private set; }

		public BaseLinker Linker { get; private set; }

		public List<BaseCompilerExtension> CompilerExtensions { get; } = new List<BaseCompilerExtension>();

		public int MaxThreads { get; }

		public MosaCompiler(List<BaseCompilerExtension> compilerExtensions = null, int maxThreads = 0)
		{
			MaxThreads = (maxThreads == 0) ? Environment.ProcessorCount : maxThreads;

			if (compilerExtensions != null)
			{
				CompilerExtensions.AddRange(compilerExtensions);
			}
		}

		private static IEnumerable<string> GetInputFileNames(List<FileInfo> inputFiles)
		{
			foreach (var file in inputFiles)
			{
				yield return file.FullName;
			}
		}

		public void Load(List<FileInfo> inputFiles)
		{
			ModuleLoader.AddPrivatePath(GetInputFileNames(inputFiles));

			foreach (string file in GetInputFileNames(inputFiles))
			{
				ModuleLoader.LoadModuleFromFile(file);
			}

			Load(TypeSystem.Load(ModuleLoader.CreateMetadata()));
		}

		public void Load(TypeSystem typeSystem)
		{
			TypeSystem = typeSystem;

			TypeLayout = new MosaTypeLayout(typeSystem, CompilerOptions.Architecture.NativePointerSize, CompilerOptions.Architecture.NativeAlignment);

			CompilationScheduler = new CompilationScheduler();
		}

		public void Execute()
		{
			Initialize();
			PreCompile();
			ScheduleAll();
			Compile();
			PostCompile();
		}

		public void ExecuteThreaded()
		{
			Initialize();
			PreCompile();
			ScheduleAll();
			Compiler.ExecuteThreadedCompile(MaxThreads);
			PostCompile();
		}

		public void Initialize()
		{
			Linker = new BaseLinker(CompilerOptions.BaseAddress, CompilerOptions.Architecture.Endianness, CompilerOptions.Architecture.MachineType, CompilerOptions.EmitSymbols, CompilerOptions.LinkerFormatType);

			Compiler = new Compiler(this);
		}

		public void PreCompile()
		{
			Compiler.PreCompile();
		}

		public void ScheduleAll()
		{
			CompilationScheduler.ScheduleAll(TypeSystem);
		}

		public void Schedule(MosaType type)
		{
			CompilationScheduler.Schedule(type);
		}

		public void Schedule(MosaMethod method)
		{
			CompilationScheduler.Schedule(method);
		}

		public void Compile()
		{
			Compiler.ExecuteCompile();
		}

		public void PostCompile()
		{
			Compiler.PostCompile();
		}

		public void Dispose()
		{
			ModuleLoader.Dispose();
		}
	}
}
