// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTest.Engine
{
	public class UnitTestFixture : IDisposable
	{
		public static UnitTestEngine UnitTestEngine { get; } = new UnitTestEngine();

		public UnitTestFixture()
		{
			return;
		}

		void IDisposable.Dispose()
		{
			((IDisposable)UnitTestEngine).Dispose();
		}
	}
}
