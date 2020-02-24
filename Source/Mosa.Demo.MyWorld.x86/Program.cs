// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;

namespace Mosa.Demo.MyWorld.x86
{
	public static class Program
	{
		public static void Setup()
		{
			Screen.Write("Hello World!");
		}

		public static void Loop()
		{ }

		public static void OnInterrupt()
		{ }
	}
}
