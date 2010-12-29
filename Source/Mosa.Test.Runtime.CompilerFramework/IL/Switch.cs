/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using MbUnit.Framework;

namespace Mosa.Test.Runtime.CompilerFramework.IL
{

	[TestFixture]
	public class Switch : TestCompilerAdapter
	{

		public Switch()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Row(1)]
		[Row(23)]
		[Row(-1)]
		[Row(0)]
		// And reverse
		[Row(2)]
		[Row(-2)]
		[Test]
		public void SwitchI1(sbyte a)
		{
			Assert.AreEqual(a, Run<sbyte>("Mosa.Test.Collection", "Switch", "SwitchI1", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(0)]
		// And reverse
		[Row(2)]
		[Test]
		public void SwitchU1(byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Switch", "SwitchU1", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(-1)]
		[Row(0)]
		// And reverse
		[Row(2)]
		[Row(-2)]
		// (MinValue, X) Cases
		[Row(short.MinValue)]
		// (MaxValue, X) Cases
		[Row(short.MaxValue)]
		[Test]
		public void SwitchI2(short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Switch", "SwitchI2", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(0)]
		// And reverse
		[Row(2)]
		// (MinValue, X) Cases
		[Row(ushort.MinValue)]
		// (MaxValue, X) Cases
		[Row(ushort.MaxValue)]
		[Test]
		public void SwitchU2(ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Switch", "SwitchU2", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(-1)]
		[Row(0)]
		// And reverse
		[Row(2)]
		[Row(-2)]
		// (MinValue, X) Cases
		[Row(int.MinValue)]
		// (MaxValue, X) Cases
		[Row(int.MaxValue)]
		[Test]
		public void SwitchI4(int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Switch", "SwitchI4", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(0)]
		// And reverse
		[Row(2)]
		// (MinValue, X) Cases
		[Row(uint.MinValue)]
		// (MaxValue, X) Cases
		[Row(uint.MaxValue)]
		[Test]
		public void SwitchU4(uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Switch", "SwitchU4", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(-1)]
		[Row(0)]
		// And reverse
		[Row(2)]
		[Row(-2)]
		// (MinValue, X) Cases
		[Row(long.MinValue)]
		// (MaxValue, X) Cases
		[Row(long.MaxValue)]
		[Test]
		public void SwitchI8(long a)
		{
			Assert.AreEqual(a, (long)Run<long>("Mosa.Test.Collection", "Switch", "SwitchI8", a, a));
		}

		[Row(1)]
		[Row(23)]
		[Row(0)]
		// And reverse
		[Row(2)]
		// (MinValue, X) Cases
		[Row(ulong.MinValue)]
		// (MaxValue, X) Cases
		[Row(ulong.MaxValue)]
		[Test]
		public void SwitchU8(ulong a)
		{

			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Switch", "SwitchU8", a, a));
		}
	}
}