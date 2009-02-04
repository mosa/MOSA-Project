/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@mosa-project.org>)
 */

namespace Pictor.Renderer.PixelFormats
{
    /// <summary>
    /// 
    /// </summary>
    public class RgbPixelFormat<BaseType> : BasePixelFormat<ColorTypes.RgbColor<BaseType>>
    {
        /// <summary>
        /// 
        /// </summary>
        private RenderingBuffer<BaseType> _rbuf;

        /// <summary>
        /// Gets the buffer.
        /// </summary>
        /// <value>The buffer.</value>
        public RenderingBuffer<BaseType> Buffer
        {
            get
            {
                return _rbuf;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RgbPixelFormat&lt;BaseType&gt;"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="buffer">The buffer.</param>
        public RgbPixelFormat(int width, int height, RenderingBuffer<BaseType> buffer)
        {
            Width = width;
            Height = height;
            _rbuf = buffer;
        }

        /// <summary>
        /// Copies the pixel.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="c">The c.</param>
        public override void CopyPixel(int x, int y, ColorTypes.RgbColor<BaseType> c)
        {
            BaseType[] p = new BaseType[3];
            p[0] = c.r;
            p[1] = c.g;
            p[2] = c.b;
            _rbuf.SetPartialRow(x, y, 1, p);
        }

        /// <summary>
        /// Copies the H line.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="len">The len.</param>
        /// <param name="c">The c.</param>
        public override void CopyHLine(int x, int y, uint len, ColorTypes.RgbColor<BaseType> c)
        {
            BaseType[] p = new BaseType[len * 3];
            for (int i = 0; i < len; i += 3)
            {
                p[i    ] = c.r;
                p[i + 1] = c.g;
                p[i + 2] = c.b;
            }
            _rbuf.SetPartialRow(x, y, len, p);
        }

        /// <summary>
        /// Blends the H line.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="len">The len.</param>
        /// <param name="c">The c.</param>
        /// <param name="cover">The cover.</param>
        public override void BlendHLine(int x, int y,
                               uint len, 
                               ColorTypes.RgbColor<BaseType> c,
                               byte cover)
        {
            return;
        }
    }
}