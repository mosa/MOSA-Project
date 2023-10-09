// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Compiler.Common.xUnit;

public class SparseBitArrayTests
{
	[Fact]
	public void AllZeroAfterCreation()
	{
		var bitarray = new SparseBitArray();

		Assert.True(!bitarray.Get(0));
		Assert.True(!bitarray.Get(1));
		Assert.True(!bitarray.Get(2));
		Assert.True(!bitarray.Get(100));
		Assert.True(!bitarray.Get(1000));
	}

	[Fact]
	public void SimpleSets()
	{
		var bitarray = new SparseBitArray();
		bitarray.Set(0, true);
		bitarray.Set(1, true);
		bitarray.Set(2, true);
		bitarray.Set(3, true);

		Assert.True(bitarray.Get(0));
		Assert.True(bitarray.Get(1));
		Assert.True(bitarray.Get(2));
		Assert.True(bitarray.Get(3));
		Assert.True(!bitarray.Get(4));
	}
}
