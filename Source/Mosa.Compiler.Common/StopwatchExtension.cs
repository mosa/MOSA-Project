// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Common
{
	public static class StopwatchExtension
	{
		public static long ElapsedNanoSeconds(this Stopwatch watch)
		{
			return watch.ElapsedTicks * 1000000000 / Stopwatch.Frequency;
		}

		public static long ElapsedMicroSeconds(this Stopwatch watch)
		{
			return watch.ElapsedTicks * 1000000 / Stopwatch.Frequency;
		}
	}
}
