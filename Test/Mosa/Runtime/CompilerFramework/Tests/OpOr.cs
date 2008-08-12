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
    public static class OpOr
    {
        /// <summary>
        /// Tests the bitwise or operation on 32-bit integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int Test_OrOnInt32()
        {
            int a, b = 0;

            for (a = 1; a <= 32; a = a * 2)
            {
                b = (b | a);
            }

            if (b == 63)
                return 1;

            return 0;
        }

        /// <summary>
        /// Tests the bitwise or operation on 64-bit integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int _Test_OrOnInt64()
        {
            return 0;
        }

        /// <summary>
        /// Tests the bitwise or operation on native integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int _Test_OrOnNativeInt()
        {
            //IntPtr i1, i2;
            return 0;
        }
    }
}
