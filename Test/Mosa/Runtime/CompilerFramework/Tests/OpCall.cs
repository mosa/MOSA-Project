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
        public static int Test_CallEmpty()
        {
            int i = CallEmpty();
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

        private static int Div(int dividend, int divisor)
        {
            return (dividend / divisor);
        }
    }
}
