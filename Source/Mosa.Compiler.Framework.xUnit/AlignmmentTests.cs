// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Xunit;

namespace Mosa.Compiler.Framework.xUnit;

public class AlignmmentTests
{
	[Fact]
	public void AlignUp()
	{
		Assert.True(Alignment.AlignUp(3, 1) == 3);
		Assert.True(Alignment.AlignUp(3, 2) == 4);
		Assert.True(Alignment.AlignUp(4096 * 2, 4096) == 4096 * 2);
		Assert.True(Alignment.AlignUp(4096 + 1, 4096) == 4096 * 2);
	}

	[Fact]
	public void AlignDown()
	{
		Assert.True(Alignment.AlignDown(3, 1) == 3);
		Assert.True(Alignment.AlignDown(3, 2) == 2);
		Assert.True(Alignment.AlignDown(4096 * 2, 4096) == 4096 * 2);
		Assert.True(Alignment.AlignDown((4096 * 2) + 10, 4096) == 4096 * 2);
	}
}
