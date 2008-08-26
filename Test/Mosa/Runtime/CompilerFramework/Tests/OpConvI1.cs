using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Mosa.Runtime.CompilerFramework.Tests
{
    public static class OpConvI1
    {
        public static int Test_ConvI1()
        {
            if (127 != ConvI1((int)127))
                return 0;

            if (127 != ConvI1((short)127))
                return 0;

            if (127 != ConvI1((sbyte)127))
                return 0;

            if (-128 != ConvI1((int)-128))
                return 0;

            if (-128 != ConvI1((short)-128))
                return 0;

            if (-128 != ConvI1((sbyte)-128))
                return 0;

            return 1;
        }

        private static sbyte ConvI1(int value)
        {
            return (sbyte)value;
        }

        private static sbyte ConvI1(short value)
        {
            return (sbyte)value;
        }

        private static sbyte ConvI1(sbyte value)
        {
            return (sbyte)value;
        }
    }
}
