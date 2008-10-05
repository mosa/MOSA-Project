namespace Pictor.Renderer.Scanline.Scanlines
{
    /// <summary>
    /// This is binary scaline container which supports the interface 
    /// used in the Rasterizer.Render(). See description of ScanlineU8 
    /// for details.
    /// </summary>
    public class BinaryScanline : IScanline
    {
        /// <summary>
        /// 
        /// </summary>
        private System.Collections.ArrayList _spans = new System.Collections.ArrayList();
        /// <summary>
        /// 
        /// </summary>
        private Span _currentSpan;
        /// <summary>
        /// 
        /// </summary>
        private uint _currentSpanIndex = 0;
        /// <summary>
        /// 
        /// </summary>
        private int _lastX = 0;
        /// <summary>
        /// 
        /// </summary>
        private int _y = 0;

        /// <summary>
        /// 
        /// </summary>
        public class Span : ISpan<short>
        {
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
        /// <value></value>
        public uint NumberOfSpans
        {
            get
            {
                return _currentSpanIndex;
            }
        }

        /// <summary>
        /// Resets the scanline
        /// </summary>
        /// <param name="minX">Mininum to begin at</param>
        /// <param name="maxX">Maximum to end at</param>
        public void Reset(int minX, int maxX)
        {
            uint max_len = (uint)(maxX - minX + 3);
            if (max_len > _spans.Count)
            {
                _spans.Capacity = (int)max_len;
            }
            _lastX = 0x7FFFFFF0;
            _currentSpanIndex = 0;
            _currentSpan = (Span)_spans[(int)_currentSpanIndex];
        }

        /// <summary>
        /// Adds a new cell to the scanline
        /// </summary>
        /// <param name="x">Position to start at</param>
        /// <param name="cell"></param>
        public void AddCell(int x, uint cell)
        {
            if (x == _lastX + 1)
            {
                _currentSpan.length++;
            }
            else
            {
                _currentSpan = (Span)_spans[(int)++_currentSpanIndex];
                _currentSpan.x = (short)x;
                _currentSpan.length = 1;
            }
            _lastX = x;
        }

        /// <summary>
        /// Adds a span to the scanline.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="length">The length.</param>
        /// <param name="span"></param>
        public void AddSpan(int x, uint length, uint span)
        {
            if (x == _lastX + 1)
            {
                _currentSpan.length = (short)(_currentSpan.length + length);
            }
            else
            {
                _currentSpan = (Span)_spans[(int)++_currentSpanIndex];
                _currentSpan.x = (short)x;
                _currentSpan.length = (short)length;
            }
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
            _currentSpanIndex = 0;
            _currentSpan = (Span)_spans[(int)_currentSpanIndex];
        }

        /// <summary>
        /// Gets the span.
        /// </summary>
        /// <typeparam name="BaseType"></typeparam>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public ISpan<BaseType> GetSpan<BaseType>(int index)
        {
            return (ISpan<BaseType>)_spans[index];
        }
    }
}