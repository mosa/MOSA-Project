// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Runtime;

public static class StartUp
{
	public static void Initialize()
	{
		BootOptions();

		SetInitialMemory();                 // Legacy

		PlatformInitialization();
		GarbageCollectionInitialization();  // Legacy

		KernelInitialization();             // Legacy

		InitializeAssembly();
		InitializeRuntimeMetadata();

		KernelEntryPoint();

		StartApplication();
	}

	public static void BootOptions()
	{
	}

	public static void PlatformInitialization()
	{
	}

	public static void GarbageCollectionInitialization()
	{
	}

	public static void KernelEntryPoint()
	{
	}

	public static void KernelInitialization()
	{
	}

	public static void SetInitialMemory()
	{
	}

	public static void InitializeAssembly()
	{
	}

	public static void InitializeRuntimeMetadata()
	{
	}

	public static void FinalKernelInitialization()
	{
	}

	public static void StartApplication()
	{
	}
}
