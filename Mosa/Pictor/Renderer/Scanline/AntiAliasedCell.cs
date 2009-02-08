/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

namespace Pictor.Renderer.Scanline
{
    /// <summary>
    /// A pixel cell. There're no constructors defined and it was done 
    /// intentionally in order to avoid extra overhead when allocating an 
    /// array of cells.
    /// </summary>
    public struct AntiAliasedCell : ICell
    {
        /// <summary>
        /// 
        /// </summary>
        private int _x;
        /// <summary>
        /// 
        /// </summary>
        private int _y;
        /// <summary>
        /// 
        /// </summary>
        private int _cover;
        /// <summary>
        /// 
        /// </summary>
        private int _area;
        /// <summary>
        /// Gets the X.
        /// </summary>
        /// <value>The X.</value>
        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }
        /// <summary>
        /// Gets the X.
        /// </summary>
        /// <value>The X.</value>
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }
        /// <summary>
        /// Gets or sets the cover.
        /// </summary>
        /// <value>The cover.</value>
        public int Cover
        {
            get
            {
                return _cover;
            }
            set
            {
                _cover = value;
            }
        }
        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        /// <value>The area.</value>
        public int Area
        {
            get
            {
                return _area;
            }
            set
            {
                _area = value;
            }
        }

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