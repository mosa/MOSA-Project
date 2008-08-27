using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Mosa.Runtime.CompilerFramework.Tests
{
    /// <summary>
    /// Holds test cases for the bitwise and operation.
    /// </summary>
    public static class OpSwitch
    {
        public static int Test_Switch()
        {
            int x = 0;
            int j = 2;
            int k = j + 1;

            switch (k)
            {
                case 1:
                    x = 9;
                    break;
                case 2:
                    x = 2;
                    break;
                case 3:
                    x = 7;
                    break;
                case 4:
                    x = 4;
                    break;
                case 5:
                    x = 5;
                    break;
            }

            return (x == 7 ? 1 : 0);
        }
    }
}
