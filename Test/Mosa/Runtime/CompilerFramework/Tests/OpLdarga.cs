using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Mosa.Runtime.CompilerFramework.Tests
{
    static class OpLdarga
    {
        public static int Test_CallEmpty()
        {
            int x = 3;
            int y = x + 1;
            CallEmpty(ref y);
            
            return (y == 5 ? 1 : 0);
        }

        public static void CallEmpty(ref int x)
        {
            x = x + 1;
        }
    }
}
