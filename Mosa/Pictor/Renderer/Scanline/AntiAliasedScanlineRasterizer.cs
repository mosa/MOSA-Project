/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */


namespace Pictor.Renderer.Scanline
{
    /// <summary>
    /// Polygon rasterizer that is used to render filled polygons with
    /// high-quality Anti-Aliasing. Internally, by default, the class uses
    /// integer coordinates in format 24.8, i.e. 24 bits for integer part
    /// and 8 bits for fractional - see PolySubpixelShift. This class can be
    /// used in the following  way:
    /// 1. FillingRule = rule - optional.
    /// 2. Gamma() - optional.
    /// 3. Reset()
    /// 4. MoveTo(x, y) / LineTo(x, y) - make the polygon. One can create
    /// more than one contour, but each contour must consist of at least 3
    /// vertices, i.e. move_to(x1, y1); line_to(x2, y2); line_to(x3, y3);
    /// is the absolute minimum of vertices that define a triangle.
    /// The algorithm does not check either the number of vertices nor
    /// coincidence of their coordinates, but in the worst case it just
    /// won't draw anything.
    /// The orger of the vertices (clockwise or counterclockwise)
    /// is important when using the non-zero filling rule (fill_non_zero).
    /// In this case the vertex order of all the contours must be the same
    /// if you want your intersecting polygons to be without "holes".
    /// You actually can use different vertices order. If the contours do not
    /// intersect each other the order is not important anyway. If they do,
    /// contours with the same vertex order will be rendered without "holes"
    /// while the intersecting contours with different orders will have "holes".
    /// FillingRule() and Gamma() can be called anytime before "sweeping".
    /// </summary>
    /// <typeparam name="Clip">The type of the lip.</typeparam>
    /// <typeparam name="CoordinateType">The type of the oordinate type.</typeparam>
    public class AntiAliasedScanlineRasterizer<Clip, CoordinateType> where Clip : IClipper<CoordinateType, AntiAliasedCell>
    {
        /// <summary>
        /// 
        /// </summary>
        private AntiAliasedRasterizerCells<AntiAliasedCell> _outline = null;
        /// <summary>
        /// 
        /// </summary>
        private Clip            _clipper = default(Clip);
        /// <summary>
        /// 
        /// </summary>
        private uint[]          _gamma = new uint[(int)AntiAliasedScale.AntiAliasedScale];
        /// <summary>
        /// 
        /// </summary>
        private FillingRule     _fillingRule;
        /// <summary>
        /// 
        /// </summary>
        private bool            _autoClose = true;
        /// <summary>
        /// 
        /// </summary>
        private CoordinateType  _startX = default(CoordinateType);
        /// <summary>
        /// 
        /// </summary>
        private CoordinateType  _startY = default(CoordinateType);
        /// <summary>
        /// 
        /// </summary>
        private Status          _status;
        /// <summary>
        /// 
        /// </summary>
        private int             _scanY;

        /// <summary>
        /// 
        /// </summary>
        enum Status
        {
            /// <summary>
            /// 
            /// </summary>
            StatusInitial,
            /// <summary>
            /// 
            /// </summary>
            StatusMoveTo,
            /// <summary>
            /// 
            /// </summary>
            StatusLineTo,
            /// <summary>
            /// 
            /// </summary>
            StatusClosed
        };

        /// <summary>
        /// 
        /// </summary>
        public enum AntiAliasedScale
        {
            /// <summary>
            /// 
            /// </summary>
            AntiAliasedShift    = 8,
            /// <summary>
            /// 
            /// </summary>
            AntiAliasedScale    = 1 << AntiAliasedShift,
            /// <summary>
            /// 
            /// </summary>
            AntiAliasedMask     = AntiAliasedScale - 1,
            /// <summary>
            /// 
            /// </summary>
            AntiAliasedScale2   = AntiAliasedScale * 2,
            /// <summary>
            /// 
            /// </summary>
            AntiAliasedMask2    = AntiAliasedScale2 - 1
        };

        /// <summary>
        /// Gets or sets the filling rule.
        /// </summary>
        /// <value>The filling rule.</value>
        public FillingRule FillingRule
        {
            get
            {
                return _fillingRule;
            }
            set
            {
                _fillingRule = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [auto close].
        /// </summary>
        /// <value><c>true</c> if [auto close]; otherwise, <c>false</c>.</value>
        public bool AutoClose
        {
            get
            {
                return _autoClose;
            }
            set
            {
                _autoClose = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AntiAliasedScanlineRasterizer&lt;Clip, CoordinateType&gt;"/> class.
        /// </summary>
        public AntiAliasedScanlineRasterizer()
        {
            for (int i = 0; i < (int)AntiAliasedScale.AntiAliasedScale; ++i) 
                _gamma[i] = (uint)i;
        }

        /// <summary>
        /// Applies the gamma.
        /// </summary>
        /// <param name="cover">The cover.</param>
        /// <returns></returns>
        uint ApplyGamma(uint cover) 
        { 
            return _gamma[cover]; 
        }

        /// <summary>
        /// Sweeps the scanline.
        /// </summary>
        /// <typeparam name="Scanline">The type of the canline.</typeparam>
        /// <param name="scanline">The scanline.</param>
        /// <returns></returns>
        public bool SweepScanline<Scanline>(Scanline scanline)
        {
            return true;
        }

        /// <summary>
        /// Closes the polygon.
        /// </summary>
        public void ClosePolygon()
        {
            if (_status == Status.StatusLineTo)
            {
                _clipper.LineTo(_outline, _startX, _startY);
                _status = Status.StatusClosed;
            }
        }

        /// <summary>
        /// Navigates the scanline.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        bool NavigateScanline(int y)
        {
            if (AutoClose)
                ClosePolygon();
            //_outline.sort_cells();
            /*if (m_outline.total_cells() == 0 || 
               y < m_outline.min_y() || 
               y > m_outline.max_y()) 
            {
                return false;
            }*/
            _scanY = y;
            return true;
        }
    }
}