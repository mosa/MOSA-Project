// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Common
{
	public class WatchDog
	{
		private Stopwatch watch = new Stopwatch();

		public long TimeOutMilliseconds = 0;

		public bool IsTimedOut => watch.ElapsedMilliseconds > TimeOutMilliseconds;

		public WatchDog(long timeOutMilliseconds)
		{
			TimeOutMilliseconds = timeOutMilliseconds;
		}

		public void Restart()
		{
			watch.Restart();
		}
	}
}
