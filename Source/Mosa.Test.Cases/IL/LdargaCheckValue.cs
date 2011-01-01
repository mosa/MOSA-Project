/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.IL
{
	[TestFixture]
	public class LdargaCheckValue : TestCompilerAdapter
	{
		public LdargaCheckValue()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		#region CheckValue

		[Column(0, 1, sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void LdargaI1_CheckValue(sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaCheckValue", "LdargaCheckValueI1", a, a));
		}

		[Column(0, 1, byte.MinValue, byte.MaxValue)]
		[Test]
		public void LdargaCheckValueU1(byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaCheckValue", "LdargaCheckValueU1", a, a));
		}

		[Column(0, 1, short.MinValue, short.MaxValue)]
		[Test]
		public void LdargaI2_CheckValue(short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaCheckValue", "LdargaCheckValueI1", a, a));
		}

		[Column(0, 1, ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void LdargaU2_CheckValue(ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaCheckValue", "LdargaCheckValueU2", a, a));
		}

		[Column(0, 1, int.MinValue, int.MaxValue)]
		[Test]
		public void LdargaI4_CheckValue(int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaCheckValue", "LdargaCheckValueI4", a, a));
		}

		[Column(0, 1, uint.MinValue, uint.MaxValue)]
		[Test]
		public void LdargaU4_CheckValue(uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaCheckValue", "LdargaCheckValueU4", a, a));
		}

		[Column(0, 1, long.MinValue, long.MaxValue)]
		[Test]
		public void LdargaI8_CheckValue(long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaCheckValue", "LdargaCheckValueI8", a, a));
		}

		[Column(0, 1, ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void LdargaU8_CheckValue(ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaCheckValue", "LdargaCheckValueU8", a, a));
		}

		[Column(0, 1, float.MinValue, float.MaxValue)]
		[Test]
		public void LdargaR4_CheckValue(float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaCheckValue", "LdargaCheckValueR4", a, a));
		}

		[Column(0, 1, double.MinValue, double.MaxValue)]
		[Test]
		public void LdargaR8_CheckValue(double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaCheckValue", "LdargaCheckValueR8", a, a));
		}

		#endregion

	}
}
