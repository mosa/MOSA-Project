/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

namespace Pictor.Renderer.Scanline
{
    /// <summary>
    /// A pixel cell. There're no constructors defined and it was done 
    /// intentionally in order to avoid extra overhead when allocating an 
    /// array of cells.
    /// </summary>
    public struct AntiAliasedCell
    {
        /// <summary>
        /// 
        /// </summary>
        public int X;
        /// <summary>
        /// 
        /// </summary>
        public int Y;
        /// <summary>
        /// 
        /// </summary>
        public int Cover;
        /// <summary>
        /// 
        /// </summary>
        public int Area;

        /// <summary>
        /// Initials this instance.
        /// </summary>
        public void Initial()
        {
            X = 0x7FFFFFFF;
            Y = 0x7FFFFFFF;
            Cover = 0;
            Area  = 0;
        }

        /// <summary>
        /// Styles the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        public void Style(AntiAliasedCell cell) 
        {
        }

        /// <summary>
        /// Nots the equal.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="ey">The ey.</param>
        /// <param name="cell">The cell.</param>
        /// <returns></returns>
        public bool NotEqual(int ex, int ey, AntiAliasedCell cell)
        {
            return ((ex - X) | (ey - Y)) != 0;
        }
    }
}