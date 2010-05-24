/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (aka Michael Ruck, grover <mailto:sharpos@michaelruck.de>)
 *
 */

namespace Test.Mosa.Runtime.CompilerFramework.CIL
{
    using MbUnit.Framework;

    [TestFixture]
    [Importance(Importance.Critical)]
    [Category(@"Compiler")]
    [Description(@"Tests support for interfaces.")]
    public class InterfaceFixture : CodeDomTestRunner
    {
        private static string CreateTestCode()
        {
            return @"
                public interface InterfaceA
                {
                    int A();
                }

                public interface InterfaceB
                {
                    int A();
                    int B();
                }

                public class TestClass : InterfaceA, InterfaceB
                {
                    public int A()
                    {
                        return 1;
                    }

                    int InterfaceB.A()
                    {
                        return 2;
                    }

                    public int B()
                    {
                        return 3;
                    }

                    public static bool MustCompileWithInterfaces()
                    {
                        return true;
                    }

                    public static bool MustReturn3FromB()
                    {
                        TestClass tc = new TestClass();
                        bool result = tc.B() == 3;
                        InterfaceB b = tc;
                        result = result & (b.B() == 3);
                        return result;
                    }
                }
            "
            + Code.ObjectClassDefinition;
        }

        private delegate bool B_V();

        [Test]
        public void MustCompileInterfaces()
        {
            this.CodeSource = CreateTestCode();
            Assert.IsTrue((bool)Run<B_V>("", "TestClass", "MustCompileWithInterfaces"));
        }

        [Test]
        public void MustReturn3FromB()
        {
            this.CodeSource = CreateTestCode();
            Assert.IsTrue((bool)Run<B_V>("", "TestClass", "MustReturn3FromB"));
        }
    }
}
