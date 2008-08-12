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
    /// Holds test cases for integer multiplication.
    /// </summary>
    public static class OpMul
    {
        public static int Test_MulInt32()
        {
            int i = 8;
            int j = 8;
            for (int a = 0; a < 5; a++)
            {
                i = i * j;
            }
            return (i == 262144 ? 1 : 0);
        }

        public static int Test_MulDouble()
        {
            double i = 2.718281828459045;
            double j = 2.0;
            for (int a = 0; a < 5; a++)
            {
                i = i * j;
            }
            return (i == 86.98501851068944 ? 1 : 0);
        }
    }
}
