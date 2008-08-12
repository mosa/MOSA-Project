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
    /// Holds test cases for the bitwise shift left operation.
    /// </summary>
    static class OpShr
    {
        /// <summary>
        /// Tests the bitwise right-shift
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int Test_ShrOnInt32()
        {
            int a = 128;

            for (int b = 1; b < 4; b++)
            {
                a = a >> b;
            }

            return (a == 2) ? 1 : 0;
        }

        /// <summary>
        /// Tests the bitwise right-shift operation on 64-bit integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int _Test_ShrOnInt64()
        {
            return 0;
        }

        /// <summary>
        /// Tests the bitwise right-shift operation on native integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int _Test_ShrOnNativeInt()
        {
            //IntPtr i1, i2;
            return 0;
        }
    }
}
