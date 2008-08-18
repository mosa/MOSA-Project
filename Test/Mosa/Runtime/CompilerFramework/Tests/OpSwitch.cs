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
            int j = 4;

            switch (j)
            {
                case 1:
                    x = 1;
                    break;
                case 2:
                    x = 2;
                    break;
                case 3:
                    x = 3;
                    break;
                case 4:
                    x = 4;
                    break;
                case 5:
                    x = 5;
                    break;
            }

            return (x == 4 ? 1 : 0);
        }
    }
}
