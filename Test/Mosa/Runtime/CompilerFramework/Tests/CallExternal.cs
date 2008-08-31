using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Mosa.Runtime.CompilerFramework.Tests
{
    public static class CallExternal
    {
        public static void Test_ObjectEquals()
        {
            bool result = System.Object.Equals(1, 2);
        }
    }
}
