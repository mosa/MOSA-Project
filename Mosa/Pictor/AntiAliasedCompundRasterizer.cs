/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using Pictor.VertexSource;

namespace Pictor
{
    ///<summary>
    ///</summary>
    public enum ELayerOrder
    {
        ///<summary>
        ///</summary>
        LayerUnsorted,
        ///<summary>
        ///</summary>
        LayerDirect,
        ///<summary>
        ///</summary>
        LayerInverse
    };


    /// <summary>
    /// 
    /// </summary>
    sealed public class AntiAliasedCompundRasterizer : IRasterizer
    {
        readonly AntiAliasedRasterizerCells _rasterizer;
        readonly IVectorClipper _vectorClipper;
        Basics.FillingRule _fillingRule;
        ELayerOrder _layerOrder;
        readonly VectorPOD<StyleInfo> _activeStyles;
        readonly VectorPOD<uint> _activeStyleTable;
        readonly VectorPOD<byte> _activeStyleMask;
        readonly VectorPOD<AntiAliasingCell> _cells;
        readonly VectorPOD<byte> _coverBuffer;
        readonly VectorPOD<uint> _masterAlpha;

        int _minStyle;
        int _maxStyle;
        int _startX;
        int _startY;
        int _scanY;
        int _scanlineStart;
        uint _scanlineLength;

        struct StyleInfo
        {
            internal uint StartCell;
            internal uint NumCells;
            internal int LastX;
        };

        private const int AaShift = 8;
        private const int AntiAliasingScale = 1 << AaShift;
        private const int AntiAliasingMask = AntiAliasingScale - 1;
        private const int AntiAliasingScale2 = AntiAliasingScale * 2;
        private const int AntiAliasingMask2 = AntiAliasingScale2 - 1;

        private const int PolygonSubpixelShift = (int)Basics.PolySubpixelScale.Shift;

        ///<summary>
        ///</summary>
        public AntiAliasedCompundRasterizer()
        {
            _rasterizer = new AntiAliasedRasterizerCells();
            _vectorClipper = new VectorClipper_DoClip();
            _fillingRule = Basics.FillingRule.NonZero;
            _layerOrder = ELayerOrder.LayerDirect;
            _activeStyles = new VectorPOD<StyleInfo>(); 
            _activeStyleTable = new VectorPOD<uint>();  
            _activeStyleMask = new VectorPOD<byte>();   
            _cells = new VectorPOD<AntiAliasingCell>();
            _coverBuffer = new VectorPOD<byte>();
            _masterAlpha = new VectorPOD<uint>();
            _minStyle = (0x7FFFFFFF);
            _maxStyle = (-0x7FFFFFFF);
            _startX = (0);
            _startY = (0);
            _scanY = (0x7FFFFFFF);
            _scanlineStart = (0);
            _scanlineLength = (0);
        }

        ///<summary>
        ///</summary>
        ///<exception cref="NotImplementedException"></exception>
        public IGammaFunction Gamma
        {
            set
            {
                throw new System.NotImplementedException();
            }
        }

        ///<summary>
        ///</summary>
        public void Reset()
        {
            _rasterizer.Reset();
            _minStyle = 0x7FFFFFFF;
            _maxStyle = -0x7FFFFFFF;
            _scanY = 0x7FFFFFFF;
            _scanlineStart = 0;
            _scanlineLength = 0;
        }

        Basics.FillingRule FillingRule
        {
            set
            {
                _fillingRule = value;
            }
        }

        ELayerOrder LayerOrder
        {
            set
            {
                _layerOrder = value;
            }
        }

        void ClipBox(double x1, double y1,
                                                    double x2, double y2)
        {
            Reset();
            _vectorClipper.ClipBox(_vectorClipper.UpScale(x1), _vectorClipper.UpScale(y1),
                               _vectorClipper.UpScale(x2), _vectorClipper.UpScale(y2));
        }

