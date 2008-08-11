/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

namespace Test.Mosa.Runtime.CompilerFramework.Tests
{
    public static class FloatingPointMul
    {
        public static int Test_Foo()
        {
            double i = 1.2;
            double j = 1.5;

            for (int x = 0; x < 2; x++)
            {
                i = i + 1.5;
            }

            return ((i == 4.2) ? 1 : 0);
        }
    }
}
