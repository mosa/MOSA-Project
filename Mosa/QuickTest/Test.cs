/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.QuickTest
{
    /// <summary>
    /// 
    /// </summary>
    public static class App
    {
        /// <summary>
        /// Main
        /// </summary>
        public static void Main()
        {
            int a = 5;
            int b = 4;

            int c = a + b;
        }
        /// <summary>
        /// Muls the constant C right.
        /// </summary>
        /// <param name="expect">The expect.</param>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        static bool MulConstantCRight(char expect, char a)
        {
            return expect == a * (char)3;
        }

        /// <summary>
        /// Ceqs the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static bool Ceq(long a, long b)
        {
            return (a == b);
        }

        /// <summary>
        /// Adds the u1.
        /// </summary>
        /// <param name="expect">The expect.</param>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        static bool AddU1(byte expect, uint a, uint b)
        {
            return expect == (a + b);
        }
    }
}
