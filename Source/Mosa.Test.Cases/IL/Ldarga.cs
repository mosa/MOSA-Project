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
	public class Ldarga : TestCompilerAdapter
	{
		public Ldarga()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		#region CheckValue

		[Column(0, 1, sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void LdargaCheckValueI1(sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaCheckValueI1", a, a));
		}

		[Column(0, 1, byte.MinValue, byte.MaxValue)]
		[Test]
		public void LdargaCheckValueU1(byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaCheckValueU1", a, a));
		}

		[Column(0, 1, short.MinValue, short.MaxValue)]
		[Test]
		public void LdargaCheckValueI1(short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaCheckValueI1", a, a));
		}

		[Column(0, 1, ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void LdargaCheckValueU2(ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaCheckValueU2", a, a));
		}

		[Column(0, 1, int.MinValue, int.MaxValue)]
		[Test]
		public void LdargaCheckValueI4(int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaCheckValueI4", a, a));
		}

		[Column(0, 1, uint.MinValue, uint.MaxValue)]
		[Test]
		public void LdargaCheckValueU4(uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaCheckValueU4", a, a));
		}

		[Column(0, 1, long.MinValue, long.MaxValue)]
		[Test]
		public void LdargaCheckValueI8(long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaCheckValueI8", a, a));
		}

		[Column(0, 1, ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void LdargaCheckValueU8(ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaCheckValueU8", a, a));
		}

		[Column(0, 1, float.MinValue, float.MaxValue)]
		[Test]
		public void LdargaCheckValueR4(float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaCheckValueR4", a, a));
		}

		[Column(0, 1, double.MinValue, double.MaxValue)]
		[Test]
		public void LdargaCheckValueR8(double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaCheckValueR8", a, a));
		}

		#endregion

		#region ChangeValue

		[Column(0, 1, sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void LdargaChangeValueI1(sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaChangeValueI1", a, a));
		}

		[Column(0, 1, byte.MinValue, byte.MaxValue)]
		[Test]
		public void LdargaChangeValueU1(byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaChangeValueU1", a, a));
		}

		[Column(0, 1, short.MinValue, short.MaxValue)]
		[Test]
		public void LdargaChangeValueI1(short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaChangeValueI1", a, a));
		}

		[Column(0, 1, ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void LdargaChangeValueU2(ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaChangeValueU2", a, a));
		}

		[Column(0, 1, int.MinValue, int.MaxValue)]
		[Test]
		public void LdargaChangeValueI4(int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaChangeValueI4", a, a));
		}

		[Column(0, 1, uint.MinValue, uint.MaxValue)]
		[Test]
		public void LdargaChangeValueU4(uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaChangeValueU4", a, a));
		}

		[Column(0, 1, long.MinValue, long.MaxValue)]
		[Test]
		public void LdargaChangeValueI8(long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaChangeValueI8", a, a));
		}

		[Column(0, 1, ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void LdargaChangeValueU8(ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaChangeValueU8", a, a));
		}

		[Column(0, 1, float.MinValue, float.MaxValue)]
		[Test]
		public void LdargaChangeValueR4(float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaChangeValueR4", a, a));
		}

		[Column(0, 1, double.MinValue, double.MaxValue)]
		[Test]
		public void LdargaChangeValueR8(double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Ldarga", "LdargaChangeValueR8", a, a));
		}

		#endregion
	}
}
