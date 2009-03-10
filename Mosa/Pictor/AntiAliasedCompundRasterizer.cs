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
    //===========================================================ELayerOrder
    public enum ELayerOrder
    {
        layer_unsorted, //------layer_unsorted
        layer_direct,   //------layer_direct
        layer_inverse   //------layer_inverse
    };


    //==================================================AntiAliasedCompundRasterizer
    //template<class Clip=rasterizer_sl_clip_int> 
    sealed public class AntiAliasedCompundRasterizer : IRasterizer
    {
        AntiAliasedRasterizerCells m_Rasterizer;
        IVectorClipper              m_VectorClipper;
        Basics.FillingRule         m_filling_rule;
        ELayerOrder          m_layer_order;
        VectorPOD<StyleInfo> m_styles;  // Active Styles
        VectorPOD<uint>   m_ast;     // Active Style Table (unique values)
        VectorPOD<byte>      m_asm;     // Active Style Mask 
        VectorPOD<AntiAliasingCell>  m_cells;
        VectorPOD<byte> m_cover_buf;
        VectorPOD<uint>  m_master_alpha;

        int        m_min_style;
        int        m_max_style;
        int m_start_x;
        int m_start_y;
        int        m_scan_y;
        int        m_sl_start;
        uint   m_sl_len;

        struct StyleInfo 
        { 
            internal uint start_cell;
            internal uint num_cells;
            internal int last_x;
        };

        private const int aa_shift  = 8;
        private const int aa_scale  = 1 << aa_shift;
        private const int aa_mask   = aa_scale - 1;
        private const int aa_scale2 = aa_scale * 2;
        private const int aa_mask2 = aa_scale2 - 1;

        private const int poly_subpixel_shift = (int)Basics.PolySubpixelScale.Shift;

        public AntiAliasedCompundRasterizer()
        {
            m_Rasterizer = new AntiAliasedRasterizerCells();
            m_VectorClipper = new VectorClipper_DoClip();
            m_filling_rule = Basics.FillingRule.NonZero;
            m_layer_order = ELayerOrder.layer_direct;
            m_styles = new VectorPOD<StyleInfo>();  // Active Styles
            m_ast = new VectorPOD<uint>();     // Active Style Table (unique values)
            m_asm = new VectorPOD<byte>();     // Active Style Mask 
            m_cells = new VectorPOD<AntiAliasingCell>();
            m_cover_buf = new VectorPOD<byte>();
            m_master_alpha = new VectorPOD<uint>();
            m_min_style = (0x7FFFFFFF);
            m_max_style=(-0x7FFFFFFF);
            m_start_x=(0);
            m_start_y=(0);
            m_scan_y=(0x7FFFFFFF);
            m_sl_start=(0);
            m_sl_len=(0);
        }

        public IGammaFunction Gamma
        {
            set
            {
                throw new System.NotImplementedException();
            }
        }


        public void Reset() 
        { 
            m_Rasterizer.Reset(); 
            m_min_style =  0x7FFFFFFF;
            m_max_style = -0x7FFFFFFF;
            m_scan_y    =  0x7FFFFFFF;
            m_sl_start  =  0;
            m_sl_len    = 0;
        }

        Basics.FillingRule FillingRule
        {
            set
            {
                m_filling_rule = value;
            }
        }

        ELayerOrder LayerOrder
        {
            set 
            {
                m_layer_order = value; 
            }
        }

        void ClipBox(double x1, double y1, 
                                                    double x2, double y2)
        {
            Reset();
            m_VectorClipper.ClipBox(m_VectorClipper.UpScale(x1), m_VectorClipper.UpScale(y1),
                               m_VectorClipper.UpScale(x2), m_VectorClipper.UpScale(y2));
        }

        void ResetClipping()
        {
            Reset();
            m_VectorClipper.ResetClipping();
        }

        public void Styles(int left, int right)
        {
            AntiAliasingCell cell = new AntiAliasingCell();
            cell.Initial();
            cell.left = (short)left;
            cell.right = (short)right;
            m_Rasterizer.Style(cell);
            if(left  >= 0 && left  < m_min_style) m_min_style = left;
            if(left  >= 0 && left  > m_max_style) m_max_style = left;
            if(right >= 0 && right < m_min_style) m_min_style = right;
            if(right >= 0 && right > m_max_style) m_max_style = right;
        }

        public void MoveTo(int x, int y)
        {
            if(m_Rasterizer.IsSorted) Reset();
            m_VectorClipper.MoveTo(m_start_x = m_VectorClipper.DownScale(x),
                              m_start_y = m_VectorClipper.DownScale(y));
        }

        public void LineTo(int x, int y)
        {
            m_VectorClipper.LineTo(m_Rasterizer, 
                              m_VectorClipper.DownScale(x),
                              m_VectorClipper.DownScale(y));
        }

        public void MoveToD(double x, double y) 
        { 
            if(m_Rasterizer.IsSorted) Reset();
            m_VectorClipper.MoveTo(m_start_x = m_VectorClipper.UpScale(x),
                              m_start_y = m_VectorClipper.UpScale(y)); 
        }

        public void LineToD(double x, double y) 
        { 
            m_VectorClipper.LineTo(m_Rasterizer, 
                              m_VectorClipper.UpScale(x),
                              m_VectorClipper.UpScale(y)); 
        }

        void AddVertex(double x, double y, uint cmd)
        {
            if(Path.IsMoveTo(cmd)) 
            {
                MoveToD(x, y);
            }
            else 
            if(Path.IsVertex(cmd))
            {
                LineToD(x, y);
            }
            else
            if(Path.IsClose(cmd))
            {
                m_VectorClipper.LineTo(m_Rasterizer, m_start_x, m_start_y);
            }
        }

        void Edge(int x1, int y1, int x2, int y2)
        {
            if(m_Rasterizer.IsSorted) Reset();
            m_VectorClipper.MoveTo(m_VectorClipper.DownScale(x1), m_VectorClipper.DownScale(y1));
            m_VectorClipper.LineTo(m_Rasterizer, 
                              m_VectorClipper.DownScale(x2),
                              m_VectorClipper.DownScale(y2));
        }
        
        void EdgeD(double x1, double y1, 
                                                  double x2, double y2)
        {
            if(m_Rasterizer.IsSorted) Reset();
            m_VectorClipper.MoveTo(m_VectorClipper.UpScale(x1), m_VectorClipper.UpScale(y1)); 
            m_VectorClipper.LineTo(m_Rasterizer, 
                              m_VectorClipper.UpScale(x2),
                              m_VectorClipper.UpScale(y2)); 
        }

        void Sort()
        {
            m_Rasterizer.SortCells();
        }

        public bool RewindScanlines()
        {
            m_Rasterizer.SortCells();
            if(m_Rasterizer.TotalCells == 0) 
            {
                return false;
            }
            if(m_max_style < m_min_style)
            {
                return false;
            }
            m_scan_y = m_Rasterizer.MinY();
            m_styles.Allocate((uint)(m_max_style - m_min_style + 2), 128);
            allocate_master_alpha();
            return true;
        }

        // Returns the number of Styles
        public uint SweepStyles()
        {
            for(;;)
            {
                if(m_scan_y > m_Rasterizer.MaxY()) return 0;
                int num_cells = (int)m_Rasterizer.ScanlineNumCells((uint)m_scan_y);
                AntiAliasingCell[] cells;
                uint cellOffset = 0;
                int curCellOffset;
                m_Rasterizer.ScanlineCells((uint)m_scan_y, out cells, out cellOffset);
                uint num_styles = (uint)(m_max_style - m_min_style + 2);
                uint style_id;
                int styleOffset = 0;

                m_cells.Allocate((uint)num_cells * 2, 256); // Each cell can have two Styles
                m_ast.Capacity(num_styles, 64);
                m_asm.Allocate((num_styles + 7) >> 3, 8);
                m_asm.Zero();

                if(num_cells > 0)
                {
                    // Pre-Add zero (for no-fill Style, that is, -1).
                    // We need that to ensure that the "-1 Style" would go first.
                    m_asm.Array[0] |= 1; 
                    m_ast.Add(0);
                    m_styles.Array[styleOffset].start_cell = 0;
                    m_styles.Array[styleOffset].num_cells = 0;
                    m_styles.Array[styleOffset].last_x = -0x7FFFFFFF;

                    m_sl_start = cells[0].x;
                    m_sl_len   = (uint)(cells[num_cells-1].x - m_sl_start + 1);
                    while(num_cells-- != 0)
                    {
                        curCellOffset = (int)cellOffset++;
                        AddStyle(cells[curCellOffset].left);
                        AddStyle(cells[curCellOffset].right);
                    }

                    // Convert the Y-histogram into the array of starting indexes
                    uint i;
                    uint start_cell = 0;
                    StyleInfo[] stylesArray = m_styles.Array;
                    for(i = 0; i < m_ast.Size(); i++)
                    {
                        int IndexToModify = (int)m_ast[i];
                        uint v = stylesArray[IndexToModify].start_cell;
                        stylesArray[IndexToModify].start_cell = start_cell;
                        start_cell += v;
                    }

                    num_cells = (int)m_Rasterizer.ScanlineNumCells((uint)m_scan_y);
                    m_Rasterizer.ScanlineCells((uint)m_scan_y, out cells, out cellOffset);

                    while(num_cells-- > 0)
                    {
                        curCellOffset = (int)cellOffset++;
                        style_id = (uint)((cells[curCellOffset].left < 0) ? 0 :
                                    cells[curCellOffset].left - m_min_style + 1);

                        styleOffset = (int)style_id;
                        if (cells[curCellOffset].x == stylesArray[styleOffset].last_x)
                        {
                            cellOffset = stylesArray[styleOffset].start_cell + stylesArray[styleOffset].num_cells - 1;
                            unchecked
                            {
                                cells[cellOffset].area += cells[curCellOffset].area;
                                cells[cellOffset].cover += cells[curCellOffset].cover;
                            }
                        }
                        else
                        {
                            cellOffset = stylesArray[styleOffset].start_cell + stylesArray[styleOffset].num_cells;
                            cells[cellOffset].x = cells[curCellOffset].x;
                            cells[cellOffset].area = cells[curCellOffset].area;
                            cells[cellOffset].cover = cells[curCellOffset].cover;
                            stylesArray[styleOffset].last_x = cells[curCellOffset].x;
                            stylesArray[styleOffset].num_cells++;
                        }

                        style_id = (uint)((cells[curCellOffset].right < 0) ? 0 :
                                    cells[curCellOffset].right - m_min_style + 1);

                        styleOffset = (int)style_id;
                        if (cells[curCellOffset].x == stylesArray[styleOffset].last_x)
                        {
                            cellOffset = stylesArray[styleOffset].start_cell + stylesArray[styleOffset].num_cells - 1;
                            unchecked
                            {
                                cells[cellOffset].area -= cells[curCellOffset].area;
                                cells[cellOffset].cover -= cells[curCellOffset].cover;
                            }
                        }
                        else
                        {
                            cellOffset = stylesArray[styleOffset].start_cell + stylesArray[styleOffset].num_cells;
                            cells[cellOffset].x = cells[curCellOffset].x;
                            cells[cellOffset].area = -cells[curCellOffset].area;
                            cells[cellOffset].cover = -cells[curCellOffset].cover;
                            stylesArray[styleOffset].last_x = cells[curCellOffset].x;
                            stylesArray[styleOffset].num_cells++;
                        }
                    }
                }
                if(m_ast.Size() > 1) break;
                ++m_scan_y;
            }
            ++m_scan_y;

            if (m_layer_order != ELayerOrder.layer_unsorted)
            {
                VectorPOD_RangeAdaptor ra = new VectorPOD_RangeAdaptor(m_ast, 1, m_ast.Size() - 1);
                if (m_layer_order == ELayerOrder.layer_direct)
                {
                    QuickSortRangeAdaptorUint m_QSorter = new QuickSortRangeAdaptorUint();
                    m_QSorter.Sort(ra);
                    //quick_sort(ra, uint_greater);
                }
                else
                {
                    throw new System.NotImplementedException();
                    //QuickSortRangeAdaptorUint m_QSorter = new QuickSortRangeAdaptorUint();
                    //m_QSorter.Sort(ra);
                    //quick_sort(ra, uint_less);
                }
            }

            return m_ast.Size() - 1;
        }

        // Returns Style ID depending of the existing Style index
        public uint Style(uint style_idx)
        {
            return m_ast[style_idx + 1] + (uint)m_min_style - 1;
        }

        bool NavigateScanline(int y)
        {
            m_Rasterizer.SortCells();
            if(m_Rasterizer.TotalCells == 0) 
            {
                return false;
            }
            if(m_max_style < m_min_style)
            {
                return false;
            }
            if(y < m_Rasterizer.MinY() || y > m_Rasterizer.MaxY()) 
            {
                return false;
            }
            m_scan_y = y;
            m_styles.Allocate((uint)(m_max_style - m_min_style + 2), 128);
            allocate_master_alpha();
            return true;
        }
        
        bool HitTest(int tx, int ty)
        {
            if(!NavigateScanline(ty)) 
            {
                return false;
            }

            uint num_styles = SweepStyles(); 
            if(num_styles <= 0)
            {
                return false;
            }

            ScanlineHitTest sl = new ScanlineHitTest(tx);
            SweepScanline(sl, -1);
            return sl.hit();
        }

        byte[] AllocateCoverBuffer(uint len)
        {
            m_cover_buf.Allocate(len, 256);
            return m_cover_buf.Array;
        }

        void MasterAlpha(int style, double alpha)
        {
            if(style >= 0)
            {
                while((int)m_master_alpha.Size() <= style)
                {
                    m_master_alpha.Add(aa_mask);
                }
                m_master_alpha.Array[style] = Basics.UnsignedRound(alpha * aa_mask);
            }
        }

        public void AddPath(IVertexSource vs)
        {
            AddPath(vs, 0);
        }

        public void AddPath(IVertexSource vs, uint path_id)
        {
            double x;
            double y;

            uint cmd;
            vs.Rewind(path_id);
            if(m_Rasterizer.IsSorted) Reset();
            while(!Path.IsStop(cmd = vs.Vertex(out x, out y)))
            {
                AddVertex(x, y, cmd);
            }
        }

        public int MinX()     { return m_Rasterizer.MinX(); }
        public int MinY() { return m_Rasterizer.MinY(); }
        public int MaxX() { return m_Rasterizer.MaxX(); }
        public int MaxY() { return m_Rasterizer.MaxY(); }
        public int MinStyle() { return m_min_style; }
        public int MaxStyle() { return m_max_style; }

        public int ScanlineStart() { return m_sl_start; }
        public uint ScanlineLength() { return m_sl_len; }

        public uint CalculateAlpha(int area, uint master_alpha)
        {
            int cover = area >> (poly_subpixel_shift*2 + 1 - aa_shift);
            if(cover < 0) cover = -cover;
            if (m_filling_rule == Basics.FillingRule.EvenOdd)
            {
                cover &= aa_mask2;
                if(cover > aa_scale)
                {
                    cover = aa_scale2 - cover;
                }
            }
            if(cover > aa_mask) cover = aa_mask;
            return (uint)((cover * master_alpha + aa_mask) >> aa_shift);
        }

        public bool SweepScanline(IScanline sl)
        {
            throw new System.NotImplementedException();
        }

        // Sweeps one scanline with one Style index. The Style ID can be 
        // determined by calling Style(). 
        //template<class Scanline> 
        public bool SweepScanline(IScanline sl, int style_idx)
        {
            int scan_y = m_scan_y - 1;
            if(scan_y > m_Rasterizer.MaxY()) return false;

            sl.ResetSpans();

            uint master_alpha = aa_mask;

            if(style_idx < 0) 
            {
                style_idx = 0;
            }
            else 
            {
                style_idx++;
                master_alpha = m_master_alpha[(uint)(m_ast[(uint)style_idx] + m_min_style - 1)];
            }

            StyleInfo st = m_styles[m_ast[style_idx]];

            int num_cells = (int)st.num_cells;
            uint CellOffset = st.start_cell;
            AntiAliasingCell cell = m_cells[CellOffset];

            int cover = 0;
            while(num_cells-- != 0)
            {
                uint alpha;
                int x = cell.x;
                int area = cell.area;

                cover += cell.cover;

                cell = m_cells[++CellOffset];

                if(area != 0)
                {
                    alpha = CalculateAlpha((cover << (poly_subpixel_shift + 1)) - area,
                                            master_alpha);
                    sl.AddCell(x, alpha);
                    x++;
                }

                if(num_cells != 0 && cell.x > x)
                {
                    alpha = CalculateAlpha(cover << (poly_subpixel_shift + 1),
                                            master_alpha);
                    if(alpha != 0)
                    {
                        sl.AddSpan(x, cell.x - x, alpha);
                    }
                }
            }

            if(sl.NumberOfSpans == 0) return false;
            sl.Finalize(scan_y);
            return true;
        }

        private void AddStyle(int style_id)
        {
            if(style_id < 0) style_id  = 0;
            else             style_id -= m_min_style - 1;

            uint nbyte = (uint)((int)style_id >> 3);
            uint mask = (uint)(1 << (style_id & 7));

            StyleInfo[] stylesArray = m_styles.Array;
            if((m_asm[nbyte] & mask) == 0)
            {
                m_ast.Add((uint)style_id);
                m_asm.Array[nbyte] |= (byte)mask;
                stylesArray[style_id].start_cell = 0;
                stylesArray[style_id].num_cells = 0;
                stylesArray[style_id].last_x = -0x7FFFFFFF;
            }
            ++stylesArray[style_id].start_cell;
        }

        private void allocate_master_alpha()
        {
            while((int)m_master_alpha.Size() <= m_max_style)
            {
                m_master_alpha.Add(aa_mask);
            }
        }
    };
}