        void ResetClipping()
        {
            Reset();
            _vectorClipper.ResetClipping();
        }

        public void Styles(int left, int right)
        {
            AntiAliasingCell cell = new AntiAliasingCell();
            cell.Initial();
            cell.left = (short)left;
            cell.right = (short)right;
            _rasterizer.Style(cell);
            if (left >= 0 && left < _minStyle) _minStyle = left;
            if (left >= 0 && left > _maxStyle) _maxStyle = left;
            if (right >= 0 && right < _minStyle) _minStyle = right;
            if (right >= 0 && right > _maxStyle) _maxStyle = right;
        }

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        public void MoveTo(int x, int y)
        {
            if (_rasterizer.IsSorted) Reset();
            _vectorClipper.MoveTo(_startX = _vectorClipper.DownScale(x),
                              _startY = _vectorClipper.DownScale(y));
        }

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        public void LineTo(int x, int y)
        {
            _vectorClipper.LineTo(_rasterizer,
                              _vectorClipper.DownScale(x),
                              _vectorClipper.DownScale(y));
        }

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        public void MoveToD(double x, double y)
        {
            if (_rasterizer.IsSorted) Reset();
            _vectorClipper.MoveTo(_startX = _vectorClipper.UpScale(x),
                              _startY = _vectorClipper.UpScale(y));
        }

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        public void LineToD(double x, double y)
        {
            _vectorClipper.LineTo(_rasterizer,
                              _vectorClipper.UpScale(x),
                              _vectorClipper.UpScale(y));
        }

        void AddVertex(double x, double y, uint cmd)
        {
            if (Path.IsMoveTo(cmd))
            {
                MoveToD(x, y);
            }
            else if (Path.IsVertex(cmd))
            {
                LineToD(x, y);
            }
            else if (Path.IsClose(cmd))
            {
                _vectorClipper.LineTo(_rasterizer, _startX, _startY);
            }
        }

        void Edge(int x1, int y1, int x2, int y2)
        {
            if (_rasterizer.IsSorted) Reset();
            _vectorClipper.MoveTo(_vectorClipper.DownScale(x1), _vectorClipper.DownScale(y1));
            _vectorClipper.LineTo(_rasterizer,
                              _vectorClipper.DownScale(x2),
                              _vectorClipper.DownScale(y2));
        }

        void EdgeD(double x1, double y1,
                                                  double x2, double y2)
        {
            if (_rasterizer.IsSorted) Reset();
            _vectorClipper.MoveTo(_vectorClipper.UpScale(x1), _vectorClipper.UpScale(y1));
            _vectorClipper.LineTo(_rasterizer,
                              _vectorClipper.UpScale(x2),
                              _vectorClipper.UpScale(y2));
        }

        void Sort()
        {
            _rasterizer.SortCells();
        }

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public bool RewindScanlines()
        {
            _rasterizer.SortCells();
            if (_rasterizer.TotalCells == 0)
            {
                return false;
            }
            if (_maxStyle < _minStyle)
            {
                return false;
            }
            _scanY = _rasterizer.MinY();
            _activeStyles.Allocate((uint)(_maxStyle - _minStyle + 2), 128);
            AllocateMasterAlpha();
            return true;
        }

