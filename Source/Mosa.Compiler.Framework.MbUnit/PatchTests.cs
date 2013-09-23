/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using MbUnit.Framework;
using Mosa.Compiler.Linker;

namespace Mosa.Compiler.Framework.MbUnit
{
	[TestFixture]
	public class PatchTests
	{
		[Test]
		public void Patch1()
		{
			var p = new Patch(0, 4, 0);
			Assert.AreEqual((ulong)0xF, p.Mask);
		}

		[Test]
		public void Patch2()
		{
			var p = new Patch(0, 8, 0);
			Assert.AreEqual((ulong)0xFF, p.Mask);
		}

		[Test]
		public void Patch3()
		{
			var p = new Patch(0, 8, 0);
			Assert.AreEqual((ulong)0xFF, p.Mask);
		}

		[Test]
		public void Patch4()
		{
			var p = new Patch(0, 16, 0);
			Assert.AreEqual((ulong)0xFFFF, p.Mask);
		}

		[Test]
		public void Patch5()
		{
			var p = new Patch(0, 32, 0);
			Assert.AreEqual((ulong)0xFFFFFFFF, p.Mask);
		}

		[Test]
		public void Patch6()
		{
			var p = new Patch(0, 64, 0);
			Assert.AreEqual((ulong)0xFFFFFFFFFFFFFFFF, p.Mask);
		}

		[Test]
		public void Patch7()
		{
			var p = new Patch(1, 4, 0);
			Assert.AreEqual((ulong)0x1E, p.Mask);
		}

		[Test]
		public void Patch8()
		{
			var p = new Patch(1, 8, 0);
			Assert.AreEqual((ulong)0x1FE, p.Mask);
		}

		[Test]
		public void Patch1B()
		{
			var p = new Patch(0, 4, 0);

			var r = p.GetResult(2);

			Assert.AreEqual((ulong)0x2, r);
		}

		[Test]
		public void Patch1C()
		{
			var p = new Patch(0, 4, 3);

			var r = p.GetResult(2);

			Assert.AreEqual((ulong)0x10, r);
		}
	}
}