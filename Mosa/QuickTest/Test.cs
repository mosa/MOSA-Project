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
		public int y;
		public int z;
    }

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
            Struct s = new Struct();

			s.x = 123;
        }
    }
}
