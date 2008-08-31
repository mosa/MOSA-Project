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
    static class OpCall
    {
        public static int Test_CallI4()
        {
            return 0 == CallI4(0) ? 1 : 0;
        }

        private static int CallI4(int i)
        {
            return i;
        }

        public static int Test_CallEmpty()
        {
            int i = CallEmpty();
            DoNothing();
            return i;
        }

        public static int CallEmpty()
        {
            return 1;
        }

        public static int Test_CallDiv()
        {
            return (2 == Div(14, 7) ? 1 : 0);
        }

        //public static void Test_CallIgnoreResult()
        //{
        //    CallEmpty();
        //}

        private static int Div(int dividend, int divisor)
        {
            return (dividend / divisor);
        }

        private static void DoNothing()
        {
        }
    }
}
