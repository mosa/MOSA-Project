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
		public enum CompileStage { Initial, Loaded, Initialized, Ready, Executing, Completed }

		public CompileStage Stage { get; private set; } = CompileStage.Initial;

		public CompilerOptions CompilerOptions { get; }

		public CompilerTrace CompilerTrace { get; }

		public TypeSystem TypeSystem { get; private set; }

		public MosaTypeLayout TypeLayout { get; private set; }

		public CompilationScheduler CompilationScheduler { get; private set; }

		public MosaLinker Linker { get; private set; }

		public List<BaseCompilerExtension> CompilerExtensions { get; } = new List<BaseCompilerExtension>();

		public int MaxThreads { get; }

		protected Compiler Compiler { get; private set; }

		private object _lock = new object();

		public MosaCompiler(List<BaseCompilerExtension> compilerExtensions = null, int maxThreads = 0)
			: this(null, compilerExtensions, maxThreads)
		{
		}

		public MosaCompiler(CompilerOptions compilerOptions = null, List<BaseCompilerExtension> compilerExtensions = null, int maxThreads = 0)
		{
			MaxThreads = (maxThreads == 0) ? Environment.ProcessorCount + 1 : maxThreads;

			CompilerOptions = compilerOptions ?? new CompilerOptions();
			CompilerTrace = new CompilerTrace(CompilerOptions);

			if (compilerExtensions != null)
			{
				CompilerExtensions.AddRange(compilerExtensions);
			}
		}

		public void Load()
		{
			lock (_lock)
			{
				if (Stage != CompileStage.Initial)
					return;

				var moduleLoader = new MosaModuleLoader();

				moduleLoader.AddSearchPaths(CompilerOptions.SearchPaths);
				moduleLoader.LoadModuleFromFiles(CompilerOptions.SourceFiles);

				var typeSystem = TypeSystem.Load(moduleLoader.CreateMetadata());

				Load(typeSystem);

				Stage = CompileStage.Loaded;
			}
		}

		public void Load(TypeSystem typeSystem)
		{
			lock (_lock)
			{
				if (Stage != CompileStage.Initial)
					return;

				TypeSystem = typeSystem;

				TypeLayout = new MosaTypeLayout(typeSystem, CompilerOptions.Architecture.NativePointerSize, CompilerOptions.Architecture.NativeAlignment);

				CompilationScheduler = new CompilationScheduler();
			}
		}

		public void Initialize()
		{
			lock (_lock)
			{
				if (Stage != CompileStage.Loaded)
					return;

				Linker = new MosaLinker(CompilerOptions.BaseAddress, CompilerOptions.Architecture.Endianness, CompilerOptions.Architecture.ElfMachineType, CompilerOptions.EmitAllSymbols, CompilerOptions.EmitStaticRelocations, CompilerOptions.LinkerFormatType, CompilerOptions.CreateExtraSections, CompilerOptions.CreateExtraProgramHeaders);
				Compiler = new Compiler(this);

				Stage = CompileStage.Initialized;
			}
		}

		public void Setup()
		{
			lock (_lock)
			{
				if (Stage != CompileStage.Initialized)
					return;

				Compiler.PreCompile();

				Stage = CompileStage.Ready;
			}
		}

		public void PostCompile()
		{
			lock (_lock)
			{
				if (Stage != CompileStage.Ready)
					return;

				Compiler.PostCompile();

				Stage = CompileStage.Completed;
			}
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

		public void Compile(bool skipFinalization = false)
		{
			Initialize();
			Setup();

			if (!CompilerOptions.EnableMethodScanner)
			{
				ScheduleAll();
			}

			lock (_lock)
			{
				if (Stage != CompileStage.Ready)
					return;

				Stage = CompileStage.Executing;
			}

			Compiler.ExecuteCompile();

			lock (_lock)
			{
				Stage = CompileStage.Ready;
			}

			if (!skipFinalization)
			{
				PostCompile();
			}
		}

		public void ThreadedCompile(bool skipFinalization = false)
		{
			Initialize();
			Setup();

			if (!CompilerOptions.EnableMethodScanner)
			{
				ScheduleAll();
			}

			lock (_lock)
			{
				if (Stage != CompileStage.Ready)
					return;

				Stage = CompileStage.Executing;
			}

			Compiler.ExecuteThreadedCompile(MaxThreads);

			lock (_lock)
			{
				Stage = CompileStage.Ready;
			}

			if (!skipFinalization)
			{
				PostCompile();
			}
		}

		public void CompileSingleMethod(MosaMethod method)
		{
			Initialize();
			Setup();

			// Thread Safe
			Compiler.CompileMethod(method);
		}
	}
}
