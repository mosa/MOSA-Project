/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

namespace Pictor.Renderer.PixelFormats
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPixelFormat
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class BasePixelFormat<ColorType> : IPixelFormat where ColorType : ColorTypes.IColorType
    {
        /// <summary>
        /// 
        /// </summary>
        private int _width = 0;
        /// <summary>
        /// 
        /// </summary>
        private int _height = 0;

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        /// <summary>
        /// Copies the pixel.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="c">The c.</param>
        public abstract void CopyPixel(int x, int y, ColorType c);

        /// <summary>
        /// Copies the H line.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="len">The len.</param>
        /// <param name="c">The c.</param>
        public abstract void CopyHLine(int x, int y, uint len, ColorType c);
    }
}