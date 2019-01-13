// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	public class MosaCompiler
	{
		public CompilerOptions CompilerOptions { get; set; } = new CompilerOptions();

		public CompilerTrace CompilerTrace { get; } = new CompilerTrace();

		public TypeSystem TypeSystem { get; private set; }

		public MosaTypeLayout TypeLayout { get; private set; }

		public CompilationScheduler CompilationScheduler { get; private set; }

		public MosaLinker Linker { get; private set; }

		public List<BaseCompilerExtension> CompilerExtensions { get; } = new List<BaseCompilerExtension>();

		public int MaxThreads { get; }

		protected Compiler Compiler { get; private set; }

		public MosaCompiler(List<BaseCompilerExtension> compilerExtensions = null, int maxThreads = 0)
		{
			MaxThreads = (maxThreads == 0) ? Environment.ProcessorCount : maxThreads;

			if (compilerExtensions != null)
			{
				CompilerExtensions.AddRange(compilerExtensions);
			}
		}

		public void Load()
		{
			var moduleLoader = new MosaModuleLoader();

			moduleLoader.AddSearchPaths(CompilerOptions.SearchPaths);

			moduleLoader.LoadModuleFromFiles(CompilerOptions.SourceFiles);

			var typeSystem = TypeSystem.Load(moduleLoader.CreateMetadata());

			Load(typeSystem);
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
			Linker = new MosaLinker(CompilerOptions.BaseAddress, CompilerOptions.Architecture.Endianness, CompilerOptions.Architecture.MachineType, CompilerOptions.EmitAllSymbols, CompilerOptions.EmitStaticRelocations, CompilerOptions.LinkerFormatType, CompilerOptions.CreateExtraSections, CompilerOptions.CreateExtraProgramHeaders);

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
	}
}
