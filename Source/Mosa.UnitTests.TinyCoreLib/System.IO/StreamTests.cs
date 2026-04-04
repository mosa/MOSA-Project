// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.UnitTests.TinyCoreLib;

public static class StreamTests
{
	// == CopyTo tests

	[MosaUnitTest(Series = "U1U1")]
	public static bool Test_Stream_CopyTo_MemoryToMemory(byte first, byte last)
	{
		byte[] data = { first, 2, 3, 4, last };
		var source = new MemoryStream(data);
		var dest = new MemoryStream();
		source.CopyTo(dest);
		var result = dest.ToArray();
		return dest.Length == 5L && result[0] == first && result[4] == last;
	}

	[MosaUnitTest(Series = "U1U1")]
	public static bool Test_Stream_CopyTo_WithData(byte first, byte last)
	{
		byte[] data = { first, 20, 30, 40, 50, last };
		var source = new MemoryStream(data);
		var dest = new MemoryStream();
		source.CopyTo(dest);
		var result = dest.ToArray();
		return result.Length == 6 && result[0] == first && result[5] == last;
	}

	[MosaUnitTest]
	public static long Test_Stream_CopyTo_LargeData()
	{
		var data = new byte[10000];
		for (int i = 0; i < data.Length; i++)
			data[i] = (byte)(i % 256);
		var source = new MemoryStream(data);
		var dest = new MemoryStream();
		source.CopyTo(dest);
		return dest.Length;
	}

	[MosaUnitTest]
	public static long Test_Stream_CopyTo_EmptyStream()
	{
		var source = new MemoryStream();
		var dest = new MemoryStream();
		source.CopyTo(dest);
		return dest.Length;
	}
}
