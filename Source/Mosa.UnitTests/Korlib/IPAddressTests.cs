// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.UnitTests.Korlib;

public class IPAddressTests
{
	[MosaUnitTest]
	public static bool TryParse()
	{
		var success = IPAddress.TryParse("127.0.0.1", out var address);

		return success && address is {A: 127, B: 0, C: 0, D: 1};
	}
}
