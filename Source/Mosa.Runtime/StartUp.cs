// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Runtime;

public static class StartUp
{
	public static void Initialize(Pointer stackFrame)
	{
		BootOptions();
		InitializePlatform(stackFrame);
		InitializeAssembly();
		InitializeRuntimeMetadata();
		KernelEntryPoint();
		StartApplication();
	}

	public static void BootOptions()
	{ }

	public static void InitializePlatform(Pointer stackFrame)
	{ }

	public static void InitializeAssembly()
	{ }

	public static void InitializeRuntimeMetadata()
	{ }

	public static void KernelEntryPoint()
	{ }

	public static void StartApplication()
	{ }
}
