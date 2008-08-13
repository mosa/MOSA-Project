using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Mosa.Runtime.CompilerFramework.Tests
{
    /// <summary>
    /// Holds test cases for the bitwise and operation.
    /// </summary>
    public static class ConstantFolding
    {
        public static int Test_ConstantFolding()
        {
            int i = 8;
            int x = 2;
            int y = 7;
            for (int a = 0; a < 5; a++)
            {
                i = x + y;
            }
            return (i == 9 ? 1 : 0);
        }
    }
}
