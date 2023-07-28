// Copyright (c) MOSA Project. Licensed under the New BSD License.
using System;

namespace Mosa.UnitTests.Regression;

public static class Issue1093
{
	[MosaUnitTest]
	public static bool TestIssue1093()
	{
		return true;
	}

	public class Attr : Attribute
	{
		public Attr(byte[] flags) { }
	}

	[Attr(new byte[] { 1, 2, 3 })]
	public static int FooBar;
}
