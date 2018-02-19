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
		protected BaseCompiler BaseCompiler { get; private set; }

		public CompilerOptions CompilerOptions { get; set; }

		public CompilerTrace CompilerTrace { get; }

		protected MosaModuleLoader ModuleLoader { get; }

		public TypeSystem TypeSystem { get; private set; }

		public MosaTypeLayout TypeLayout { get; private set; }

		public CompilationScheduler CompilationScheduler { get; private set; }

		public BaseLinker Linker { get; private set; }

		public int MaxThreads { get; }

		public MosaCompiler(int maxThreads = 0)
		{
			CompilerOptions = new CompilerOptions();
			CompilerTrace = new CompilerTrace();
			ModuleLoader = new MosaModuleLoader();

			MaxThreads = (maxThreads == 0) ? Environment.ProcessorCount : maxThreads;
		}

		/// <summary>
		/// Gets a list of input file names.
		/// </summary>
		/// <param name="inputFiles">The input files.</param>
		/// <returns></returns>
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
			BaseCompiler.ExecuteThreadedCompile(MaxThreads);
			PostCompile();
		}

		public void Initialize()
		{
			Linker = new BaseLinker(CompilerOptions.BaseAddress, CompilerOptions.Architecture.Endianness, CompilerOptions.Architecture.MachineType, CompilerOptions.EmitSymbols, CompilerOptions.LinkerFormatType);

			BaseCompiler = new BaseCompiler();

			BaseCompiler.Initialize(this);
		}

		public void PreCompile()
		{
			BaseCompiler.PreCompile();
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
			BaseCompiler.ExecuteCompile();
		}

		public void PostCompile()
		{
			BaseCompiler.PostCompile();
		}

		public void Dispose()
		{
			ModuleLoader.Dispose();
		}
	}
}
