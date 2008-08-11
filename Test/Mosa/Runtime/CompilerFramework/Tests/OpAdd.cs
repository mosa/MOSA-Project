using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Mosa.Runtime.CompilerFramework.Tests
{
    /// <summary>
    /// Holds test cases for the bitwise and operation.
    /// </summary>
    public static class OpAdd
    {
        public static int Test_AddInt32()
        {
            int i = 8;
            int j = 8;
            for (int a = 0; a < 5; a++)
            {
                i = i + j;
            }
            return (i == 48 ? 1 : 0);
        }

        public static int Test_AddDouble()
        {
            double i = 1.2;
            double j = 1.5;
            for (int a = 0; a < 5; a++)
            {
                i = i + j;
            }
            return (i == 8.7 ? 1 : 0);
        }
    }
}
