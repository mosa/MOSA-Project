// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.InteropServices;

namespace Mosa.UnitTests.TinyCoreLib;

public static class MarshalTests
{
	[MosaUnitTest]
	public static bool Test_Marshal_GetFunctionPointerForDelegate_Generic()
	{
		TestDelegate del = () => 42;
		var ptr = Marshal.GetFunctionPointerForDelegate(del);
		return ptr != nint.Zero;
	}

	[MosaUnitTest]
	public static bool Test_Marshal_GetFunctionPointerForDelegate_NonGeneric()
	{
		TestDelegate del = () => 42;
		var ptr = Marshal.GetFunctionPointerForDelegate((Delegate)del);
		return ptr != nint.Zero;
	}

	private delegate int TestDelegate();
}
