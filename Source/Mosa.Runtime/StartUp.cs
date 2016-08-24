// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Runtime
{
	public static class StartUp
	{
		public static void Start()
		{
			PostBoot();
			PostBootStage2();
			Kernel();
			Application();
		}

		public static void PostBoot()
		{
		}

		public static void PostBootStage2()
		{
		}

		public static void Kernel()
		{
		}

		public static void Application()
		{
		}
	}
}
