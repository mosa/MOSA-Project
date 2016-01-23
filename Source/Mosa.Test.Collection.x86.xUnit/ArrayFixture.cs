// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class ArrayFixture : X86TestFixture
	{
		[Fact]
		public void BoundsCheck()
		{
			Assert.Equal(Mosa.Test.Collection.ArrayTest.BoundsCheck(), Run<bool>("Mosa.Test.Collection.ArrayTest.BoundsCheck"));
		}
	}
}
