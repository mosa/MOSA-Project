// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.Runtime
{
	public static class StartUp
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Initialize()
		{
			Stage1();
			Stage2();
			Memory();
			Assembly();
			Kernel();
			Application();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Stage1()
		{
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Stage2()
		{
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Memory()
		{
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Assembly()
		{
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Kernel()
		{
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Application()
		{
		}
	}
}
