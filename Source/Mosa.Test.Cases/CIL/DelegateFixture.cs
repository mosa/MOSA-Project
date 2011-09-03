/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai Patrick Reisert <kpreisert@googlemail.com> 
 */

using System;
using MbUnit.Framework;

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
			Assert.AreEqual(1, Run<int>("Mosa.Test.Collection", "DelegateTests", "CallDelegateVoid1"));
		}

        [Test]
        public void CallDelegateVoid2()
        {
            Assert.AreEqual(2, Run<int>("Mosa.Test.Collection", "DelegateTests", "CallDelegateVoid2"));
        }

        [Test]
        public void ReassignDelegateVoid()
        {
            Assert.AreEqual(3, Run<int>("Mosa.Test.Collection", "DelegateTests", "ReassignDelegateVoid"));
        }

        [Test]
        [Row(3,5)]
        public void CallDelegateParameters(int a, int b)
        {
            Assert.AreEqual(a * 10 + b, Run<int>("Mosa.Test.Collection", "DelegateTests", "CallDelegateParameters", a, b));
        }

        [Test]
        public void CallDelegateReturn()
        {
            Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DelegateTests", "CallDelegateReturn"));
        }

        [Test]
        public void CallDelegateParametersReturn()
        {
            Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DelegateTests", "CallDelegateParametersReturn", 3, 5));
        }

        [Test]
        public void CallDelegateBox()
        {
             Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DelegateTests", "CallDelegateBox", 99));
        }

        [Test]
        public void CallDelegateGenericReturn()
        {
            Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DelegateTests", "CallDelegateGenericReturn"));
        }

        [Test]
        public void CallDelegateGenericReturnStructA()
        {
            Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DelegateTests", "CallDelegateGenericReturnStructA"));
        }

        [Test]
        public void CallDelegateGenericReturnStructB()
        {
            Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DelegateTests", "CallDelegateGenericReturnStructB"));
        }

        [Test]
        public void CallDelegateGenericTarget()
        {
            settings.CodeSource = @"
namespace Mosa.Test.Code
{
	public static class DelegateTests
	{
        delegate void DelegateGenericTarget(int p);

        public static bool CallDelegateGenericTarget()
        {
            DelegateGenericTarget d = DelegateGenericTargetTarget<int>;
            d(1);
            return true;
        }

        public static void DelegateGenericTargetTarget<T>(T p)
        {
        }
    }
}";
            Assert.IsTrue(Run<bool>("Mosa.Test.Code", "DelegateTests", "CallDelegateGenericTarget"));
            settings.CodeSource = string.Empty;
        }

        [Test]
        public void CallDelegateGenericTargetReturn()
        {
            settings.CodeSource = @"
namespace Mosa.Test.Code
{
	public static class DelegateTests
	{
        delegate T DelegateGenericTargetReturn<T>(T p);

        public static bool CallDelegateGenericTargetReturn()
        {
            DelegateGenericTargetReturn<int> d = DelegateGenericTargetReturnTarget<int>;
            return d(50) == 50;
        }

        public static T DelegateGenericTargetReturnTarget<T>(T p)
        {
            return p;
        }
    }
}";
            Assert.IsTrue(Run<bool>("Mosa.Test.Code", "DelegateTests", "CallDelegateGenericTargetReturn"));
            settings.CodeSource = string.Empty;
        }
	}
}