        ///<summary>
        ///</summary>
        ///<returns></returns>
        ///<exception cref="NotImplementedException"></exception>
        public uint SweepStyles()
        {
            for (; ; )
            {
                if (_scanY > _rasterizer.MaxY()) return 0;
                int numCells = (int)_rasterizer.ScanlineNumCells((uint)_scanY);
                AntiAliasingCell[] cells;
                uint cellOffset;
                _rasterizer.ScanlineCells((uint)_scanY, out cells, out cellOffset);
                uint numStyles = (uint)(_maxStyle - _minStyle + 2);
                int styleOffset = 0;

                _cells.Allocate((uint)numCells * 2, 256); // Each cell can have two Styles
                _activeStyleTable.Capacity(numStyles, 64);
                _activeStyleMask.Allocate((numStyles + 7) >> 3, 8);
                _activeStyleMask.Zero();

                if (numCells > 0)
                {
                    // Pre-Add zero (for no-fill Style, that is, -1).
                    // We need that to ensure that the "-1 Style" would go first.
                    _activeStyleMask.Array[0] |= 1;
                    _activeStyleTable.Add(0);
                    _activeStyles.Array[styleOffset].StartCell = 0;
                    _activeStyles.Array[styleOffset].NumCells = 0;
                    _activeStyles.Array[styleOffset].LastX = -0x7FFFFFFF;

                    _scanlineStart = cells[0].x;
                    _scanlineLength = (uint)(cells[numCells - 1].x - _scanlineStart + 1);
                    int curCellOffset;
                    while (numCells-- != 0)
                    {
                        curCellOffset = (int)cellOffset++;
                        AddStyle(cells[curCellOffset].left);
                        AddStyle(cells[curCellOffset].right);
                    }

                    // Convert the Y-histogram into the array of starting indexes
                    uint i = 0u;
                    uint startCell = 0;
                    StyleInfo[] stylesArray = _activeStyles.Array;
                    int indexToModify = (int)_activeStyleTable[i];
                    for (i = 0; i < _activeStyleTable.Size(); i++)
                    {
                        uint v = stylesArray[indexToModify].StartCell;
                        stylesArray[indexToModify].StartCell = startCell;
                        startCell += v;
                    }

                    numCells = (int)_rasterizer.ScanlineNumCells((uint)_scanY);
                    _rasterizer.ScanlineCells((uint)_scanY, out cells, out cellOffset);

                    while (numCells-- > 0)
                    {
                        curCellOffset = (int)cellOffset;
                        uint styleId = (uint)((cells[curCellOffset].left < 0) ? 0 :
                                                                                       cells[curCellOffset].left - _minStyle + 1);

                        styleOffset = (int)styleId;
                        if (cells[curCellOffset].x == stylesArray[styleOffset].LastX)
                        {
                            cellOffset = stylesArray[styleOffset].StartCell + stylesArray[styleOffset].NumCells - 1;
                            unchecked
                            {
                                cells[cellOffset].area += cells[curCellOffset].area;
                                cells[cellOffset].cover += cells[curCellOffset].cover;
                            }
                        }
                        else
                        {
                            cellOffset = stylesArray[styleOffset].StartCell + stylesArray[styleOffset].NumCells;
                            cells[cellOffset].x = cells[curCellOffset].x;
                            cells[cellOffset].area = cells[curCellOffset].area;
                            cells[cellOffset].cover = cells[curCellOffset].cover;
                            stylesArray[styleOffset].LastX = cells[curCellOffset].x;
                            stylesArray[styleOffset].NumCells++;
                        }

                        styleId = (uint)((cells[curCellOffset].right < 0) ? 0 :
                                    cells[curCellOffset].right - _minStyle + 1);

                        styleOffset = (int)styleId;
                        if (cells[curCellOffset].x == stylesArray[styleOffset].LastX)
                        {
                            cellOffset = stylesArray[styleOffset].StartCell + stylesArray[styleOffset].NumCells - 1;
                            unchecked
                            {
                                cells[cellOffset].area -= cells[curCellOffset].area;
                                cells[cellOffset].cover -= cells[curCellOffset].cover;
                            }
                        }
                        else
                        {
                            cellOffset = stylesArray[styleOffset].StartCell + stylesArray[styleOffset].NumCells;
                            cells[cellOffset].x = cells[curCellOffset].x;
                            cells[cellOffset].area = -cells[curCellOffset].area;
                            cells[cellOffset].cover = -cells[curCellOffset].cover;
                            stylesArray[styleOffset].LastX = cells[curCellOffset].x;
                            stylesArray[styleOffset].NumCells++;
                        }
                    }
                }
                if (_activeStyleTable.Size() > 1) break;
                ++_scanY;
            }
            ++_scanY;

            if (_layerOrder != ELayerOrder.LayerUnsorted)
            {
                VectorPOD_RangeAdaptor ra = new VectorPOD_RangeAdaptor(_activeStyleTable, 1, _activeStyleTable.Size() - 1);
                if (_layerOrder == ELayerOrder.LayerDirect)
                {
                    QuickSortRangeAdaptorUint mQSorter = new QuickSortRangeAdaptorUint();
                    mQSorter.Sort(ra);
                    //quick_sort(ra, uint_greater);
                }
                else
                {
                    throw new NotImplementedException();
                    //QuickSortRangeAdaptorUint m_QSorter = new QuickSortRangeAdaptorUint();
                    //m_QSorter.Sort(ra);
                    //quick_sort(ra, uint_less);
                }
            }

            return _activeStyleTable.Size() - 1;
        }

