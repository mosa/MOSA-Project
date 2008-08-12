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
    /// Holds test cases for integer division.
    /// </summary>
    public static class OpDiv
    {
        public static int Test_DivInt32()
        {
            int i = 262144;
            int j = 8;
            for (int a = 0; a < 5; a++)
            {
                i = i / j;
            }
            return (i == 8 ? 1 : 0);
        }

        public static int _Test_DivDouble()
        {
            return 0;
        }
    }
}
