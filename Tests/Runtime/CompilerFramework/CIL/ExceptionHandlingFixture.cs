/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 *
 */


using System;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework
{
    [TestFixture]
    [Parallelizable]
    public class ExceptionHandlingFixture : CodeDomTestRunner
    {
        private static string CreateTestCode()
        {
            return @"
                using System;

                static class Test
				{
                    public static int CatchAndReturn(int value)
                    {
                        try
                        {
                            throw new Exception ();
                        }
                        catch (Exception e)
                        {
                            return value;
                        }
                        return -value;
                    }

                    public static bool CatchException(int value)
                    {
                        return value == CatchAndReturn(value);
                    }
                }" + Code.ObjectClassDefinition;
        }

        private delegate bool B_I(int value);

        [Test]
        [Row(1)]
        public void CatchException (int value)
        {
            CodeSource = CreateTestCode();
            this.DoNotReferenceMsCorlib = true;
            this.References = new string[] { "mscorlib.dll" };
            Assert.IsTrue((bool)Run<B_I>("", "Test", "CatchException", value));
        }
    }
}