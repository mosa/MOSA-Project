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
    /// Holds test cases for the bitwise or operation.
    /// </summary>
    public static class OpXor
    {
        /// <summary>
        /// Tests the bitwise or operation on 32-bit integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int Test_XorOnInt32()
        {
            int a, b = 32;

            for (a = 1; a <= 32; a = a * 2)
            {
                b = b - (b ^ a);
            }

            if (b == 0)
                return 1;

            return 0;
        }

        /// <summary>
        /// Tests the bitwise or operation on 64-bit integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int _Test_XorOnInt64()
        {
            return 0;
        }

        /// <summary>
        /// Tests the bitwise or operation on native integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int _Test_XorOnNativeInt()
        {
            //IntPtr i1, i2;
            return 0;
        }
    }
}
