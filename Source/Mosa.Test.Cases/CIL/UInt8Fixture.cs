/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */

using System;
using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework;
using Mosa.Test.Runtime.CompilerFramework.Numbers;
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	public class UInt8Fixture : TestCompilerAdapter
	{

		public UInt8Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void AddU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.AddU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "AddU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void SubU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.SubU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "SubU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void MulU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.MulU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "MulU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1WithoutZero")]
		public void DivU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.DivU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "DivU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU1U1DivideByZeroException(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.DivU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "DivU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1WithoutZero")]
		public void RemU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.RemU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "RemU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU1U1DivideByZeroException(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.RemU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "RemU1U1", a, b));
		}

		[Test, Factory(typeof(U1), "Samples")]
		public void RetU1(byte a)
		{
			Assert.AreEqual(UInt8Tests.RetU1(a), Run<byte>("Mosa.Test.Collection", "UInt8Tests", "RetU1", a));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void AndU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.AndU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "AndU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void OrU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.OrU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "OrU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void XorU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.XorU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "XorU1U1", a, b));
		}

		[Test, Factory(typeof(U1), "Samples")]
		public void CompU1(byte a)
		{
			Assert.AreEqual(UInt8Tests.CompU1(a), Run<int>("Mosa.Test.Collection", "UInt8Tests", "CompU1", a));
		}

		[Test, Factory(typeof(Variations), "U1_U1UpTo8")]
		public void ShiftLeftU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.ShiftLeftU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "ShiftLeftU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1UpTo8")]
		public void ShiftRightU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.ShiftRightU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "ShiftRightU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void CeqU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.CeqU1U1(a, b), Run<bool>("Mosa.Test.Collection", "UInt8Tests", "CeqU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void CltU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.CltU1U1(a, b), Run<bool>("Mosa.Test.Collection", "UInt8Tests", "CltU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void CgtU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.CgtU1U1(a, b), Run<bool>("Mosa.Test.Collection", "UInt8Tests", "CgtU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void CleU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.CleU1U1(a, b), Run<bool>("Mosa.Test.Collection", "UInt8Tests", "CleU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void CgeU1U1(byte a, byte b)
		{
			Assert.AreEqual(UInt8Tests.CgeU1U1(a, b), Run<bool>("Mosa.Test.Collection", "UInt8Tests", "CgeU1U1", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt8Tests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt8Tests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_U1")]
		public void StelemU1(int index, byte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt8Tests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_U1")]
		public void LdelemU1(int index, byte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt8Tests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_U1")]
		public void LdelemaU1(int index, byte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt8Tests", "Ldelema", index, value));
		}

	}
}
