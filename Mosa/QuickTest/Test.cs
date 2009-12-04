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
    public struct Struct
    {
        public int x;
    }
    /// <summary>
    /// 
    /// </summary>
    public static class App
    {
		static public int y;

        /// <summary>
        /// Main
        /// </summary>
        public static void Main()
        {
			y = 543;

            Struct s = new Struct();

			//s.x = 123;
        }
    }
}