        // Returns Style ID depending of the existing Style index
        ///<summary>
        ///</summary>
        ///<param name="styleIndex"></param>
        ///<returns></returns>
        public uint Style(uint styleIndex)
        {
            return _activeStyleTable[styleIndex + 1] + (uint)_minStyle - 1;
        }

        bool NavigateScanline(int y)
        {
            _rasterizer.SortCells();
            if (_rasterizer.TotalCells == 0)
            {
                return false;
            }
            if (_maxStyle < _minStyle)
            {
                return false;
            }
            if (y < _rasterizer.MinY() || y > _rasterizer.MaxY())
            {
                return false;
            }
            _scanY = y;
            _activeStyles.Allocate((uint)(_maxStyle - _minStyle + 2), 128);
            AllocateMasterAlpha();
            return true;
        }

        bool HitTest(int tx, int ty)
        {
            if (!NavigateScanline(ty))
            {
                return false;
            }

            uint numStyles = SweepStyles();
            if (numStyles <= 0)
            {
                return false;
            }

            ScanlineHitTest sl = new ScanlineHitTest(tx);
            SweepScanline(sl, -1);
            return sl.hit();
        }

        byte[] AllocateCoverBuffer(uint len)
        {
            _coverBuffer.Allocate(len, 256);
            return _coverBuffer.Array;
        }

