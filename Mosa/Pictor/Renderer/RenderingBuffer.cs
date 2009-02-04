/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@mosa-project.org>)
 */

namespace Pictor.Renderer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="BaseType">The type of the ase type.</typeparam>
    public class RenderingBuffer<BaseType>
    {
        /// <summary>
        /// 
        /// </summary>
        private BaseType[] _buf;
        /// <summary>
        /// 
        /// </summary>
        private uint _width;
        /// <summary>
        /// 
        /// </summary>
        private uint _height;
        /// <summary>
        /// 
        /// </summary>
        private int _stride;

        /// <summary>
        /// Gets the absolute stride.
        /// </summary>
        /// <value>The absolute stride.</value>
        public uint AbsoluteStride
        {
            get
            {
                return (uint)System.Math.Abs(_stride);
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public uint Width
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
        public uint Height
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
        /// Initializes a new instance of the <see cref="RenderingBuffer&lt;BaseType&gt;"/> class.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="stride">The stride.</param>
        public RenderingBuffer(BaseType[] buffer, uint width, uint height, int stride)
        {
            _buf = buffer;
            Width = width;
            Height = height;
            _stride = stride;
        }

        /// <summary>
        /// Attaches the specified buf.
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="stride">The stride.</param>
        public void Attach(BaseType[] buf, uint width, uint height, int stride)
        {
			_buf = buf;
			_width = width;
			_height = height;
			_stride = stride;
        }

        /// <summary>
        /// Gets the partial row.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public System.ArraySegment<BaseType> GetPartialRow(int y)
        {
            return GetPartialRow(0, y, Width);
        }

        /// <summary>
        /// Gets the partial row.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="len">The len.</param>
        /// <returns></returns>
        public System.ArraySegment<BaseType> GetPartialRow(int x, int y, uint len)
        {
            // Check pixel boundings
            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;
            return new System.ArraySegment<BaseType>(_buf, (int)((y * Width + x) * _stride), (int)len * _stride);
        }

        /// <summary>
        /// Sets the partial row.
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="len">The rows length</param>
        /// <param name="buf">The buffer to copy from</param>
        public void SetPartialRow(int x, int y, uint len, BaseType[] buf)
        {
            // Copy the buffer
            System.Array.Copy(buf, 0, _buf, (y * Width + x) * _stride, len * _stride);
        }

        /// <summary>
        /// Clears the buffer.
        /// </summary>
        /// <param name="value">The color to clear the buffer with.</param>
        public void Clear(BaseType value)
        {
            uint w = Width;
            uint stride = AbsoluteStride;

            // Create a buffer for one line 
            BaseType[] p = new BaseType[w * stride];

            // And set the specified color
            for (int x = 0; x < w * stride; ++x)
                p[x] = value;

            // Set each line
            for (int y = 0; y < Height; ++y)
                SetPartialRow(0, y, w, p);
        }

        /// <summary>
        /// Copies from another rendering buffer
        /// </summary>
        /// <param name="src">The buffer to copy from</param>
        public void CopyFrom(RenderingBuffer<BaseType> src)
        {
            // Resize to source buffer
            if (src.Height < Height)
                Height = src.Height;

            // Calculate absolute stride
            uint l = AbsoluteStride;
            if (src.AbsoluteStride < l) 
                l = src.AbsoluteStride;

            // Copy rows from sourcebuffer 
            for (int y = 0; y < Height; y++)
                SetPartialRow(0, y, Width, src.GetPartialRow(y).Array);
        }
    }
}