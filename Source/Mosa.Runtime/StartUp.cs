// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Runtime
{
	public static class StartUp
	{
		public static void Initialize()
		{
			KernalInitialization();
			SetInitialMemory();
			InitializeAssembly();
			InitializeRuntimeMetadata();
			StartApplication();
		}

		public static void KernalInitialization()
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

		public static void StartApplication()
		{
		}
	}
}
