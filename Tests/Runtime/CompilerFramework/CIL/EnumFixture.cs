/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (aka grover, <mailto:sharpos@michaelruck.de>)
 *  
 */

namespace Test.Mosa.Runtime.CompilerFramework.CIL
{
    using MbUnit.Framework;

    [TestFixture]
    [Importance(Importance.Critical)]
    [Category(@"Basic types")]
    [Description(@"Tests support for the basic type System.Enum")]
    public class EnumFixture : CodeDomTestRunner
    {
        private static string CreateTestCode()
        {
            return @"
                public enum TestEnum
                {
                    ItemA = 5,
                    ItemB
                }

                public class TestClass
                {
                    public static bool AMustBe5()
                    {
                        return 5 == (int)TestEnum.ItemA;
                    }
                }

            " 
            + Code.ObjectClassDefinition
            + Code.NoStdLibDefinitions;
        }

        private delegate bool B_V();

        [Test]
        public void ItemAMustEqual5()
        {
            this.CodeSource = CreateTestCode();
            this.DoNotReferenceMsCorlib = true;

            // Due to Code.NoStdLibDefinitions... :(
            this.UnsafeCode = true;

            Assert.IsTrue((bool)Run<B_V>("", "TestClass", "AMustBe5"));
        }
    }
}
