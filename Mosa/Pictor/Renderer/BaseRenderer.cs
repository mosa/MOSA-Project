/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

namespace Pictor.Renderer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="PixelFormat">The type of the pixelformat.</typeparam>
    /// <typeparam name="ColorType">The type of the olor type.</typeparam>
    public class BaseRenderer<PixelFormat, ColorType> where PixelFormat : PixelFormats.BasePixelFormat<ColorType>
                                                      where ColorType : ColorTypes.IColorType
    {
        /// <summary>
        /// 
        /// </summary>
        private PixelFormat _ren;
        /// <summary>
        /// 
        /// </summary>
        private Math.Rect<int> _clipBox;

        /// <summary>
        /// Gets the pixelformat.
        /// </summary>
        public PixelFormat Ren
        {
            get
            {
                return _ren;
            }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get
            {
                return Ren.Width;
            }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get
            {
                return Ren.Height;
            }
        }

        /// <summary>
        /// Gets the x min.
        /// </summary>
        /// <value>The x min.</value>
        int xMin
        { 
            get
            {
               return _clipBox.Left; 
            }
        }

        /// <summary>
        /// Gets the x max.
        /// </summary>
        /// <value>The x max.</value>
        int xMax
        {
            get
            {
                return _clipBox.Right;
            }
        }

        /// <summary>
        /// Gets the y min.
        /// </summary>
        /// <value>The y min.</value>
        int yMin
        {
            get
            {
                return _clipBox.Top;
            }
        }

        /// <summary>
        /// Gets the y max.
        /// </summary>
        /// <value>The y max.</value>
        int yMax
        {
            get
            {
                return _clipBox.Bottom;
            }
        }

        /// <summary>
        /// Attaches the specified FMT.
        /// </summary>
        /// <param name="fmt">The FMT.</param>
        public void Attach(PixelFormat fmt)
        {
            _ren = fmt;
            _clipBox = new Math.Rect<int>(0, 0, fmt.Width - 1, fmt.Height - 1);
        }

        /// <summary>
        /// Ins the box.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool InBox(int x, int y) 
        {
            return x >= _clipBox.Left  && y >= _clipBox.Top &&
                   x <= _clipBox.Right && y <= _clipBox.Bottom;
        }

        /// <summary>
        /// Copies the H line.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y">The y.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="c">The c.</param>
        public void CopyHLine(int x1, int y, int x2, ColorType c)
        {
            if (x1 > x2) 
            { 
                int t = x2;
                x2 = x1;
                x1 = t; 
            }

            if (y  > yMax) 
                return;
            if (y  < yMin) 
                return;
            if (x1 > xMax) 
                return;
            if (x2 < xMin) 
                return;

            if (x1 < xMin)
                x1 = xMin;
            if (x2 > xMax) 
                x2 = xMax;

            Ren.CopyHLine(x1, y, (uint)(x2 - x1 + 1), c);
        }

        /// <summary>
        /// Copies the pixel.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="c">The c.</param>
        public void CopyPixel(int x, int y, ColorType c)
        {
            if (InBox(x, y))
                Ren.CopyPixel(x, y, c);
        }

        /// <summary>
        /// Blend_hlines the specified x1.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y">The y.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="c">The c.</param>
        /// <param name="cover">The cover.</param>
        public void BlendHLine(int x1, int y, int x2, 
                                ColorType c, byte cover)
        {
            if (x1 > x2) 
            { 
                int t = x2; 
                x2 = x1; 
                x1 = t; 
            }
            if (y  > yMax) 
                return;
            if (y  < yMin) 
                return;
            if (x1 > xMax) 
                return;
            if (x2 < xMin) 
                return;

            if (x1 < xMin) 
                x1 = xMin;
            if (x2 > xMax) 
                x2 = xMax;

            Ren.BlendHLine(x1, y, (uint)(x2 - x1 + 1), c, cover);
        }
    }
}