// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;

namespace Mosa.Runtime.x86.Korlib.System.Threading
{
	public static class Interlocked
	{
		[Method("System.Threading.Exchange")]
		public static int Exchange(ref int location1, int value)
		{
			// TODO
			return 0;
		}
	}
}
