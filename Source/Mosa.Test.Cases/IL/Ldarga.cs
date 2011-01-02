/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
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
		
		[Column(0, 1, 2, byte.MinValue, byte.MaxValue)]
		[Test]
		public void LdargaCheckValueU1(byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU1", a, a));
		}
		
		[Column(0, 1, 2, ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void LdargaCheckValueU2(ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU2", a, a));
		}
		
		[Column(0, 1, 2, uint.MinValue, uint.MaxValue)]
		[Test]
		public void LdargaCheckValueU4(uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU4", a, a));
		}
		
		[Column(0, 1, 2, ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void LdargaCheckValueU8(ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU8", a, a));
		}
		
		[Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void LdargaCheckValueI1(sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI1", a, a));
		}
		
		[Column(0, 1, 2, short.MinValue, short.MaxValue)]
		[Test]
		public void LdargaCheckValueI2(short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI2", a, a));
		}
		
		[Column(0, 1, 2, int.MinValue, int.MaxValue)]
		[Test]
		public void LdargaCheckValueI4(int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI4", a, a));
		}
		
		[Column(0, 1, 2, long.MinValue, long.MaxValue)]
		[Test]
		public void LdargaCheckValueI8(long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI8", a, a));
		}
		
		[Column(0, 1, 2, float.MinValue, float.MaxValue)]
		[Test]
		public void LdargaCheckValueR4(float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueR4", a, a));
		}
		
		[Column(0, 1, 2, double.MinValue, double.MaxValue)]
		[Test]
		public void LdargaCheckValueR8(double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueR8", a, a));
		}
		
		[Column(0, 1, 2, char.MinValue, char.MaxValue)]
		[Test]
		public void LdargaCheckValueC(char a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueC", a, a));
		}
		
		#endregion

		#region ChangeValue
		
		[Column(0, 1, 2, byte.MinValue, byte.MaxValue)]
		[Test]
		public void LdargaChangeValueU1(byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU1", a, a));
		}
		
		[Column(0, 1, 2, ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void LdargaChangeValueU2(ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU2", a, a));
		}
		
		[Column(0, 1, 2, uint.MinValue, uint.MaxValue)]
		[Test]
		public void LdargaChangeValueU4(uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU4", a, a));
		}
		
		[Column(0, 1, 2, ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void LdargaChangeValueU8(ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU8", a, a));
		}
		
		[Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void LdargaChangeValueI1(sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI1", a, a));
		}
		
		[Column(0, 1, 2, short.MinValue, short.MaxValue)]
		[Test]
		public void LdargaChangeValueI2(short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI2", a, a));
		}
		
		[Column(0, 1, 2, int.MinValue, int.MaxValue)]
		[Test]
		public void LdargaChangeValueI4(int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI4", a, a));
		}
		
		[Column(0, 1, 2, long.MinValue, long.MaxValue)]
		[Test]
		public void LdargaChangeValueI8(long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI8", a, a));
		}
		
		[Column(0, 1, 2, float.MinValue, float.MaxValue)]
		[Test]
		public void LdargaChangeValueR4(float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueR4", a, a));
		}
		
		[Column(0, 1, 2, double.MinValue, double.MaxValue)]
		[Test]
		public void LdargaChangeValueR8(double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueR8", a, a));
		}
		
		[Column(0, 1, 2, char.MinValue, char.MaxValue)]
		[Test]
		public void LdargaChangeValueC(char a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueC", a, a));
		}
		
		#endregion
	}
}
