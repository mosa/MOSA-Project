namespace Pictor.Renderer.Scanline
{
    /// <summary>
    /// An internal class that implements the main rasterization algorithm.
    /// Used in the rasterizer. Should not be used direcly.
    /// </summary>
    /// <typeparam name="Cell">The type of the Cell.</typeparam>
    public class AntiAliasedRasterizerCells<Cell>
    {
        /*private uint _num_blocks;
        private uint _max_blocks;
        private uint _curr_block;
        private uint _num_cells;
        private Cell[] _cells;
        private System.Collections.Generic.List<Cell> _sortedCells;
        private System.Collections.Generic.List<SortedY> _sortedY;
        private Cell _currentCell;
        private Cell _styleCell;*/
        //private int _minX;
        private int _minY = 0;
        //private int _maxX;
        private int _maxY = 0;
        //private bool _sorted;
        /// <summary>
        /// 
        /// </summary>
        enum CellBlockScale
        {
            /// <summary>
            /// 
            /// </summary>
            CellBlockShift = 12,
            /// <summary>
            /// 
            /// </summary>
            CellBlockSize = 1 << CellBlockShift,
            /// <summary>
            /// 
            /// </summary>
            CellBlockMask = CellBlockSize - 1,
            /// <summary>
            /// 
            /// </summary>
            CellBlockPool = 256,
            /// <summary>
            /// 
            /// </summary>
            CellBlockLimit = 1024
        };

        struct SortedY
        {
            /// <summary>
            /// 
            /// </summary>
            public uint start;
            /// <summary>
            /// 
            /// </summary>
            public uint num;

            /// <summary>
            /// Initializes a new instance of the <see cref="AntiAliasedRasterizerCells&lt;Cell&gt;.SortedY"/> struct.
            /// </summary>
            /// <param name="s">The s.</param>
            /// <param name="n">The n.</param>
            public SortedY(uint s, uint n)
            {
                start = s;
                num = n;
            }
        };

        /// <summary>
        /// Gets the total cells.
        /// </summary>
        /// <value>The total cells.</value>
        public uint TotalCells
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the min Y.
        /// </summary>
        /// <value>The min Y.</value>
        public int MinY
        {
            get
            {
                return _minY;
            }
        }

        /// <summary>
        /// Gets the max Y.
        /// </summary>
        /// <value>The max Y.</value>
        public int MaxY
        {
            get
            {
                return _maxY;
            }
        }

        /// <summary>
        /// Sorts the cells.
        /// </summary>
        public void SortCells()
        {
        }
    }
}