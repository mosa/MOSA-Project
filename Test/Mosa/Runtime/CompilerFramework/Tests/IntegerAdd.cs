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
    public static class IntegerAdd
    {
        public static int Test_Foo()
        {
            int i = 8;
            int j = 8;
            for (int a = 0; a < 5; a++)
            {
                i = i + j;
                i = i - a * (j + 2);
                i = i << 3;
            }
            return i * j;
        }
    }
}