        void MasterAlpha(int style, double alpha)
        {
            if (style >= 0)
            {
                while ((int)_masterAlpha.Size() <= style)
                {
                    _masterAlpha.Add(AntiAliasingMask);
                }
                _masterAlpha.Array[style] = Basics.UnsignedRound(alpha * AntiAliasingMask);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="vs"></param>
        public void AddPath(IVertexSource vs)
        {
            AddPath(vs, 0);
        }

        ///<summary>
        ///</summary>
        ///<param name="vs"></param>
        ///<param name="pathId"></param>
        public void AddPath(IVertexSource vs, uint pathId)
        {
            double x;
            double y;

            uint cmd;
            vs.Rewind(pathId);
            if (_rasterizer.IsSorted) Reset();
            while (!Path.IsStop(cmd = vs.Vertex(out x, out y)))
            {
                AddVertex(x, y, cmd);
            }
        }

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public int MinX() { return _rasterizer.MinX(); }
        ///<summary>
        ///</summary>
        ///<returns></returns>
        public int MinY() { return _rasterizer.MinY(); }
        ///<summary>
        ///</summary>
        ///<returns></returns>
        public int MaxX() { return _rasterizer.MaxX(); }
        ///<summary>
        ///</summary>
        ///<returns></returns>
        public int MaxY() { return _rasterizer.MaxY(); }
        ///<summary>
        ///</summary>
        ///<returns></returns>
        public int MinStyle() { return _minStyle; }
        ///<summary>
        ///</summary>
        ///<returns></returns>
        public int MaxStyle() { return _maxStyle; }

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public int ScanlineStart() { return _scanlineStart; }
        ///<summary>
        ///</summary>
        ///<returns></returns>
        public uint ScanlineLength() { return _scanlineLength; }

        ///<summary>
        ///</summary>
        ///<param name="area"></param>
        ///<returns></returns>
        public uint CalculateAlpha(int area)
        {
            return CalculateAlpha(area, 0u);
        }

        ///<summary>
        ///</summary>
        ///<param name="area"></param>
        ///<param name="masterAlpha"></param>
        ///<returns></returns>
        public uint CalculateAlpha(int area, uint masterAlpha)
        {
            int cover = area >> (PolygonSubpixelShift * 2 + 1 - AaShift);
            if (cover < 0) cover = -cover;
            if (_fillingRule == Basics.FillingRule.EvenOdd)
            {
                cover &= AntiAliasingMask2;
                if (cover > AntiAliasingScale)
                {
                    cover = AntiAliasingScale2 - cover;
                }
            }
            if (cover > AntiAliasingMask) cover = AntiAliasingMask;
            return (uint)((cover * masterAlpha + AntiAliasingMask) >> AaShift);
        }

        ///<summary>
        ///</summary>
        ///<param name="sl"></param>
        ///<returns></returns>
        ///<exception cref="NotImplementedException"></exception>
        public bool SweepScanline(IScanline sl)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///</summary>
        ///<param name="sl"></param>
        ///<param name="styleIdx"></param>
        ///<returns></returns>
        public bool SweepScanline(IScanline sl, int styleIdx)
        {
            int scanY = _scanY - 1;
            if (scanY > _rasterizer.MaxY()) return false;

            sl.ResetSpans();

            uint masterAlpha = AntiAliasingMask;

            if (styleIdx < 0)
            {
                styleIdx = 0;
            }
            else
            {
                styleIdx++;
                masterAlpha = _masterAlpha[(uint)(_activeStyleTable[(uint)styleIdx] + _minStyle - 1)];
            }

            StyleInfo st = _activeStyles[_activeStyleTable[styleIdx]];

            int numCells = (int)st.NumCells;
            uint cellOffset = st.StartCell;
            AntiAliasingCell cell = _cells[cellOffset];

            int cover = 0;
            while (numCells-- != 0)
            {
                uint alpha;
                int x = cell.x;
                int area = cell.area;

                cover += cell.cover;

                cell = _cells[++cellOffset];

                if (area != 0)
                {
                    alpha = CalculateAlpha((cover << (PolygonSubpixelShift + 1)) - area,
                                            masterAlpha);
                    sl.AddCell(x, alpha);
                    x++;
                }

                if (numCells == 0 || cell.x <= x) 
                    continue;

                alpha = CalculateAlpha(cover << (PolygonSubpixelShift + 1),
                                       masterAlpha);
                if (alpha != 0)
                {
                    sl.AddSpan(x, cell.x - x, alpha);
                }
            }

            if (sl.NumberOfSpans == 0) return false;
            sl.Finalize(scanY);
            return true;
        }

        private void AddStyle(int styleId)
        {
            if (styleId < 0) styleId = 0;
            else styleId -= _minStyle - 1;

            uint nbyte = (uint)((int)styleId >> 3);
            uint mask = (uint)(1 << (styleId & 7));

            StyleInfo[] stylesArray = _activeStyles.Array;
            if ((_activeStyleMask[nbyte] & mask) == 0)
            {
                _activeStyleTable.Add((uint)styleId);
                _activeStyleMask.Array[nbyte] |= (byte)mask;
                stylesArray[styleId].StartCell = 0;
                stylesArray[styleId].NumCells = 0;
                stylesArray[styleId].LastX = -0x7FFFFFFF;
            }
            ++stylesArray[styleId].StartCell;
        }

        private void AllocateMasterAlpha()
        {
            while ((int)_masterAlpha.Size() <= _maxStyle)
            {
                _masterAlpha.Add(AntiAliasingMask);
            }
        }
    };
}
