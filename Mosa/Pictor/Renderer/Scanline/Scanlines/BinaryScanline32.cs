using System;
using System.Collections;

namespace Pictor.Renderer.Scanline.Scanlines
{
    /// <summary>
    /// 
    /// </summary>
    public class BinaryScanline32 : IScanline
    {
        /// <summary>
        /// 
        /// </summary>
        private ArrayList _spans = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        private int _lastX;
        /// <summary>
        /// 
        /// </summary>
        private int _y;
        struct Span
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Span"/> struct.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="length">The length.</param>
            public Span(int x, int length)
            {
                this.x = x;
                this.length = length;
            }

            /// <summary>
            /// 
            /// </summary>
            public int x;
            /// <summary>
            /// 
            /// </summary>
            public int length;
        };
        /// <summary>
        /// Resets the scanline
        /// </summary>
        /// <param name="minX">Mininum to begin at</param>
        /// <param name="maxX">Maximum to end at</param>
        public void Reset(int minX, int maxX)
        {
            _lastX = 0x7FFFFFF0;
            _spans.Clear();
        }
        /// <summary>
        /// Adds a new cell to the scanline
        /// </summary>
        /// <param name="x">Position to start at</param>
        /// <param name="cell">The ?.</param>
        public void AddCell(int x, uint cell)
        {
            if (x == _lastX + 1)
            {
                Span span = (Span)_spans[_spans.Count - 1];
                ++span.length;
            }
            else
                _spans.Add(new Span(x, 1));
            _lastX = x;
        }
        /// <summary>
        /// Adds a span to the scanline.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="length">The length.</param>
        /// <param name="s">The ?.</param>
        public void AddSpan(int x, uint length, uint s)
        {
            if (x == _lastX + 1)
            {
                Span span = (Span)_spans[_spans.Count - 1];
                span.length += (int)length;
            }
            else
                _spans.Add(new Span(x, (int)length));
            _lastX = (int)(x + length - 1);
        }
        /// <summary>
        /// Adds the cells.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="length">The length.</param>
        /// <param name="cells">The cells.</param>
        public void AddCells(int x, uint length, object[] cells)
        {
            AddSpan(x, length, 0);
        }
        /// <summary>
        /// Finalizes the specified y.
        /// </summary>
        /// <param name="y">The y.</param>
        public void Finalize(int y)
        {
            _y = y;
        }
        /// <summary>
        /// Resets all spans.
        /// </summary>
        public void ResetSpans()
        {
            _lastX = 0x7FFFFFF0;
            _spans.Clear();
        }

        /// <summary>
        /// Gets the Y.
        /// </summary>
        /// <value>The Y.</value>
        public int Y
        {
            get
            {
                return _y;
            }
        }

        /// <summary>
        /// Gets the number of spans.
        /// </summary>
        public uint NumberOfSpans
        {
            get
            {
                return (uint)_spans.Count;
            }
        }


    }
}
