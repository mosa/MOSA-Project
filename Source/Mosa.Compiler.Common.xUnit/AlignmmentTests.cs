// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Compiler.Common.xUnit;

public class AlignmmentTests
{
	[Fact]
	public void AlignUp()
	{
		Assert.Equal(3, Alignment.AlignUp(3, 1));
		Assert.Equal(4, Alignment.AlignUp(3, 2));
		Assert.Equal(4096 * 2, Alignment.AlignUp(4096 * 2, 4096));
		Assert.Equal(4096 * 2, Alignment.AlignUp(4096 + 1, 4096));
	}

	[Fact]
	public void AlignDown()
	{
		Assert.Equal(3, Alignment.AlignDown(3, 1));
		Assert.Equal(2, Alignment.AlignDown(3, 2));
		Assert.Equal(4096 * 2, Alignment.AlignDown(4096 * 2, 4096));
		Assert.Equal(4096 * 2, Alignment.AlignDown((4096 * 2) + 10, 4096));
	}
}
