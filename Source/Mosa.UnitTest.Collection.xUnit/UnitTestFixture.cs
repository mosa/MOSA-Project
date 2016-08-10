// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTest.Engine;
using System;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class UnitTestFixture : IDisposable
	{
		public static UnitTestEngine UnitTestEngine { get; private set; }

		private static int count = 0;

		private static object sync = new object();

		public UnitTestFixture()
		{
			lock (sync)
			{
				if (count == 0)
				{
					UnitTestEngine = new UnitTestEngine();
				}
				count++;
			}
		}

		void IDisposable.Dispose()
		{
			lock (sync)
			{
				count--;
				if (count == 0)
				{
					((IDisposable)UnitTestEngine).Dispose();
					UnitTestEngine = null;
				}
			}
		}
	}
}
