// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Runtime
{
	public static class StartUp
	{
		public static void Initialize()
		{
			Stage1();
			Stage2();
			InitalizeProcessor1();
			InitalizeProcessor2();
			SetInitialMemory();
			InitializeAssembly();
			InitializeRuntimeMetadata();
			StartApplication();
		}

		public static void Stage1()
		{
		}

		public static void Stage2()
		{
		}

		public static void InitalizeProcessor1()
		{
		}

		public static void InitalizeProcessor2()
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
