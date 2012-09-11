/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using MbUnit.Framework;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;

namespace Mosa.Test.Compiler.Framework
{

	[TestFixture]
	public class PatchTests
	{

		[Test]
		public void Patch1()
		{
			Patch p = new Patch(0, 4, 0);
			Assert.AreEqual((ulong)0xF, p.Mask);
		}

		[Test]
		public void Patch2()
		{
			Patch p = new Patch(0, 8, 0);
			Assert.AreEqual((ulong)0xFF, p.Mask);
		}

		[Test]
		public void Patch3()
		{
			Patch p = new Patch(0, 8, 0);
			Assert.AreEqual((ulong)0xFF, p.Mask);
		}

		[Test]
		public void Patch4()
		{
			Patch p = new Patch(0, 16, 0);
			Assert.AreEqual((ulong)0xFFFF, p.Mask);
		}

		[Test]
		public void Patch5()
		{
			Patch p = new Patch(0, 32, 0);
			Assert.AreEqual((ulong)0xFFFFFFFF, p.Mask);
		}

		[Test]
		public void Patch6()
		{
			Patch p = new Patch(0, 64, 0);
			Assert.AreEqual((ulong)0xFFFFFFFFFFFFFFFF, p.Mask);
		}

		[Test]
		public void Patch7()
		{
			Patch p = new Patch(1, 4, 0);
			Assert.AreEqual((ulong)0x1E, p.Mask);
		}

		[Test]
		public void Patch8()
		{
			Patch p = new Patch(1, 8, 0);
			Assert.AreEqual((ulong)0x1FE, p.Mask);
		}
	
	}
}
