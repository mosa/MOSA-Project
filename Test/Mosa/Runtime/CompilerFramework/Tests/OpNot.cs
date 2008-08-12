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
    /// Holds test cases for the bitwise negation.
    /// </summary>
    public static class OpNot
    {
        /// <summary>
        /// Tests the bitwise or operation on 32-bit integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int Test_NotOnInt32()
        {
            int a, b = 0;

            for (a = 0; a < 10; a++)
            {
                b = b + (~(~a));
            }

            if (b == 45)
                return 1;

            return 0;
        }

        /// <summary>
        /// Tests the bitwise negation operation on 64-bit integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int _Test_NotOnInt64()
        {
            return 0;
        }

        /// <summary>
        /// Tests the bitwise negation operation on native integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int _Test_NotOnNativeInt()
        {
            //IntPtr i1, i2;
            return 0;
        }
    }
}
