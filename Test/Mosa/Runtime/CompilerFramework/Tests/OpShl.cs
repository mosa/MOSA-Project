/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Mosa.Runtime.CompilerFramework.Tests
{
    /// <summary>
    /// Holds test cases for the bitwise shift left operation.
    /// </summary>
    static class OpShl
    {
        /// <summary>
        /// Tests the bitwise left-shift
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int Test_ShlOnInt32()
        {
            int a = 2;

            for (int b = 1; b < 4; b++)
            {
                a = a << 1;
            }

            return (a == 16) ? 1 : 0;
        }

        /// <summary>
        /// Tests the bitwise and operation on 64-bit integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int _Test_AndOnInt64()
        {
            return 0;
        }

        /// <summary>
        /// Tests the bitwise and operation on native integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int _Test_AndOnNativeInt()
        {
            //IntPtr i1, i2;
            return 0;
        }
    }
}
