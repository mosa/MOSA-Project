// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Linker;
using Xunit;

namespace Mosa.Compiler.Framework.xUnit
{
	public class PatchTests
	{
		[Fact]
		public void Patch1()
		{
			var p = new Patch(0, 4, 0);
			Assert.Equal((ulong)0xF, p.Mask);
		}

		[Fact]
		public void Patch2()
		{
			var p = new Patch(0, 8, 0);
			Assert.Equal((ulong)0xFF, p.Mask);
		}

		[Fact]
		public void Patch3()
		{
			var p = new Patch(0, 8, 0);
			Assert.Equal((ulong)0xFF, p.Mask);
		}

		[Fact]
		public void Patch4()
		{
			var p = new Patch(0, 16, 0);
			Assert.Equal((ulong)0xFFFF, p.Mask);
		}

		[Fact]
		public void Patch5()
		{
			var p = new Patch(0, 32, 0);
			Assert.Equal((ulong)0xFFFFFFFF, p.Mask);
		}

		[Fact]
		public void Patch6()
		{
			var p = new Patch(0, 64, 0);
			Assert.Equal((ulong)0xFFFFFFFFFFFFFFFF, p.Mask);
		}

		[Fact]
		public void Patch7()
		{
			var p = new Patch(1, 4, 0);
			Assert.Equal((ulong)0x1E, p.Mask);
		}

		[Fact]
		public void Patch8()
		{
			var p = new Patch(1, 8, 0);
			Assert.Equal((ulong)0x1FE, p.Mask);
		}

		[Fact]
		public void Patch1B()
		{
			var p = new Patch(0, 4, 0);

			var r = p.GetResult(2);

			Assert.Equal((ulong)0x2, r);
		}

		[Fact]
		public void Patch1C()
		{
			var p = new Patch(0, 4, 3);

			var r = p.GetResult(2);

			Assert.Equal((ulong)0x10, r);
		}
	}
}
