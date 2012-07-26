/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai Patrick Reisert <kpreisert@googlemail.com> 
 */

using MbUnit.Framework;
using Mosa.Test.Collection;
using Mosa.Test.System;

namespace Mosa.Test.Cases.CIL
{
	public class DelegateFixture : TestCompilerAdapter
	{
		public DelegateFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void DefineDelegate()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DelegateTests", "DefineDelegate"));
		}

		[Test]
		public void CallDelegateVoid1()
		{
			Assert.AreEqual(DelegateTests.CallDelegateVoid1(), Run<int>("Mosa.Test.Collection", "DelegateTests", "CallDelegateVoid1"));
		}

		[Test]
		public void CallDelegateVoid2()
		{
			Assert.AreEqual(DelegateTests.CallDelegateVoid2(), Run<int>("Mosa.Test.Collection", "DelegateTests", "CallDelegateVoid2"));
		}

		[Test]
		public void ReassignDelegateVoid()
		{
			Assert.AreEqual(DelegateTests.ReassignDelegateVoid(), Run<int>("Mosa.Test.Collection", "DelegateTests", "ReassignDelegateVoid"));
		}

		[Test]
		public void CallDelegateParameters([I4Small]int a, [I4Small]int b)
		{
			Assert.AreEqual(DelegateTests.CallDelegateParameters(a, b), Run<int>("Mosa.Test.Collection", "DelegateTests", "CallDelegateParameters", a, b));
		}

		[Test]
		public void CallDelegateReturn([I4]int a)
		{
			Assert.AreEqual(DelegateTests.CallDelegateReturn(a), Run<int>("Mosa.Test.Collection", "DelegateTests", "CallDelegateReturn", a));
		}

		[Test]
		public void CallDelegateParametersReturn([I4Small]int a, [I4Small]int b)
		{
			Assert.AreEqual(DelegateTests.CallDelegateParametersReturn(a, b), Run<int>("Mosa.Test.Collection", "DelegateTests", "CallDelegateParametersReturn", a, b));
		}

		[Test]
		public void CallDelegateBox([I4]int a)
		{
			Assert.AreEqual(DelegateTests.CallDelegateBox(a), Run<int>("Mosa.Test.Collection", "DelegateTests", "CallDelegateBox", a));
		}

		[Test]
		[Row(99)]
		public void CallDelegateGenericReturn(int a)
		{
			Assert.AreEqual(DelegateTests.CallDelegateGenericReturn(a), Run<int>("Mosa.Test.Collection", "DelegateTests", "CallDelegateGenericReturn", a));
		}

		[Test]
		[Row(123)]
		public void CallDelegateGenericReturnStructA(int a)
		{
			Assert.AreEqual(DelegateTests.CallDelegateGenericReturnStructA(a), Run<int>("Mosa.Test.Collection", "DelegateTests", "CallDelegateGenericReturnStructA", a));
		}

		[Test]
		public void CallDelegateGenericReturnStructB()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DelegateTests", "CallDelegateGenericReturnStructB"));
		}

//        [Test]
//        public void CallDelegateGenericTarget()
//        {
//            settings.CodeSource = @"
//namespace Mosa.Test.Code
//{
//	public static class DelegateTests
//	{
//		delegate void DelegateGenericTarget(int p);
//
//		public static bool CallDelegateGenericTarget()
//		{
//			DelegateGenericTarget d = DelegateGenericTargetTarget<int>;
//			d(1);
//			return true;
//		}
//
//		public static void DelegateGenericTargetTarget<T>(T p)
//		{
//		}
//	}
//}";
//            Assert.IsTrue(Run<bool>("Mosa.Test.Code", "DelegateTests", "CallDelegateGenericTarget"));
//            settings.CodeSource = string.Empty;
//        }

//        [Test]
//        public void CallDelegateGenericTargetReturn()
//        {
//            settings.CodeSource = @"
//namespace Mosa.Test.Code
//{
//	public static class DelegateTests
//	{
//		delegate T DelegateGenericTargetReturn<T>(T p);
//
//		public static bool CallDelegateGenericTargetReturn()
//		{
//			DelegateGenericTargetReturn<int> d = DelegateGenericTargetReturnTarget<int>;
//			return d(50) == 50;
//		}
//
//		public static T DelegateGenericTargetReturnTarget<T>(T p)
//		{
//			return p;
//		}
//	}
//}";
//            Assert.IsTrue(Run<bool>("Mosa.Test.Code", "DelegateTests", "CallDelegateGenericTargetReturn"));
//            settings.CodeSource = string.Empty;
//        }

		[Test]
		public void CallInstanceDelegate()
		{
			Assert.AreEqual(DelegateTests.CallInstanceDelegate(), Run<int>("Mosa.Test.Collection", "DelegateTests", "CallInstanceDelegate"));
		}

		[Test]
		public void CallInstanceDelegateStatic()
		{
			Assert.AreEqual(DelegateTests.CallInstanceDelegateStatic(), Run<int>("Mosa.Test.Collection", "DelegateTests", "CallInstanceDelegateStatic"));
		}
	}
}