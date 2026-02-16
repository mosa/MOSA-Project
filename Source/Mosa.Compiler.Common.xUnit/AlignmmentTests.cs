// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Compiler.Common.xUnit;

public class AlignmmentTests
{
	[Fact]
	public void AlignUp()
	{
		Assert.Equal(Alignment.AlignUp(3, 1), 3);
		Assert.Equal(Alignment.AlignUp(3, 2), 4);
		Assert.Equal(Alignment.AlignUp(4096 * 2, 4096), 4096 * 2);
		Assert.Equal(Alignment.AlignUp(4096 + 1, 4096), 4096 * 2);
	}

	[Fact]
	public void AlignDown()
	{
		Assert.Equal(Alignment.AlignDown(3, 1), 3);
		Assert.Equal(Alignment.AlignDown(3, 2), 2);
		Assert.Equal(Alignment.AlignDown(4096 * 2, 4096), 4096 * 2);
		Assert.Equal(Alignment.AlignDown((4096 * 2) + 10, 4096), 4096 * 2);
	}
}
