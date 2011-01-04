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

namespace Mosa.Test.Cases.FIX.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	public class SingleFixture : TestCompilerAdapter
	{

		public SingleFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void AddR4_R4([R4]float a, [R4]float b)
		{
			Assert.AreEqual(0, SingleTests.AddR4R4(a, b).CompareTo(Run<float>("Mosa.Test.Collection", "SingleTests", "AddR4R4", a, b)));
		}

		[Test]
		public void SubR4_R4([R4]float a, [R4]float b)
		{
			Assert.AreEqual(0, SingleTests.SubR4R4(a, b).CompareTo(Run<float>("Mosa.Test.Collection", "SingleTests", "SubR4R4", a, b)));
		}

		[Test]
		public void MulR4_R4([R4]float a, [R4]float b)
		{
			Assert.AreEqual(0, SingleTests.MulR4R4(a, b).CompareTo(Run<float>("Mosa.Test.Collection", "SingleTests", "MulR4R4", a, b)));
		}

		[Test]
		public void DivR4_R4([R4]float a, [R4NotZero]float b)
		{
			Assert.AreEqual(0, SingleTests.DivR4R4(a, b).CompareTo(Run<float>("Mosa.Test.Collection", "SingleTests", "DivR4R4", a, b)));
		}

		//[Test]
		//[ExpectedException(typeof(DivideByZeroException))]
		//public void DivR4_R4DivideByZeroException([R4]float a)
		//{
		//    Assert.AreEqual(0, SingleTests.DivR4R4(a, (float)0).CompareTo(Run<float>("Mosa.Test.Collection", "SingleTests", "DivR4R4", a, (float)0)));
		//}

		[Test]
		public void RemR4_R4([R4]float a, [R4NotZero]float b)
		{
			Assert.AreEqual(0, SingleTests.RemR4R4(a, b).CompareTo(Run<float>("Mosa.Test.Collection", "SingleTests", "RemR4R4", a, b)));
		}

		//[Test]
		//[ExpectedException(typeof(DivideByZeroException))]
		//public void RemR4_R4DivideByZeroException([R4]float a)
		//{
		//    Assert.AreEqual(0, SingleTests.RemR4R4(a, (float)0).CompareTo(Run<float>("Mosa.Test.Collection", "SingleTests", "RemR4R4", a, (float)0)));
		//}

		[Test]
		public void RetR4([R4]float a)
		{
			Assert.AreEqual(0, SingleTests.RetR4(a).CompareTo(Run<float>("Mosa.Test.Collection", "SingleTests", "RetR4", a)));
		}

		[Test]
		public void CeqR4_R4([R4]float a, [R4]float b)
		{
			Assert.AreEqual(SingleTests.CeqR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CeqR4R4", a, b));
		}

		[Test]
		public void CltR4_R4([R4]float a, [R4]float b)
		{
			Assert.AreEqual(SingleTests.CltR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CltR4R4", a, b));
		}

		[Test]
		public void CgtR4_R4([R4]float a, [R4]float b)
		{
			Assert.AreEqual(SingleTests.CgtR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CgtR4R4", a, b));
		}

		[Test]
		public void CleR4_R4([R4]float a, [R4]float b)
		{
			Assert.AreEqual(SingleTests.CleR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CleR4R4", a, b));
		}

		[Test]
		public void CgeR4_R4([R4]float a, [R4]float b)
		{
			Assert.AreEqual(SingleTests.CgeR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CgeR4R4", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SingleTests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small]int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SingleTests", "Ldlen", length));
		}

		[Test]
		public void StelemR4([I4Small]int index, [R4]float value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SingleTests", "Stelem", index, value));
		}

		[Test]
		public void LdelemR4([I4Small]int index, [R4]float value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SingleTests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaR4([I4Small]int index, [R4]float value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SingleTests", "Ldelema", index, value));
		}

	}
}


//[Test, Factory(typeof(Variations), "R4_R4WithoutZero")]
//public void DivR4R4([R4]float a, [R4]float b)
//{
//    this.arithmeticTests.Div((a / b), a, b);
//}

////[Test, Factory(typeof(Variations), "R4_R4Zero")]
////[ExpectedException(typeof(DivideByZeroException))]
////public void DivR4R4DivideByZeroException([R4]float a, [R4]float b)
////{
////    this.arithmeticTests.Div((a / b), a, b);
////}


//[Test, Factory(typeof(Variations), "R4_R4WithoutZero")]
//public void RemR4R4([R4]float a, [R4]float b)
//{
//    this.arithmeticTests.Rem((a % b), a, b);
//}

////[Test, Factory(typeof(Variations), "R4_R4Zero")]
////[ExpectedException(typeof(DivideByZeroException))]
////public void RemR4R4DivideByZeroException([R4]float a, [R4]float b)
////{
////    this.arithmeticTests.Rem((a % b), a, b);
////}

////[Test, Factory(typeof(Variations), "R4_R4BelowZero")]
////[ExpectedException(typeof(OverflowException))]
////public void RemR4R4OverflowException([R4]float a, [R4]float b)
////{
////    this.arithmeticTests.Rem((a % b), a, b);
////}

