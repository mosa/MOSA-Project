using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Mosa.Runtime.CompilerFramework.Tests
{
    /// <summary>
    /// Holds test cases for the bitwise and operation.
    /// </summary>
    static class OpAnd
    {
        /// <summary>
        /// Tests the bitwise and operation on 32-bit integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int Test_AndOnInt32()
        {
            int a, b;

            for (a = Int32.MinValue; a < Int32.MaxValue; a++)
            {
                b = a;
                if (a != (a & b))
                    return 0;
            }

            b = a;
            if (a != (a & b))
                return 0;

            return 1;
        }

        /// <summary>
        /// Tests the bitwise and operation on 64-bit integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int Test_AndOnInt64()
        {
            return 0;
        }

        /// <summary>
        /// Tests the bitwise and operation on native integers.
        /// </summary>
        /// <returns>Non-zero on success, zero on failure.</returns>
        public static int Test_AndOnNativeInt()
        {
            //IntPtr i1, i2;
            return 0;
        }
    }
}
