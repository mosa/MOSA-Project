// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common;
using Xunit;

namespace Mosa.Compiler.Framework.xUnit;

public class IntegerTwiddlingTests
{
	[Fact]
	public void IsAddSignedOverflow()
	{
		Debug.Assert(IntegerTwiddling.IsAddSignedOverflow(2147483640, 10));
		Debug.Assert(!IntegerTwiddling.IsAddSignedOverflow(0, 0));
	}

	[Fact]
	public void IsAddUnsignedCarry()
	{
		Debug.Assert(IntegerTwiddling.IsAddUnsignedCarry(ulong.MaxValue, 1ul));
		Debug.Assert(!IntegerTwiddling.IsAddUnsignedCarry(ulong.MaxValue, 0ul));
		Debug.Assert(!IntegerTwiddling.IsAddUnsignedCarry(0ul, 0ul));
		Debug.Assert(!IntegerTwiddling.IsAddUnsignedCarry(0ul, ulong.MaxValue));
	}
}
