/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:simon_wollwage@yahoo.co.jp>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Mosa.Runtime.CompilerFramework.Tests
{
    /// <summary>
    /// Holds test cases for integer substraction.
    /// </summary>
    public static class OpSub
    {
        public static int Test_SubInt32()
        {
            int i = 48;
            int j = 8;
            for (int a = 0; a < 5; a++)
            {
                i = i - j;
            }
            return (i == 8 ? 1 : 0);
        }

        public static int Test_SubDouble()
        {
            double i = 8.7;
            double j = 1.5;
            for (int a = 0; a < 5; a++)
            {
                i = i - j;
            }
            return (i == 1.2 ? 1 : 0);
        }
    }
}
