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
	public class UShortFixture : TestCompilerAdapter
	{

		public UShortFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void AddU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.AddU2U2(a, b), Run<int>("Mosa.Test.Collection", "UShortTests", "AddU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void SubU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.SubU2U2(a, b), Run<int>("Mosa.Test.Collection", "UShortTests", "SubU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void MulU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.MulU2U2(a, b), Run<int>("Mosa.Test.Collection", "UShortTests", "MulU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2WithoutZero")]
		public void DivU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.DivU2U2(a, b), Run<int>("Mosa.Test.Collection", "UShortTests", "DivU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU2U2DivideByZeroException(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.DivU2U2(a, b), Run<int>("Mosa.Test.Collection", "UShortTests", "DivU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2WithoutZero")]
		public void RemU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.RemU2U2(a, b), Run<int>("Mosa.Test.Collection", "UShortTests", "RemU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU2U2DivideByZeroException(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.RemU2U2(a, b), Run<int>("Mosa.Test.Collection", "UShortTests", "RemU2U2", a, b));
		}

		[Test, Factory(typeof(U2), "Samples")]
		public void RetU2(ushort a)
		{
			Assert.AreEqual(UShortTests.RetU2(a), Run<ushort>("Mosa.Test.Collection", "UShortTests", "RetU2", a));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void AndU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.AndU2U2(a, b), Run<int>("Mosa.Test.Collection", "UShortTests", "AndU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void OrU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.OrU2U2(a, b), Run<int>("Mosa.Test.Collection", "UShortTests", "OrU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void XorU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.XorU2U2(a, b), Run<int>("Mosa.Test.Collection", "UShortTests", "XorU2U2", a, b));
		}

		[Test, Factory(typeof(U2), "Samples")]
		public void CompU2(ushort a)
		{
			Assert.AreEqual(UShortTests.CompU2(a), Run<int>("Mosa.Test.Collection", "UShortTests", "CompU2", a));
		}

		[Test, Factory(typeof(Variations), "U2_I1UpTo16")]
		public void ShiftLeftU2U2(ushort a, byte b)
		{
			Assert.AreEqual(UShortTests.ShiftLeftU2U2(a, b), Run<int>("Mosa.Test.Collection", "UShortTests", "ShiftLeftU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_I1UpTo16")]
		public void ShiftRightU2U2(ushort a, byte b)
		{
			Assert.AreEqual(UShortTests.ShiftRightU2U2(a, b), Run<int>("Mosa.Test.Collection", "UShortTests", "ShiftRightU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CeqU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.CeqU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UShortTests", "CeqU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CltU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.CltU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UShortTests", "CltU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CgtU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.CgtU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UShortTests", "CgtU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CleU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.CleU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UShortTests", "CleU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CgeU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UShortTests.CgeU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UShortTests", "CgeU2U2", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UShortTests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UShortTests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_U2")]
		public void StelemU2(int index, ushort value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UShortTests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_U2")]
		public void LdelemU2(int index, ushort value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UShortTests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_U2")]
		public void LdelemaU2(int index, ushort value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UShortTests", "Ldelema", index, value));
		}

	}
}
