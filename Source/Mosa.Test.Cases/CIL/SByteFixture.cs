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
	public class SByteFixture : TestCompilerAdapter
	{

		public SByteFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void AddU1U1(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.AddU1U1(a, b), Run<int>("Mosa.Test.Collection", "SByteTests", "AddU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void SubU1U1(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.SubU1U1(a, b), Run<int>("Mosa.Test.Collection", "SByteTests", "SubU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void MulU1U1(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.MulU1U1(a, b), Run<int>("Mosa.Test.Collection", "SByteTests", "MulU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1WithoutZero")]
		public void DivU1U1(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.DivU1U1(a, b), Run<int>("Mosa.Test.Collection", "SByteTests", "DivU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU1U1DivideByZeroException(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.DivU1U1(a, b), Run<int>("Mosa.Test.Collection", "SByteTests", "DivU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1WithoutZero")]
		public void RemU1U1(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.RemU1U1(a, b), Run<int>("Mosa.Test.Collection", "SByteTests", "RemU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU1U1DivideByZeroException(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.RemU1U1(a, b), Run<int>("Mosa.Test.Collection", "SByteTests", "RemU1U1", a, b));
		}

		[Test, Factory(typeof(I1), "Samples")]
		public void RetU1(sbyte a)
		{
			Assert.AreEqual(SByteTests.RetU1(a), Run<sbyte>("Mosa.Test.Collection", "SByteTests", "RetU1", a));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void AndU1U1(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.AndU1U1(a, b), Run<int>("Mosa.Test.Collection", "SByteTests", "AndU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void OrU1U1(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.OrU1U1(a, b), Run<int>("Mosa.Test.Collection", "SByteTests", "OrU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void XorU1U1(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.XorU1U1(a, b), Run<int>("Mosa.Test.Collection", "SByteTests", "XorU1U1", a, b));
		}

		//[Test, Factory(typeof(U1), "Samples")]
		//public void CompU1(sbyte a)
		//{
		//    Assert.AreEqual(SByteTests.CompU1(a), Run<int>("Mosa.Test.Collection", "SByteTests", "CompU1", a));
		//}

		//[Test, Factory(typeof(Variations), "I1_U1UpTo8")]
		//public void ShiftLeftU1U1(sbyte a, byte b)
		//{
		//    Assert.AreEqual(SByteTests.ShiftLeftU1U1(a, b), Run<int>("Mosa.Test.Collection", "SByteTests", "ShiftLeftU1U1", a, b));
		//}

		//[Test, Factory(typeof(Variations), "I1_U1UpTo8")]
		//public void ShiftRightU1U1(sbyte a, byte b)
		//{
		//    Assert.AreEqual(SByteTests.ShiftRightU1U1(a, b), Run<int>("Mosa.Test.Collection", "SByteTests", "ShiftRightU1U1", a, b));
		//}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CeqU1U1(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.CeqU1U1(a, b), Run<bool>("Mosa.Test.Collection", "SByteTests", "CeqU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CltU1U1(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.CltU1U1(a, b), Run<bool>("Mosa.Test.Collection", "SByteTests", "CltU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CgtU1U1(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.CgtU1U1(a, b), Run<bool>("Mosa.Test.Collection", "SByteTests", "CgtU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CleU1U1(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.CleU1U1(a, b), Run<bool>("Mosa.Test.Collection", "SByteTests", "CleU1U1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CgeU1U1(sbyte a, sbyte b)
		{
			Assert.AreEqual(SByteTests.CgeU1U1(a, b), Run<bool>("Mosa.Test.Collection", "SByteTests", "CgeU1U1", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SByteTests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SByteTests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void StelemU1(int index, sbyte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SByteTests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void LdelemU1(int index, sbyte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SByteTests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void LdelemaU1(int index, sbyte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SByteTests", "Ldelema", index, value));
		}

	}
}
