// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTest.System
{
	public class UnitTestSystemFixture : IDisposable
	{
		public static UnitTestEngine UnitTestSystem { get; } = new UnitTestEngine();

		public UnitTestSystemFixture()
		{
			return;
		}

		void IDisposable.Dispose()
		{
			((IDisposable)UnitTestSystem).Dispose();
		}
	}
}
