// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Runtime
{
	public static class StartUp
	{
		public static void Initialize()
		{
			SetInitialMemory();             // Legacy

			PlatformInitialization();
			GarbageCollectionInitialization();

			KernelInitialization();         // Legacy

			InitializeAssembly();
			InitializeRuntimeMetadata();

			KernelEntryPoint();

			StartApplication();
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
}
