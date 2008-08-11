/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

namespace cltester.Tests
{
    public static class FloatingPointMul
    {
        public static double Test_Foo()
        {
            double i = 8.5;
            double j = 8.5;
            for (int a = 0; a < 5; a++)
            {
                i = i * j;
            }
            return i * j;
        }
    }
}
