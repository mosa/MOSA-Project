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
	public class DoubleFixture : TestCompilerAdapter
	{

		public DoubleFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void AddR8_R8([R8]double a, [R8]double b)
		{
			Assert.AreEqual(0, DoubleTests.AddR8R8(a, b).CompareTo(Run<double>("Mosa.Test.Collection", "DoubleTests", "AddR8R8", a, b)));
		}

		[Test]
		public void SubR8_R8([R8]double a, [R8]double b)
		{
			Assert.AreEqual(0, DoubleTests.SubR8R8(a, b).CompareTo(Run<double>("Mosa.Test.Collection", "DoubleTests", "SubR8R8", a, b)));
		}

		[Test]
		public void MulR8_R8([R8]double a, [R8]double b)
		{
			Assert.AreEqual(0, DoubleTests.MulR8R8(a, b).CompareTo(Run<double>("Mosa.Test.Collection", "DoubleTests", "MulR8R8", a, b)));
		}

		[Test]
		public void DivR8_R8([R8]double a, [R8NotZero]double b)
		{
			Assert.AreEqual(0, DoubleTests.DivR8R8(a, b).CompareTo(Run<double>("Mosa.Test.Collection", "DoubleTests", "DivR8R8", a, b)));
		}

		//[Test]
		//[ExpectedException(typeof(DivideByZeroException))]
		//public void DivR8_R8DivideByZeroException([R8]double a)
		//{
		//    Assert.AreEqual(0, DoubleTests.DivR8R8(a, (double)0).CompareTo(Run<double>("Mosa.Test.Collection", "DoubleTests", "DivR8R8", a, (double)0)));
		//}

		[Test]
		public void RemR8_R8([R8]double a, [R8NotZero]double b)
		{
			Assert.AreEqual(0, DoubleTests.RemR8R8(a, b).CompareTo(Run<double>("Mosa.Test.Collection", "DoubleTests", "RemR8R8", a, b)));
		}

		//[Test]
		//[ExpectedException(typeof(DivideByZeroException))]
		//public void RemR8_R8DivideByZeroException([R8]double a)
		//{
		//    Assert.AreEqual(0, DoubleTests.RemR8R8(a, (double)0).CompareTo(Run<double>("Mosa.Test.Collection", "DoubleTests", "RemR8R8", a, (double)0)));
		//}

		[Test]
		public void RetR8([R8]double a)
		{
			Assert.AreEqual(0, DoubleTests.RetR8(a).CompareTo(Run<double>("Mosa.Test.Collection", "DoubleTests", "RetR8", a)));
		}

		[Test]
		public void CeqR8_R8([R8]double a, [R8]double b)
		{
			Assert.AreEqual(DoubleTests.CeqR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CeqR8R8", a, b));
		}

		[Test]
		public void CltR8_R8([R8]double a, [R8]double b)
		{
			Assert.AreEqual(DoubleTests.CltR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CltR8R8", a, b));
		}

		[Test]
		public void CgtR8_R8([R8]double a, [R8]double b)
		{
			Assert.AreEqual(DoubleTests.CgtR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CgtR8R8", a, b));
		}

		[Test]
		public void CleR8_R8([R8]double a, [R8]double b)
		{
			Assert.AreEqual(DoubleTests.CleR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CleR8R8", a, b));
		}

		[Test]
		public void CgeR8_R8([R8]double a, [R8]double b)
		{
			Assert.AreEqual(DoubleTests.CgeR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CgeR8R8", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small]int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Ldlen", length));
		}

		[Test]
		public void StelemR8([I4Small]int index, [R8]double value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Stelem", index, value));
		}

		[Test]
		public void LdelemR8([I4Small]int index, [R8]double value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaR8([I4Small]int index, [R8]double value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Ldelema", index, value));
		}

	}
}


		//[Test, Factory(typeof(Variations), "R8_R8WithoutZero")]
		//public void DivR8R8([R8]double a, [R8]double b)
		//{
		//    this.arithmeticTests.Div((a / b), a, b);
		//}

		////[Test, Factory(typeof(Variations), "R8_R8Zero")]
		////[ExpectedException(typeof(DivideByZeroException))]
		////public void DivR8R8DivideByZeroException([R8]double a, [R8]double b)
		////{
		////    this.arithmeticTests.Div((a / b), a, b);
		////}


		//[Test, Factory(typeof(Variations), "R8_R8WithoutZero")]
		//public void RemR8R8([R8]double a, [R8]double b)
		//{
		//    this.arithmeticTests.Rem((a % b), a, b);
		//}

		////[Test, Factory(typeof(Variations), "R8_R8Zero")]
		////[ExpectedException(typeof(DivideByZeroException))]
		////public void RemR8R8DivideByZeroException([R8]double a, [R8]double b)
		////{
		////    this.arithmeticTests.Rem((a % b), a, b);
		////}

		////[Test, Factory(typeof(Variations), "R8_R8BelowZero")]
		////[ExpectedException(typeof(OverflowException))]
		////public void RemR8R8OverflowException([R8]double a, [R8]double b)
		////{
		////    this.arithmeticTests.Rem((a % b), a, b);
		////}

