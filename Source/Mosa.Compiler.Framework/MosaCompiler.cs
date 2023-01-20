// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework
{
	public sealed class MosaCompiler
	{
		public CompilerSettings CompilerSettings { get; }

		public TypeSystem TypeSystem { get; private set; }

		public MosaTypeLayout TypeLayout { get; private set; }

		public BaseArchitecture Platform { get; private set; }

		public MosaLinker Linker { get { return Compiler.Linker; } }

		public CompilerHooks CompilerHooks { get; }

		public IModuleLoader ModuleLoader { get; }

		public ITypeResolver TypeResolver { get; }

		private enum CompileStage
		{ Initial, Loaded, Initialized, Ready, Executing, Completed }

		private CompileStage Stage = CompileStage.Initial;

		private Compiler Compiler;

		private readonly object _lock = new object();

		public MosaCompiler(Settings settings, CompilerHooks compilerHook, IModuleLoader moduleLoader, ITypeResolver typeResolver)
		{
			CompilerSettings = new CompilerSettings(settings.Clone());
			CompilerHooks = compilerHook;
			ModuleLoader = moduleLoader;
			TypeResolver = typeResolver;
		}

		public void Load()
		{
			lock (_lock)
			{
				ModuleLoader.AddSearchPaths(CompilerSettings.SearchPaths);
				ModuleLoader.LoadModuleFromFiles(CompilerSettings.SourceFiles);

				var typeSystem = TypeSystem.Load(ModuleLoader.CreateMetadata(), TypeResolver);

				Load(typeSystem);
			}
		}

		public void Load(TypeSystem typeSystem)
		{
			lock (_lock)
			{
				TypeSystem = typeSystem;

				Platform = PlatformRegistry.GetPlatform(CompilerSettings.Platform);
				TypeLayout = new MosaTypeLayout(typeSystem, Platform.NativePointerSize, Platform.NativeAlignment);

				Compiler = null;

				Stage = CompileStage.Loaded;
			}
		}

		public void Initialize()
		{
			lock (_lock)
			{
				if (Stage != CompileStage.Loaded)
					return;

				Compiler = new Compiler(this);

				Stage = CompileStage.Initialized;
			}
		}

		public void Setup()
		{
			Initialize();

			lock (_lock)
			{
				if (Stage != CompileStage.Initialized)
					return;

				Compiler.Setup();

				Stage = CompileStage.Ready;
			}
		}

		public void Finalization()
		{
			lock (_lock)
			{
				if (Stage != CompileStage.Ready)
					return;

				Compiler.Finalization();

				Stage = CompileStage.Completed;
			}
		}

		public void ScheduleAll()
		{
			Setup();
			Compiler.MethodScheduler.ScheduleAll(TypeSystem);
		}

		public void Schedule(MosaType type)
		{
			Setup();
			Compiler.MethodScheduler.Schedule(type);
		}

		public void Schedule(MosaMethod method)
		{
			Setup();
			Compiler.MethodScheduler.Schedule(method);
		}

		public void Compile(bool skipFinalization = false)
		{
			Setup();

			if (!CompilerSettings.MethodScanner)
			{
				ScheduleAll();
			}

			lock (_lock)
			{
				if (Stage != CompileStage.Ready)
					return;

				Stage = CompileStage.Executing;
			}

			var maxThreads = CompilerSettings.MaxThreads != 0 ? CompilerSettings.MaxThreads : (int)(Environment.ProcessorCount * 1.2);

			Compiler.ExecuteCompile(maxThreads);

			lock (_lock)
			{
				Stage = CompileStage.Ready;
			}

			if (!skipFinalization)
			{
				Finalization();
			}
		}

		public void CompileSingleMethod(MosaMethod method)
		{
			Setup();

			// Thread Safe
			Compiler.CompileMethod(method);
		}
	}
}
