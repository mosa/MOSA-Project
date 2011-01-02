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
using Mosa.Test.Runtime.CompilerFramework.Numbers;

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
		
		[Test, Factory(typeof(U1), "Samples")]
		public void LdargaCheckValueU1(byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU1", a, a));
		}
		
		[Test, Factory(typeof(U2), "Samples")]
		public void LdargaCheckValueU2(ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU2", a, a));
		}
		
		[Test, Factory(typeof(U4), "Samples")]
		public void LdargaCheckValueU4(uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU4", a, a));
		}
		
		[Test, Factory(typeof(U8), "Samples")]
		public void LdargaCheckValueU8(ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU8", a, a));
		}
		
		[Test, Factory(typeof(I1), "Samples")]
		public void LdargaCheckValueI1(sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI1", a, a));
		}
		
		[Test, Factory(typeof(I2), "Samples")]
		public void LdargaCheckValueI2(short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI2", a, a));
		}
		
		[Test, Factory(typeof(I4), "Samples")]
		public void LdargaCheckValueI4(int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI4", a, a));
		}
		
		[Test, Factory(typeof(I8), "Samples")]
		public void LdargaCheckValueI8(long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI8", a, a));
		}
		
		[Test, Factory(typeof(R4), "Samples")]
		public void LdargaCheckValueR4(float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueR4", a, a));
		}
		
		[Test, Factory(typeof(R8), "Samples")]
		public void LdargaCheckValueR8(double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueR8", a, a));
		}
		
		[Test, Factory(typeof(C), "Samples")]
		public void LdargaCheckValueC(char a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueC", a, a));
		}
		
		#endregion

		#region ChangeValue
		
		[Test, Factory(typeof(U1), "Samples")]
		public void LdargaChangeValueU1(byte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU1", a, a));
		}
		
		[Test, Factory(typeof(U2), "Samples")]
		public void LdargaChangeValueU2(ushort a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU2", a, a));
		}
		
		[Test, Factory(typeof(U4), "Samples")]
		public void LdargaChangeValueU4(uint a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU4", a, a));
		}
		
		[Test, Factory(typeof(U8), "Samples")]
		public void LdargaChangeValueU8(ulong a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU8", a, a));
		}
		
		[Test, Factory(typeof(I1), "Samples")]
		public void LdargaChangeValueI1(sbyte a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI1", a, a));
		}
		
		[Test, Factory(typeof(I2), "Samples")]
		public void LdargaChangeValueI2(short a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI2", a, a));
		}
		
		[Test, Factory(typeof(I4), "Samples")]
		public void LdargaChangeValueI4(int a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI4", a, a));
		}
		
		[Test, Factory(typeof(I8), "Samples")]
		public void LdargaChangeValueI8(long a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI8", a, a));
		}
		
		[Test, Factory(typeof(R4), "Samples")]
		public void LdargaChangeValueR4(float a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueR4", a, a));
		}
		
		[Test, Factory(typeof(R8), "Samples")]
		public void LdargaChangeValueR8(double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueR8", a, a));
		}
		
		#endregion
	}
}
