// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common;
using Xunit;

namespace Mosa.Compiler.Framework.xUnit;

public class IntegerTwiddlingTests
{
	[Fact]
	public void SignedIntegerOverflow()
	{
		Debug.Assert(IntegerTwiddling.IsAddOverflow(2147483640, 10));
		Debug.Assert(!IntegerTwiddling.IsAddOverflow(0, 0));
	}

	[Fact]
	public void UnsignedIntegerOverflow()
	{
		Debug.Assert(IntegerTwiddling.IsAddOverflow(ulong.MaxValue, 1ul));
		Debug.Assert(!IntegerTwiddling.IsAddOverflow(ulong.MaxValue, 0ul));
		Debug.Assert(!IntegerTwiddling.IsAddOverflow(0ul, 0ul));
		Debug.Assert(!IntegerTwiddling.IsAddOverflow(0ul, ulong.MaxValue));
	}
}
