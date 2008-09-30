
namespace Pictor.Renderer.Scanline
{
    /// <summary>
    /// 
    /// </summary>
    public class ScanlineHitTest
    {
        /// <summary>
        /// 
        /// </summary>
        private int _x;
        /// <summary>
        /// 
        /// </summary>
        private bool _hit = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanlineHitTest"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        public ScanlineHitTest(int x) 
        {
            _x = x;
        }

        /// <summary>
        /// Resets the spans.
        /// </summary>
        public void ResetSpans() 
        {
        }

        /// <summary>
        /// Finalizes the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        public void Finalize(int x) 
        {
        }

        /// <summary>
        /// Adds the cell.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="f">The f.</param>
        void AddCell(int x, int f)
        {
            if (_x == x) 
                _hit = true;
        }

        /// <summary>
        /// Adds the span.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="len">The len.</param>
        /// <param name="f">The f.</param>
        void AddSpan(int x, int len, int f)
        {
            if (_x >= x && _x < x + len) 
                _hit = true;
        }

        /// <summary>
        /// Gets the number of spans.
        /// </summary>
        /// <value>The number of spans.</value>
        public uint NumberOfSpans
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Hits this instance.
        /// </summary>
        /// <returns></returns>
        public bool Hit()
        { 
            return _hit; 
        }
    }
}
