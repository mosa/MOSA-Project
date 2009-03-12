/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;

using Pictor.PixelFormat;
using Pictor.VertexSource;
using Pictor.Transform;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStyleHandler
    {
        /// <summary>
        /// Determines whether the given stylee is solid or not
        /// </summary>
        /// <param name="Style"></param>
        /// <returns>True if it is solid</returns>
        bool IsSolid(uint style);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Style"></param>
        /// <returns></returns>
        RGBA_Bytes Color(uint style);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Span"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="len"></param>
        /// <param name="Style"></param>
        unsafe void GenerateSpan(RGBA_Bytes* span, int x, int y, uint len, uint style);
    };

    /// <summary>
    /// 
    /// </summary>
    public class Renderer
    {
        /// <summary>
        /// 
        /// </summary>
        const int cover_full = 255;

        /// <summary>
        /// 
        /// </summary>
        protected IPixelFormat m_PixelFormat;

        /// <summary>
        /// 
        /// </summary>
        protected AntiAliasedScanlineRasterizer m_Rasterizer;

        /// <summary>
        /// 
        /// </summary>
        protected IScanline m_Scanline;

        /// <summary>
        /// 
        /// </summary>
        protected GsvText TextPath;

        /// <summary>
        /// 
        /// </summary>
        protected StrokeConverter StrockedText;

        /// <summary>
        /// 
        /// </summary>
        protected Stack<Affine> m_AffineTransformStack = new Stack<Affine>();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PixelFormat"></param>
        /// <param name="Rasterizer"></param>
        /// <param name="Scanline"></param>
        public Renderer(IPixelFormat PixelFormat, AntiAliasedScanlineRasterizer Rasterizer, IScanline Scanline)
        {
        	m_PixelFormat = PixelFormat;
        	m_Rasterizer = Rasterizer;
        	m_Scanline = Scanline;

            TextPath = new GsvText();
            StrockedText = new StrokeConverter(TextPath);
            m_AffineTransformStack.Push(Affine.NewIdentity());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Affine PopTransform()
        {
            if (m_AffineTransformStack.Count == 1)
            {
                throw new System.Exception("You cannot remove the last Transform from the stack.");
            }

            return m_AffineTransformStack.Pop();
        }

        /// <summary>
        /// 
        /// </summary>
        public void PushTransform()
        {
            if (m_AffineTransformStack.Count > 1000)
            {
                throw new System.Exception("You seem to be leaking transforms.  You should be poping some of them At some point.");
            }

            m_AffineTransformStack.Push(m_AffineTransformStack.Peek());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Affine Transform
        {
            get { return m_AffineTransformStack.Peek(); }
            set
            {
                m_AffineTransformStack.Pop();
                m_AffineTransformStack.Push(value); 
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
		public AntiAliasedScanlineRasterizer Rasterizer
		{
			get
			{
				return m_Rasterizer;
			}
		}
		
        /// <summary>
        /// 
        /// </summary>
		public IScanline ScanlineCache
		{
			get
			{
				return m_Scanline;
			}
		}
		
        /// <summary>
        /// 
        /// </summary>
		public IPixelFormat PixelFormat
		{
			get
			{
				return m_PixelFormat;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertexSource"></param>
        /// <param name="idx"></param>
        /// <param name="color"></param>
        public void Render(IVertexSource vertexSource, uint idx, RGBA_Bytes color)
        {
            m_Rasterizer.Reset();
            Affine transform = Transform;
            if (!transform.IsIdentity())
            {
                vertexSource = new TransformationConverter(vertexSource, transform);
            }
            m_Rasterizer.AddPath(vertexSource, idx);
            Renderer.RenderSolid(m_PixelFormat, m_Rasterizer, m_Scanline, color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertexSource"></param>
        /// <param name="color"></param>
		public void Render(IVertexSource vertexSource, RGBA_Bytes color)
        {
            Render(vertexSource, 0, color);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        public void Clear(IColorType color)
        {
        	/*FormatClippingProxy clipper = (FormatClippingProxy)m_PixelFormat;
        	if(clipper != null)
        	{
        		clipper.Clear(color);
        	}*/
        }

        #region RenderSolid

#if use_timers
        static CNamedTimer PrepareTimer = new CNamedTimer("Prepare");
#endif
        //========================================================render_scanlines
        public static void RenderSolid(IPixelFormat pixFormat, IRasterizer rasterizer, IScanline scanLine, RGBA_Bytes color)
        {
            if(rasterizer.RewindScanlines())
            {
                scanLine.Reset(rasterizer.MinX(), rasterizer.MaxX());
#if use_timers
                PrepareTimer.Start();
#endif
                //renderer.Prepare();
#if use_timers
                PrepareTimer.Stop();
#endif
                while(rasterizer.SweepScanline(scanLine))
                {
                    Renderer.RenderSolidSingleScanLine(pixFormat, scanLine, color);
                }
            }
        }
#endregion

        #region RenderSolidSingleScanLine
#if use_timers
        static CNamedTimer render_scanline_aa_solidTimer = new CNamedTimer("render_scanline_aa_solid");
        static CNamedTimer render_scanline_aa_solid_blend_solid_hspan = new CNamedTimer("render_scanline_aa_solid_blend_solid_hspan");
        static CNamedTimer render_scanline_aa_solid_blend_hline = new CNamedTimer("render_scanline_aa_solid_blend_hline");
#endif
        //================================================render_scanline_aa_solid
        private static void RenderSolidSingleScanLine(IPixelFormat pixFormat, IScanline scanLine, RGBA_Bytes color)
        {
#if use_timers
            render_scanline_aa_solidTimer.Start();
#endif
            int y = scanLine.y();
            uint num_spans = scanLine.NumberOfSpans;
            ScanlineSpan scanlineSpan = scanLine.Begin;

            byte[] ManagedCoversArray = scanLine.GetCovers();
            unsafe
            {
                fixed (byte* pCovers = ManagedCoversArray)
                {
                    for (; ; )
                    {
                        int x = scanlineSpan.x;
                        if (scanlineSpan.len > 0)
                        {
#if use_timers
                            render_scanline_aa_solid_blend_solid_hspan.Start();
#endif
                            pixFormat.BlendSolidHorizontalSpan(x, y, (uint)scanlineSpan.len, color, &pCovers[scanlineSpan.cover_index]);
#if use_timers
                            render_scanline_aa_solid_blend_solid_hspan.Stop();
#endif
                        }
                        else
                        {
#if use_timers
                            render_scanline_aa_solid_blend_hline.Start();
#endif
                            pixFormat.BlendHorizontalLine(x, y, (x - (int)scanlineSpan.len - 1), color, pCovers[scanlineSpan.cover_index]);
#if use_timers
                            render_scanline_aa_solid_blend_hline.Stop();
#endif
                        }
                        if (--num_spans == 0) break;
                        scanlineSpan = scanLine.GetNextScanlineSpan();
                    }
                }
            }
#if use_timers
            render_scanline_aa_solidTimer.Stop();
#endif
        }
#endregion

        #region RenderSolidAllPaths
#if use_timers
        static CNamedTimer AddPathTimer = new CNamedTimer("AddPath");
        static CNamedTimer RenderSLTimer = new CNamedTimer("RenderSLs");
#endif
        //========================================================render_all_paths
        public static void RenderSolidAllPaths(IPixelFormat pixFormat, 
            IRasterizer ras, 
            IScanline sl,
            IVertexSource vs, 
            RGBA_Bytes[] color_storage,
            uint[] path_id,
            uint num_paths)
        {
            for(uint i = 0; i < num_paths; i++)
            {
                ras.Reset();
                
#if use_timers
                AddPathTimer.Start();
#endif
                ras.AddPath(vs, path_id[i]);
#if use_timers
                AddPathTimer.Stop();
#endif

                
#if use_timers
                RenderSLTimer.Start();
#endif
                RenderSolid(pixFormat, ras, sl, color_storage[i]);
#if use_timers
                RenderSLTimer.Stop();
#endif
            }
        }
#endregion

        #region GenerateAndRenderSingleScanline
        static VectorPOD<RGBA_Bytes> tempSpanColors = new VectorPOD<RGBA_Bytes>();

#if use_timers
        static CNamedTimer blend_color_hspan = new CNamedTimer("blend_color_hspan");
#endif
        //======================================================render_scanline_aa
        private static void GenerateAndRenderSingleScanline(IScanline sl, IPixelFormat ren,
                                SpanAllocator alloc, ISpanGenerator span_gen)
        {
            int y = sl.y();
            uint num_spans = sl.NumberOfSpans;
            ScanlineSpan scanlineSpan = sl.Begin;

            byte[] ManagedCoversArray = sl.GetCovers();
            unsafe
            {
                fixed (byte* pCovers = ManagedCoversArray)
                {
                    for (; ; )
                    {
                        int x = scanlineSpan.x;
                        int len = scanlineSpan.len;
                        if(len < 0) len = -len;

                        if(tempSpanColors.Capacity() < len)
                        {
                            tempSpanColors.Allocate((uint)(len));
                        }

                        fixed (RGBA_Bytes* pColors = tempSpanColors.Array)
                        {
                            span_gen.Generate(pColors, x, y, (uint)len);
#if use_timers
                            blend_color_hspan.Start();
#endif
                            ren.BlendHorizontalColorSpan(x, y, (uint)len, pColors, (scanlineSpan.len < 0) ? null : &pCovers[scanlineSpan.cover_index], pCovers[scanlineSpan.cover_index]);
#if use_timers
                            blend_color_hspan.Stop();
#endif
                        }

                        if (--num_spans == 0) break;
                        scanlineSpan = sl.GetNextScanlineSpan();
                    }
                }
            }
        }
        #endregion

        #region GenerateAndRender
        //=====================================================render_scanlines_aa
        public static void GenerateAndRender(IRasterizer ras, IScanline sl, IPixelFormat ren,
                                 SpanAllocator alloc, ISpanGenerator span_gen)
        {
            if(ras.RewindScanlines())
            {
                sl.Reset(ras.MinX(), ras.MaxX());
                span_gen.Prepare();
                while(ras.SweepScanline(sl))
                {
                    GenerateAndRenderSingleScanline(sl, ren, alloc, span_gen);
                }
            }
        }
        #endregion

        #region RenderCompound
        public static void RenderCompound(AntiAliasedCompundRasterizer ras,
                                       IScanline sl_aa,
                                       IScanline sl_bin,
                                       IPixelFormat pixelFormat,
                                       SpanAllocator alloc,
                                       IStyleHandler sh)
        {
#if true
            unsafe
            {
                if (ras.RewindScanlines())
                {
                    int min_x = ras.MinX();
                    int len = ras.MaxX() - min_x + 2;
                    sl_aa.Reset(min_x, ras.MaxX());
                    sl_bin.Reset(min_x, ras.MaxX());

                    //typedef typename BaseRenderer::color_type color_type;
                    ArrayPOD<RGBA_Bytes> color_span = alloc.Allocate((uint)len * 2);
                    byte[] ManagedCoversArray = sl_aa.GetCovers();
                    fixed (byte* pCovers = ManagedCoversArray)
                    {
                        fixed (RGBA_Bytes* pColorSpan = color_span.Array)
                        {
                            int mix_bufferOffset = len;
                            uint num_spans;

                            uint num_styles;
                            uint style;
                            bool solid;
                            while ((num_styles = ras.SweepStyles()) > 0)
                            {
                                if (num_styles == 1)
                                {
                                    // Optimization for a single Style. Happens often
                                    //-------------------------
                                    if (ras.SweepScanline(sl_aa, 0))
                                    {
                                        style = ras.Style(0);
                                        if (sh.IsSolid(style))
                                        {
                                            // Just solid fill
                                            //-----------------------
                                            RenderSolidSingleScanLine(pixelFormat, sl_aa, sh.Color(style));
                                        }
                                        else
                                        {
                                            // Arbitrary Span Generator
                                            //-----------------------
                                            ScanlineSpan span_aa = sl_aa.Begin;
                                            num_spans = sl_aa.NumberOfSpans;

                                            for (; ; )
                                            {
                                                len = span_aa.len;
                                                sh.GenerateSpan(pColorSpan,
                                                                 span_aa.x,
                                                                 sl_aa.y(),
                                                                 (uint)len,
                                                                 style);

                                                pixelFormat.BlendHorizontalColorSpan(span_aa.x,
                                                                      sl_aa.y(),
                                                                      (uint)span_aa.len,
                                                                      pColorSpan,
                                                                      &pCovers[span_aa.cover_index], 0);
                                                if (--num_spans == 0) break;
                                                span_aa = sl_aa.GetNextScanlineSpan();
                                            }
                                        }
                                    }
                                }
                                else // there are multiple Styles
                                {
                                    if (ras.SweepScanline(sl_bin, -1))
                                    {
                                        // Clear the spans of the mix_buffer
                                        //--------------------
                                        ScanlineSpan span_bin = sl_bin.Begin;
                                        num_spans = sl_bin.NumberOfSpans;
                                        for (; ; )
                                        {
                                            Basics.MemClear((byte*)&pColorSpan[mix_bufferOffset + span_bin.x - min_x],
                                                   span_bin.len * sizeof(RGBA_Bytes));

                                            if (--num_spans == 0) break;
                                            span_bin = sl_bin.GetNextScanlineSpan();
                                        }

                                        for (uint i = 0; i < num_styles; i++)
                                        {
                                            style = ras.Style(i);
                                            solid = sh.IsSolid(style);

                                            if (ras.SweepScanline(sl_aa, (int)i))
                                            {
                                                //IColorType* Colors;
                                                //IColorType* cspan;
                                                //typename ScanlineAA::cover_type* covers;
                                                ScanlineSpan span_aa = sl_aa.Begin;
                                                num_spans = sl_aa.NumberOfSpans;
                                                if (solid)
                                                {
                                                    // Just solid fill
                                                    //-----------------------
                                                    for (; ; )
                                                    {
                                                        RGBA_Bytes c = sh.Color(style);
                                                        len = span_aa.len;
                                                        RGBA_Bytes* colors = &pColorSpan[mix_bufferOffset + span_aa.x - min_x];
                                                        byte* covers = &pCovers[span_aa.cover_index];
                                                        do
                                                        {
                                                            if (*covers == cover_full)
                                                            {
                                                                *colors = c;
                                                            }
                                                            else
                                                            {
                                                                colors->add(c, *covers);
                                                            }
                                                            ++colors;
                                                            ++covers;
                                                        }
                                                        while (--len != 0);
                                                        if (--num_spans == 0) break;
                                                        span_aa = sl_aa.GetNextScanlineSpan();
                                                    }
                                                }
                                                else
                                                {
                                                    // Arbitrary Span Generator
                                                    //-----------------------
                                                    for (; ; )
                                                    {
                                                        len = span_aa.len;
                                                        RGBA_Bytes* colors = &pColorSpan[mix_bufferOffset + span_aa.x - min_x];
                                                        RGBA_Bytes* cspan = pColorSpan;
                                                        sh.GenerateSpan(cspan,
                                                                         span_aa.x,
                                                                         sl_aa.y(),
                                                                         (uint)len,
                                                                         style);
                                                        byte* covers = &pCovers[span_aa.cover_index]; 
                                                        do
                                                        {
                                                            if (*covers == cover_full)
                                                            {
                                                                *colors = *cspan;
                                                            }
                                                            else
                                                            {
                                                                colors->add(*cspan, *covers);
                                                            }
                                                            ++cspan;
                                                            ++colors;
                                                            ++covers;
                                                        }
                                                        while (--len != 0);
                                                        if (--num_spans == 0) break;
                                                        span_aa = sl_aa.GetNextScanlineSpan();
                                                    }
                                                }
                                            }
                                        }

                                        // Emit the blended result as a Color hspan
                                        //-------------------------
                                        span_bin = sl_bin.Begin;
                                        num_spans = sl_bin.NumberOfSpans;
                                        for (; ; )
                                        {
                                            pixelFormat.BlendHorizontalColorSpan(span_bin.x,
                                                                  sl_bin.y(),
                                                                  (uint)span_bin.len,
                                                                  &pColorSpan[mix_bufferOffset + span_bin.x - min_x],
                                                                  null,
                                                                  cover_full);
                                            if (--num_spans == 0) break;
                                            span_bin = sl_bin.GetNextScanlineSpan();
                                        }
                                    } // if(ras.SweepScanline(sl_bin, -1))
                                } // if(num_styles == 1) ... else
                            } // while((num_styles = ras.SweepStyles()) > 0)
                        }
                    }
                } // if(ras.RewindScanlines())
#endif
            }
        }
        #endregion

        public void DrawString(string Text, double x, double y)
        {
            TextPath.SetFontSize(10);
            TextPath.StartPoint(x, y);
            TextPath.Text = Text;
            Render(StrockedText, new RGBA_Bytes(0, 0, 0, 255));
        }

        public void Line(PointD Start, PointD End, RGBA_Bytes color)
        {
            Line(Start.x, Start.y, End.x, End.y, color);
        }

        public void Line(double x1, double y1, double x2, double y2, RGBA_Bytes color)
        {
            PathStorage m_LinesToDraw = new PathStorage(); 
            m_LinesToDraw.RemoveAll();
            m_LinesToDraw.move_to(x1, y1);
            m_LinesToDraw.line_to(x2, y2);
            StrokeConverter StrockedLineToDraw = new StrokeConverter(m_LinesToDraw);
            Render(StrockedLineToDraw, color);
        }
    }

    /*
    //==============================================renderer_scanline_aa_solid
    public class renderer_scanline_aa_solid : IRenderer
    {
        private IPixelFormat m_ren;
        private IColorType m_color;

        //--------------------------------------------------------------------
        public renderer_scanline_aa_solid(IPixelFormat ren) 
        {
            m_ren = ren;
            m_color = new rgba(0,0,0,1);
        }

        public void Attach(IPixelFormat ren)
        {
            m_ren = ren;
        }
        
        //--------------------------------------------------------------------
        public IColorType Color
        {
            Get
            {
                return m_color;
            }
            Set
            {
                m_color = Value;
            }
        }

        //--------------------------------------------------------------------
        public void Prepare() {}

        //--------------------------------------------------------------------
        public void render(IScanline sl)
        {
            renderer_scanlines.render_scanline_aa_solid(sl, m_ren, m_color.Get_rgba8());
        }
    };

    //====================================================renderer_scanline_aa
    template<class BaseRenderer, class SpanAllocator, class SpanGenerator> 
    class renderer_scanline_aa
    {
    public:
        typedef BaseRenderer  base_ren_type;
        typedef SpanAllocator alloc_type;
        typedef SpanGenerator span_gen_type;

        //--------------------------------------------------------------------
        renderer_scanline_aa() : m_ren(0), m_alloc(0), m_span_gen(0) {}
        renderer_scanline_aa(base_ren_type& ren, 
                             alloc_type& alloc, 
                             span_gen_type& span_gen) :
            m_ren(&ren),
            m_alloc(&alloc),
            m_span_gen(&span_gen)
        {}
        void Attach(base_ren_type& ren, 
                    alloc_type& alloc, 
                    span_gen_type& span_gen)
        {
            m_ren = &ren;
            m_alloc = &alloc;
            m_span_gen = &span_gen;
        }
        
        //--------------------------------------------------------------------
        void Prepare() { m_span_gen->Prepare(); }

        //--------------------------------------------------------------------
        template<class Scanline> void render(const Scanline& sl)
        {
            render_scanline_aa(sl, *m_ren, *m_alloc, *m_span_gen);
        }

    private:
        base_ren_type* m_ren;
        alloc_type*    m_alloc;
        span_gen_type* m_span_gen;
    };

    //===============================================render_scanline_bin_solid
    template<class Scanline, class BaseRenderer, class ColorT> 
    void render_scanline_bin_solid(const Scanline& sl, 
                                   BaseRenderer& ren, 
                                   const ColorT& Color)
    {
        uint NumberOfSpans = sl.NumberOfSpans();
        typename Scanline::const_iterator Span = sl.Begin();
        for(;;)
        {
            ren.BlendHorizontalLine(Span->x, 
                            sl.y(), 
                            Span->x - 1 + ((Span->len < 0) ? 
                                              -Span->len : 
                                               Span->len), 
                               Color, 
                               cover_full);
            if(--NumberOfSpans == 0) break;
            ++Span;
        }
    }

    //==============================================render_scanlines_bin_solid
    template<class Rasterizer, class Scanline, 
             class BaseRenderer, class ColorT>
    void render_scanlines_bin_solid(Rasterizer& ras, Scanline& sl, 
                                    BaseRenderer& ren, const ColorT& Color)
    {
        if(ras.RewindScanlines())
        {
            // Explicitly convert "Color" to the BaseRenderer Color type.
            // For example, it can be called with Color type "rgba", while
            // "rgba8" is needed. Otherwise it will be implicitly 
            // converted in the loop many times.
            //----------------------
            typename BaseRenderer::color_type ren_color(Color);

            sl.Reset(ras.MinX(), ras.MaxX());
            while(ras.SweepScanline(sl))
            {
                //render_scanline_bin_solid(sl, ren, ren_color);

                // This code is equivalent to the above call (copy/paste). 
                // It's just a "manual" optimization for old compilers,
                // like Microsoft Visual C++ v6.0
                //-------------------------------
                uint NumberOfSpans = sl.NumberOfSpans();
                typename Scanline::const_iterator Span = sl.Begin();
                for(;;)
                {
                    ren.BlendHorizontalLine(Span->x, 
                                    sl.y(), 
                                    Span->x - 1 + ((Span->len < 0) ? 
                                                      -Span->len : 
                                                       Span->len), 
                                       ren_color, 
                                       cover_full);
                    if(--NumberOfSpans == 0) break;
                    ++Span;
                }
            }
        }
    }

    //=============================================renderer_scanline_bin_solid
    template<class BaseRenderer> class renderer_scanline_bin_solid
    {
    public:
        typedef BaseRenderer base_ren_type;
        typedef typename base_ren_type::color_type color_type;

        //--------------------------------------------------------------------
        renderer_scanline_bin_solid() : m_ren(0) {}
        explicit renderer_scanline_bin_solid(base_ren_type& ren) : m_ren(&ren) {}
        void Attach(base_ren_type& ren)
        {
            m_ren = &ren;
        }
        
        //--------------------------------------------------------------------
        void Color(const color_type& c) { m_color = c; }
        const color_type& Color() const { return m_color; }

        //--------------------------------------------------------------------
        void Prepare() {}

        //--------------------------------------------------------------------
        template<class Scanline> void render(const Scanline& sl)
        {
            render_scanline_bin_solid(sl, *m_ren, m_color);
        }
        
    private:
        base_ren_type* m_ren;
        color_type m_color;
    };

    //======================================================render_scanline_bin
    template<class Scanline, class BaseRenderer, 
             class SpanAllocator, class SpanGenerator> 
    void render_scanline_bin(const Scanline& sl, BaseRenderer& ren, 
                             SpanAllocator& alloc, SpanGenerator& span_gen)
    {
        int y = sl.y();

        uint NumberOfSpans = sl.NumberOfSpans();
        typename Scanline::const_iterator Span = sl.Begin();
        for(;;)
        {
            int x = Span->x;
            int len = Span->len;
            if(len < 0) len = -len;
            typename BaseRenderer::color_type* Colors = alloc.Allocate(len);
            span_gen.Generate(Colors, x, y, len);
            ren.BlendHorizontalColorSpan(x, y, len, Colors, 0, cover_full); 
            if(--NumberOfSpans == 0) break;
            ++Span;
        }
    }

    //=====================================================render_scanlines_bin
    template<class Rasterizer, class Scanline, class BaseRenderer, 
             class SpanAllocator, class SpanGenerator>
    void render_scanlines_bin(Rasterizer& ras, Scanline& sl, BaseRenderer& ren, 
                              SpanAllocator& alloc, SpanGenerator& span_gen)
    {
        if(ras.RewindScanlines())
        {
            sl.Reset(ras.MinX(), ras.MaxX());
            span_gen.Prepare();
            while(ras.SweepScanline(sl))
            {
                render_scanline_bin(sl, ren, alloc, span_gen);
            }
        }
    }

    //====================================================renderer_scanline_bin
    template<class BaseRenderer, class SpanAllocator, class SpanGenerator> 
    class renderer_scanline_bin
    {
    public:
        typedef BaseRenderer  base_ren_type;
        typedef SpanAllocator alloc_type;
        typedef SpanGenerator span_gen_type;

        //--------------------------------------------------------------------
        renderer_scanline_bin() : m_ren(0), m_alloc(0), m_span_gen(0) {}
        renderer_scanline_bin(base_ren_type& ren, 
                              alloc_type& alloc, 
                              span_gen_type& span_gen) :
            m_ren(&ren),
            m_alloc(&alloc),
            m_span_gen(&span_gen)
        {}
        void Attach(base_ren_type& ren, 
                    alloc_type& alloc, 
                    span_gen_type& span_gen)
        {
            m_ren = &ren;
            m_alloc = &alloc;
            m_span_gen = &span_gen;
        }
        
        //--------------------------------------------------------------------
        void Prepare() { m_span_gen->Prepare(); }

        //--------------------------------------------------------------------
        template<class Scanline> void render(const Scanline& sl)
        {
            render_scanline_bin(sl, *m_ren, *m_alloc, *m_span_gen);
        }

    private:
        base_ren_type* m_ren;
        alloc_type*    m_alloc;
        span_gen_type* m_span_gen;
    };

    //=======================================render_scanlines_compound_layered
    template<class Rasterizer, 
             class ScanlineAA, 
             class BaseRenderer, 
             class SpanAllocator,
             class StyleHandler>
    void render_scanlines_compound_layered(Rasterizer& ras, 
                                           ScanlineAA& sl_aa,
                                           BaseRenderer& ren,
                                           SpanAllocator& alloc,
                                           StyleHandler& sh)
    {
        if(ras.RewindScanlines())
        {
            int MinX = ras.MinX();
            int len = ras.MaxX() - MinX + 2;
            sl_aa.Reset(MinX, ras.MaxX());

            typedef typename BaseRenderer::color_type color_type;
            color_type* color_span   = alloc.Allocate(len * 2);
            color_type* mix_buffer   = color_span + len;
            cover_type* cover_buffer = ras.AllocateCoverBuffer(len);
            uint NumberOfSpans;

            uint num_styles;
            uint Style;
            bool     solid;
            while((num_styles = ras.SweepStyles()) > 0)
            {
                typename ScanlineAA::const_iterator span_aa;
                if(num_styles == 1)
                {
                    // Optimization for a single Style. Happens often
                    //-------------------------
                    if(ras.SweepScanline(sl_aa, 0))
                    {
                        Style = ras.Style(0);
                        if(sh.IsSolid(Style))
                        {
                            // Just solid fill
                            //-----------------------
                            render_scanline_aa_solid(sl_aa, ren, sh.Color(Style));
                        }
                        else
                        {
                            // Arbitrary Span Generator
                            //-----------------------
                            span_aa   = sl_aa.Begin();
                            NumberOfSpans = sl_aa.NumberOfSpans();
                            for(;;)
                            {
                                len = span_aa->len;
                                sh.GenerateSpan(color_span, 
                                                 span_aa->x, 
                                                 sl_aa.y(), 
                                                 len, 
                                                 Style);

                                ren.BlendHorizontalColorSpan(span_aa->x, 
                                                      sl_aa.y(), 
                                                      span_aa->len,
                                                      color_span,
                                                      span_aa->covers);
                                if(--NumberOfSpans == 0) break;
                                ++span_aa;
                            }
                        }
                    }
                }
                else
                {
                    int      sl_start = ras.ScanlineStart();
                    uint sl_len   = ras.ScanlineLength();

                    if(sl_len)
                    {
                        MemClear(mix_buffer + sl_start - MinX, 
                               sl_len * sizeof(color_type));

                        MemClear(cover_buffer + sl_start - MinX, 
                               sl_len * sizeof(cover_type));

                        int sl_y = 0x7FFFFFFF;
                        uint i;
                        for(i = 0; i < num_styles; i++)
                        {
                            Style = ras.Style(i);
                            solid = sh.IsSolid(Style);

                            if(ras.SweepScanline(sl_aa, i))
                            {
                                uint    cover;
                                color_type* Colors;
                                color_type* cspan;
                                cover_type* src_covers;
                                cover_type* dst_covers;
                                span_aa   = sl_aa.Begin();
                                NumberOfSpans = sl_aa.NumberOfSpans();
                                sl_y      = sl_aa.y();
                                if(solid)
                                {
                                    // Just solid fill
                                    //-----------------------
                                    for(;;)
                                    {
                                        color_type c = sh.Color(Style);
                                        len    = span_aa->len;
                                        Colors = mix_buffer + span_aa->x - MinX;
                                        src_covers = span_aa->covers;
                                        dst_covers = cover_buffer + span_aa->x - MinX;
                                        do
                                        {
                                            cover = *src_covers;
                                            if(*dst_covers + cover > cover_full)
                                            {
                                                cover = cover_full - *dst_covers;
                                            }
                                            if(cover)
                                            {
                                                Colors->Add(c, cover);
                                                *dst_covers += cover;
                                            }
                                            ++Colors;
                                            ++src_covers;
                                            ++dst_covers;
                                        }
                                        while(--len);
                                        if(--NumberOfSpans == 0) break;
                                        ++span_aa;
                                    }
                                }
                                else
                                {
                                    // Arbitrary Span Generator
                                    //-----------------------
                                    for(;;)
                                    {
                                        len = span_aa->len;
                                        Colors = mix_buffer + span_aa->x - MinX;
                                        cspan  = color_span;
                                        sh.GenerateSpan(cspan, 
                                                         span_aa->x, 
                                                         sl_aa.y(), 
                                                         len, 
                                                         Style);
                                        src_covers = span_aa->covers;
                                        dst_covers = cover_buffer + span_aa->x - MinX;
                                        do
                                        {
                                            cover = *src_covers;
                                            if(*dst_covers + cover > cover_full)
                                            {
                                                cover = cover_full - *dst_covers;
                                            }
                                            if(cover)
                                            {
                                                Colors->Add(*cspan, cover);
                                                *dst_covers += cover;
                                            }
                                            ++cspan;
                                            ++Colors;
                                            ++src_covers;
                                            ++dst_covers;
                                        }
                                        while(--len);
                                        if(--NumberOfSpans == 0) break;
                                        ++span_aa;
                                    }
                                }
                            }
                        }
                        ren.BlendHorizontalColorSpan(sl_start, 
                                              sl_y, 
                                              sl_len,
                                              mix_buffer + sl_start - MinX,
                                              0,
                                              cover_full);
                    } //if(sl_len)
                } //if(num_styles == 1) ... else
            } //while((num_styles = ras.SweepStyles()) > 0)
        } //if(ras.RewindScanlines())
    }
     */
}
