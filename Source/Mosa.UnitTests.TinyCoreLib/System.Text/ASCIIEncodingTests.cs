// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.UnitTests.TinyCoreLib;

public static class ASCIIEncodingTests
{
	// == GetString tests

	[MosaUnitTest(Series = "U1U1U1")]
	public static string Test_ASCIIEncoding_GetString_ValidBytes(byte b0, byte b1, byte b2)
	{
		return Encoding.ASCII.GetString([b0, b1, b2]);
	}

	[MosaUnitTest]
	public static bool Test_ASCIIEncoding_GetString_EmptyArray()
	{
		var encoding = Encoding.ASCII;
		byte[] bytes = [];
		return encoding.GetString(bytes) == "";
	}

	[MosaUnitTest]
	public static bool Test_ASCIIEncoding_GetString_HelloWorld()
	{
		var encoding = Encoding.ASCII;
		byte[] bytes = { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 }; // Hello World
		return encoding.GetString(bytes) == "Hello World";
	}

	[MosaUnitTest]
	public static bool Test_ASCIIEncoding_GetString_Numbers()
	{
		var encoding = Encoding.ASCII;
		byte[] bytes = { 48, 49, 50, 51, 52 }; // 01234
		return encoding.GetString(bytes) == "01234";
	}

	[MosaUnitTest("I4UpTo8")]
	public static string Test_ASCIIEncoding_GetString_WithRange(int count)
	{
		var encoding = Encoding.ASCII;
		byte[] bytes = { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 }; // Hello World
		return encoding.GetString(bytes, 0, count);
	}

	// == GetBytes tests

	[MosaUnitTest]
	public static bool Test_ASCIIEncoding_GetBytes_ValidString()
	{
		var encoding = Encoding.ASCII;
		var result = encoding.GetBytes("ABC");
		return result.Length == 3 && result[0] == 65 && result[1] == 66 && result[2] == 67;
	}

	[MosaUnitTest]
	public static int Test_ASCIIEncoding_GetBytes_EmptyString()
	{
		var encoding = Encoding.ASCII;
		return encoding.GetBytes("").Length;
	}

	// == Round-trip tests

	[MosaUnitTest]
	public static bool Test_ASCIIEncoding_RoundTrip_Basic()
	{
		var encoding = Encoding.ASCII;
		var original = "Hello World 123";
		var bytes = encoding.GetBytes(original);
		return encoding.GetString(bytes) == original;
	}
}
