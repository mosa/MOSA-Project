namespace Pictor.Renderer.Scanline
{
    /// <summary>
    /// An internal class that implements the main rasterization algorithm.
    /// Used in the rasterizer. Should not be used direcly.
    /// </summary>
    /// <typeparam name="Cell">The type of the Cell.</typeparam>
    public class AntiAliasedRasterizerCells<Cell> where Cell : ICell
    {
        // <summary>
        // 
        // </summary>
        //private uint _num_blocks = 0;
        // <summary>
        // 
        // </summary>
        //private uint _max_blocks = 0;
        // <summary>
        // 
        // </summary>
        //private uint _curr_block = 0;
        // <summary>
        // 
        // </summary>
        //private Cell[] _cells;
        // <summary>
        // 
        // </summary>
        //private System.Collections.Generic.List<Cell> _sortedCells;
        // <summary>
        // 
        // </summary>
        //private System.Collections.Generic.List<SortedY> _sortedY;
        // <summary>
        // 
        // </summary>
        //private Cell _currentCell;
        // <summary>
        // 
        // </summary>
        //private Cell _styleCell;
        //private int _minX;
        /// <summary>
        /// 
        /// </summary>
        private int _minY = 0;
        //private int _maxX;
        /// <summary>
        /// 
        /// </summary>
        private int _maxY = 0;
        // <summary>
        // 
        // </summary>
        //private uint _numberOfCells;
        // <summary>
        // 
        // </summary>
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

        class SortedY
        {
            /// <summary>
            /// 
            /// </summary>
            public uint Start;
            /// <summary>
            /// 
            /// </summary>
            public uint Num;

            /// <summary>
            /// Initializes a new instance of the <see cref="AntiAliasedRasterizerCells&lt;Cell&gt;.SortedY"/> struct.
            /// </summary>
            /// <param name="s">The s.</param>
            /// <param name="n">The n.</param>
            public SortedY(uint s, uint n)
            {
                Start = s;
                Num = n;
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
        /// Adds the current cell.
        /// </summary>
        private void AddCurrentCell()
        {/*
            if ((_currentCell.Area | _currentCell.Cover) != 0)
            {
                if (((int)_numberOfCells & (int)CellBlockScale.CellBlockMask) == 0)
                {
                    if ((int)_num_blocks >= (int)CellBlockScale.CellBlockLimit)
                        return;
                    //AllocateBlock();
                }
                //*m_curr_cell_ptr++ = m_curr_cell;
                ++_numberOfCells;
            }*/
        }

        /// <summary>
        /// Sorts the cells.
        /// </summary>
        public void SortCells()
        {/*
            if (_sorted)
                return; //Perform sort only the first time.

            AddCurrentCell();
            _currentCell.X      = 0x7FFFFFFF;
            _currentCell.Y      = 0x7FFFFFFF;
            _currentCell.Cover  = 0;
            _currentCell.Area   = 0;

            if (_numberOfCells == 0)
                return;

            // Allocate the array of cell pointers
            _sortedCells.Capacity = (int)_numberOfCells;

            // Allocate and zero the Y array
            _sortedY.Capacity = _maxY - _minY + 1;
            _sortedY.Clear();

            // Create the Y-histogram (count the numbers of cells for each Y)
            int index = 0;
            int mainIndex = 0;
            Cell curCell;
            uint nb = (uint)((int)_numberOfCells >> (int)CellBlockScale.CellBlockShift);
            uint i;
            while (nb-- > 0)
            {
                curCell = _cells[mainIndex++];
                index = mainIndex;
                i = (uint)CellBlockScale.CellBlockSize;
                while (i-- > 0) 
                {
                    _sortedY[curCell.Y - _minY].Start++;
                    curCell = _cells[index++];
                }
            }

            curCell = _cells[mainIndex++];
            index = mainIndex;
            i = (uint)((int)_numberOfCells & (int)CellBlockScale.CellBlockMask);
            while (i-- > 0) 
            {
                _sortedY[curCell.Y - _minY].Start++;
                curCell = _cells[index++];
            }

            // Convert the Y-histogram into the array of starting indexes
            uint start = 0;
            for (i = 0; i < _sortedY.Count; i++)
            {
                uint v = _sortedY[(int)i].Start;
                _sortedY[(int)i].Start = start;
                start += v;
            }

            // Fill the cell pointer array sorted by Y
            mainIndex = index = 0;
            nb = (uint)((int)_numberOfCells >> (int)CellBlockScale.CellBlockShift);
            while (nb-- > 0)
            {
                curCell = _cells[mainIndex++];
                index = mainIndex;
                i = (uint)CellBlockScale.CellBlockSize;
                while(i-- > 0) 
                {
                    SortedY curr_y = _sortedY[curCell.Y - _minY];
                    _sortedCells[(int)curr_y.Start + (int)curr_y.Num] = curCell;
                    ++curr_y.Num;
                    curCell = _cells[index++];
                }
            }

            curCell = _cells[mainIndex++];
            index = mainIndex;
            i = (uint)((int)_numberOfCells & (int)CellBlockScale.CellBlockMask);
            while (i-- > 0) 
            {
                SortedY curr_y = _sortedY[curCell.Y - _minY];
                _sortedCells[(int)curr_y.Start + (int)curr_y.Num] = curCell;
                ++curr_y.Num;
                curCell = _cells[index++];
            }

            // Finally arrange the X-arrays
            for (i = 0; i < _sortedY.Count; i++)
            {
                SortedY curr_y = _sortedY[(int)i];
                if (curr_y.Num != 0)
                {
                    //QSortCells(_sortedCells.Data + curr_y.Start, curr_y.Num);
                }
            }
            _sorted = true;*/
        }
    }
}