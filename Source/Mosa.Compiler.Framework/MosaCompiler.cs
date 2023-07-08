// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.Configuration;

namespace Mosa.Compiler.Framework;

public sealed class MosaCompiler
{
	public MosaSettings MosaSettings { get; }

	public TypeSystem TypeSystem { get; private set; }

	public MosaTypeLayout TypeLayout { get; private set; }

	public BaseArchitecture Platform { get; private set; }

	public MosaLinker Linker => Compiler.Linker;

	public CompilerHooks CompilerHooks { get; }

	public IModuleLoader ModuleLoader { get; }

	public ITypeResolver TypeResolver { get; }

	private enum CompileStage
	{ Initial, Loaded, Initialized, Ready, Executing, Completed, Error }

	private CompileStage Stage = CompileStage.Initial;

	private Compiler Compiler;

	private readonly object _lock = new object();

	public bool IsSuccess => !Compiler.HasError && Stage != CompileStage.Error;

	public MosaCompiler(MosaSettings mosaSettings, CompilerHooks compilerHook, IModuleLoader moduleLoader, ITypeResolver typeResolver)
	{
		MosaSettings = new MosaSettings();
		MosaSettings.SetDetfaultSettings();
		MosaSettings.Merge(mosaSettings);

		CompilerHooks = compilerHook;
		ModuleLoader = moduleLoader;
		TypeResolver = typeResolver;
	}

	public void Load()
	{
		lock (_lock)
		{
			ModuleLoader.AddSearchPaths(MosaSettings.SearchPaths);
			ModuleLoader.LoadModuleFromFiles(MosaSettings.SourceFiles);

			var typeSystem = TypeSystem.Load(ModuleLoader.CreateMetadata(), TypeResolver);

			Load(typeSystem);
		}
	}

	public void Load(TypeSystem typeSystem)
	{
		lock (_lock)
		{
			TypeSystem = typeSystem;

			Platform = PlatformRegistry.GetPlatform(MosaSettings.Platform);
			TypeLayout = new MosaTypeLayout(typeSystem, Platform.Is32BitPlatform, Platform.NativePointerSize, Platform.NativeAlignment);

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

		if (!MosaSettings.MethodScanner)
		{
			ScheduleAll();
		}

		lock (_lock)
		{
			if (Stage != CompileStage.Ready)
				return;

			Stage = CompileStage.Executing;
		}

		var maxThreads = MosaSettings.Multithreading ? MosaSettings.MaxThreads > 0 ? MosaSettings.MaxThreads : (int)(Environment.ProcessorCount * 1.2) : 0;

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
