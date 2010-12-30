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
	public class UInt64Fixture : TestCompilerAdapter
	{

		public UInt64Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void AddU8U8(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.AddU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "AddU8U8", a, b));
		}

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void SubU8U8(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.SubU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "SubU8U8", a, b));
		}

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void MulU8U8(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.MulU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "MulU8U8", a, b));
		}

		[Test, Factory(typeof(Variations), "U8_U8WithoutZero")]
		public void DivU8U8(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.DivU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "DivU8U8", a, b));
		}

		[Test, Factory(typeof(Variations), "U8_U8Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU8U8DivideByZeroException(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.DivU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "DivU8U8", a, b));
		}

		[Test, Factory(typeof(Variations), "U8_U8WithoutZero")]
		public void RemU8U8(ulong a, ulong b)
		{
			//[Row(UInt64.MaxValue - 1, UInt64.MaxValue)] // BUG: Crashes test runner
			if ((a == UInt64.MaxValue - 1) && (b == UInt64.MaxValue))
				return;

			Assert.AreEqual(UInt64Tests.RemU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "RemU8U8", a, b));
		}

		[Test, Factory(typeof(Variations), "U8_U8Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU8U8DivideByZeroException(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.RemU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "RemU8U8", a, b));
		}

		[Test, Factory(typeof(U8), "Samples")]
		public void RetU8(ulong a)
		{
			Assert.AreEqual(UInt64Tests.RetU8(a), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "RetU8", a));
		}

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void AndU8U8(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.AndU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "AndU8U8", a, b));
		}

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void OrU8U8(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.OrU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "OrU8U8", a, b));
		}

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void XorU8U8(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.XorU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "XorU8U8", a, b));
		}

		[Test, Factory(typeof(U8), "Samples")]
		public void CompU8(ulong a)
		{
			Assert.AreEqual(UInt64Tests.CompU8(a), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "CompU8", a));
		}

		//[Test, Factory(typeof(Variations), "U8_U1UpTo64")]
		//public void ShiftLeftU8U8(ulong a, byte b)
		//{
		//    Assert.AreEqual(UInt64Tests.ShiftLeftU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "ShiftLeftU8U8", a, b));
		//}

		//[Test, Factory(typeof(Variations), "U8_U1UpTo64")]
		//public void ShiftRightU8U8(ulong a, byte b)
		//{
		//    Assert.AreEqual(UInt64Tests.ShiftRightU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "ShiftRightU8U8", a, b));
		//}

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void CeqU8U8(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.CeqU8U8(a, b), Run<bool>("Mosa.Test.Collection", "UInt64Tests", "CeqU8U8", a, b));
		}

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void CltU8U8(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.CltU8U8(a, b), Run<bool>("Mosa.Test.Collection", "UInt64Tests", "CltU8U8", a, b));
		}

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void CgtU8U8(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.CgtU8U8(a, b), Run<bool>("Mosa.Test.Collection", "UInt64Tests", "CgtU8U8", a, b));
		}

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void CleU8U8(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.CleU8U8(a, b), Run<bool>("Mosa.Test.Collection", "UInt64Tests", "CleU8U8", a, b));
		}

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void CgeU8U8(ulong a, ulong b)
		{
			Assert.AreEqual(UInt64Tests.CgeU8U8(a, b), Run<bool>("Mosa.Test.Collection", "UInt64Tests", "CgeU8U8", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt64Tests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt64Tests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_U8")]
		public void StelemU8(int index, ulong value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt64Tests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_U8")]
		public void LdelemU8(int index, ulong value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt64Tests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_U8")]
		public void LdelemaU8(int index, ulong value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt64Tests", "Ldelema", index, value));
		}

	}
}
