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

        //public static bool Ceq(long a, long b)
        //{
        //    return (a == b);
        //}
    }
}
