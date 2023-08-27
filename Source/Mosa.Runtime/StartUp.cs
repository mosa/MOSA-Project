// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Runtime;

public static class StartUp
{
	public static void Initialize()
	{
		BootOptions();
		PlatformInitialization();
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

	public static void InitializeAssembly()
	{
	}

	public static void InitializeRuntimeMetadata()
	{
	}

	public static void KernelEntryPoint()
	{
	}

	public static void StartApplication()
	{
	}
}
