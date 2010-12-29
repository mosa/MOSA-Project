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
	public class ByteFixture : TestCompilerAdapter
	{

		public ByteFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void AddU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.AddU1U1(a, b), Run<int>("Mosa.Test.Collection", "ByteTests", "AddU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void SubU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.SubU1U1(a, b), Run<int>("Mosa.Test.Collection", "ByteTests", "SubU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void MulU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.MulU1U1(a, b), Run<int>("Mosa.Test.Collection", "ByteTests", "MulU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1WithoutZero")]
		public void DivU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.DivU1U1(a, b), Run<int>("Mosa.Test.Collection", "ByteTests", "DivU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU1U1DivideByZeroException(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.DivU1U1(a, b), Run<int>("Mosa.Test.Collection", "ByteTests", "DivU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1WithoutZero")]
		public void RemU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.RemU1U1(a, b), Run<int>("Mosa.Test.Collection", "ByteTests", "RemU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU1U1DivideByZeroException(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.RemU1U1(a, b), Run<int>("Mosa.Test.Collection", "ByteTests", "RemU1U1", a, b));
		}

		[Test, Factory(typeof(U1), "Samples")]
		public void RetU1(byte a)
		{
			Assert.AreEqual(ByteTests.RetU1(a), Run<byte>("Mosa.Test.Collection", "ByteTests", "RetU1", a));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void AndU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.AndU1U1(a, b), Run<int>("Mosa.Test.Collection", "ByteTests", "AndU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void OrU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.OrU1U1(a, b), Run<int>("Mosa.Test.Collection", "ByteTests", "OrU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void XorU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.XorU1U1(a, b), Run<int>("Mosa.Test.Collection", "ByteTests", "XorU1U1", a, b));
		}

		[Test, Factory(typeof(U1), "Samples")]
		public void CompU1(byte a)
		{
			Assert.AreEqual(ByteTests.CompU1(a), Run<int>("Mosa.Test.Collection", "ByteTests", "CompU1", a));
		}

		[Test, Factory(typeof(Variations), "U1_U1UpTo8")]
		public void ShiftLeftU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.ShiftLeftU1U1(a, b), Run<int>("Mosa.Test.Collection", "ByteTests", "ShiftLeftU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1UpTo8")]
		public void ShiftRightU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.ShiftRightU1U1(a, b), Run<int>("Mosa.Test.Collection", "ByteTests", "ShiftRightU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void CeqU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.CeqU1U1(a, b), Run<bool>("Mosa.Test.Collection", "ByteTests", "CeqU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void CltU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.CltU1U1(a, b), Run<bool>("Mosa.Test.Collection", "ByteTests", "CltU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void CgtU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.CgtU1U1(a, b), Run<bool>("Mosa.Test.Collection", "ByteTests", "CgtU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void CleU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.CleU1U1(a, b), Run<bool>("Mosa.Test.Collection", "ByteTests", "CleU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "U1_U1")]
		public void CgeU1U1(byte a, byte b)
		{
			Assert.AreEqual(ByteTests.CgeU1U1(a, b), Run<bool>("Mosa.Test.Collection", "ByteTests", "CgeU1U1", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ByteTests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ByteTests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_U1")]
		public void StelemU1(int index, byte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ByteTests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_U1")]
		public void LdelemU1(int index, byte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ByteTests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_U1")]
		public void LdelemaU1(int index, byte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ByteTests", "Ldelema", index, value));
		}

	}
}
