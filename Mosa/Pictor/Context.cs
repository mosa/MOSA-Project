using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 
        /// </summary>
        int x;
        
        /// <summary>
        /// 
        /// </summary>
        int y;

        /// <summary>
        /// 
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct PointD
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public PointD(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 
        /// </summary>
        double x;
        
        /// <summary>
        /// 
        /// </summary>
        double y;

        /// <summary>
        /// 
        /// </summary>
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct Distance
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public Distance(double dx, double dy)
        {
            this.dx = dx;
            this.dy = dy;
        }

        /// <summary>
        /// 
        /// </summary>
        double dx;
        
        /// <summary>
        /// 
        /// </summary>
        double dy;

        /// <summary>
        /// 
        /// </summary>
        public double Dx
        {
            get { return dx; }
            set { dx = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Dy
        {
            get { return dy; }
            set { dy = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public Color(double r, double g, double b)
            : this(r, g, b, 1.0)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public Color(double r, double g, double b, double a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        /// <summary>
        /// 
        /// </summary>
        double r;
        
        /// <summary>
        /// 
        /// </summary>
        double g;

        /// <summary>
        /// 
        /// </summary>
        double b;

        /// <summary>
        /// 
        /// </summary>
        double a;

        /// <summary>
        /// 
        /// </summary>
        public double R
        {
            get { return r; }
            set { r = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double G
        {
            get { return g; }
            set { g = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double B
        {
            get { return b; }
            set { b = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double A
        {
            get { return a; }
            set { a = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Obsolete("Renamed Pictor.Context per suggestion from cairo binding guidelines.")]
    public class Graphics : Context
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public Graphics(IntPtr state) : base(state) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        public Graphics(Surface surface) : base(surface) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Context : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr state = IntPtr.Zero;

        /// <summary>
        /// 
        /// </summary>
        static int native_glyph_size;

        /// <summary>
        /// 
        /// </summary>
        static int c_compiler_long_size;

        /// <summary>
        /// 
        /// </summary>
        static Context()
        {
            //
            // This is used to determine what kind of structure
            // we should use to marshal Glyphs, as the public
            // definition in Pictor uses `long', which can be
            // 32 bits or 64 bits depending on the platform.
            //
            // We assume that sizeof(long) == sizeof(void*)
            // except in the case of Win64 where sizeof(long)
            // is 32 bits
            //
            int ptr_size = Marshal.SizeOf(typeof(IntPtr));

            PlatformID platform = Environment.OSVersion.Platform;
            if (platform == PlatformID.Win32NT ||
                platform == PlatformID.Win32S ||
                platform == PlatformID.Win32Windows ||
                platform == PlatformID.WinCE ||
                ptr_size == 4)
            {
                c_compiler_long_size = 4;
                native_glyph_size = Marshal.SizeOf(typeof(NativeGlyph_4byte_longs));
            }
            else
            {
                c_compiler_long_size = 8;
                native_glyph_size = Marshal.SizeOf(typeof(Glyph));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        public Context(Surface surface)
        {
            //state = NativeMethods.cairo_create(surface.Handle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public Context(IntPtr state)
        {
            this.state = state;
        }

        /// <summary>
        /// 
        /// </summary>
        ~Context()
        {
            Dispose(false);
        }

        /// <summary>
        /// 
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                Console.Error.WriteLine("Pictor.Context: called from finalization thread, programmer is missing a call to Dispose");
                return;
            }

            if (state == IntPtr.Zero)
                return;

            //Console.WriteLine ("Destroying");
            //NativeMethods.cairo_destroy(state);
            state = IntPtr.Zero;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            //NativeMethods.cairo_save(state);
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Restore()
        {
            //NativeMethods.cairo_restore(state);
        }

        /// <summary>
        /// 
        /// </summary>
        public Antialias Antialias
        {
            get { return Antialias.Default; }
            //get { return NativeMethods.cairo_get_antialias(state); }
            //set { NativeMethods.cairo_set_antialias(state, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Pictor.Status Status
        {
            get
            {
                return Pictor.Status.InvalidContent;
                //return NativeMethods.cairo_status(state);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return state;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Pictor.Operator Operator
        {
            /*set
            {
                NativeMethods.cairo_set_operator(state, value);
            }*/

            get
            {
                return Operator.Add;
                //return NativeMethods.cairo_get_operator(state);
            }
        }

        //FIXME: obsolete this property
        /// <summary>
        /// 
        /// </summary>
        public Pictor.Color Color
        {
            set
            {
                //NativeMethods.cairo_set_source_rgba(state, value.R, value.G, value.B, value.A);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Obsolete("Use Color property")]
        public Pictor.Color ColorRgb
        {
            set
            {
                Color = new Color(value.R, value.G, value.B);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Tolerance
        {
            /*get
            {
                return NativeMethods.cairo_get_tolerance(state);
            }*/

            set
            {
                //NativeMethods.cairo_set_tolerance(state, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Pictor.FillRule FillRule
        {
            set
            {
                //NativeMethods.cairo_set_fill_rule(state, value);
            }

            get
            {
                return FillRule.EvenOdd; // return NativeMethods.cairo_get_fill_rule(state);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double LineWidth
        {
            set
            {
                //NativeMethods.cairo_set_line_width(state, value);
            }

            get
            {
                return 0.0; // return NativeMethods.cairo_get_line_width(state);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Pictor.LineCap LineCap
        {
            set
            {
                //NativeMethods.cairo_set_line_cap(state, value);
            }

            get
            {
                return LineCap.Butt; // return NativeMethods.cairo_get_line_cap(state);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Pictor.LineJoin LineJoin
        {
            set
            {
                //NativeMethods.cairo_set_line_join(state, value);
            }

            get
            {
                return LineJoin.Bevel; //return NativeMethods.cairo_get_line_join(state);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dashes"></param>
        /// <param name="offset"></param>
        public void SetDash(double[] dashes, double offset)
        {
            //NativeMethods.cairo_set_dash(state, dashes, dashes.Length, offset);
        }

        /// <summary>
        /// 
        /// </summary>
        public Pattern Pattern
        {
            set
            {
                //NativeMethods.cairo_set_source(state, value.Pointer);
            }

            get
            {
                return null;// return new Pattern(NativeMethods.cairo_get_source(state));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Pattern Source
        {
            set
            {
                //NativeMethods.cairo_set_source(state, value.Pointer);
            }

            get
            {
                return null; // return Pattern.Lookup(NativeMethods.cairo_get_source(state));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double MiterLimit
        {
            set
            {
                //NativeMethods.cairo_set_miter_limit(state, value);
            }

            get
            {
                return 0.0;//return NativeMethods.cairo_get_miter_limit(state);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PointD CurrentPoint
        {
            get
            {
                double x = 0.0, y = 0.0;
                //NativeMethods.cairo_get_current_point(state, out x, out y);
                return new PointD(x, y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Pictor.Surface Target
        {
            set
            {
                //if (state != IntPtr.Zero)
                    //NativeMethods.cairo_destroy(state);

                //state = NativeMethods.cairo_create(value.Handle);
            }

            get
            {
                return null;
                //return Pictor.Surface.LookupExternalSurface(
                        //NativeMethods.cairo_get_target(state));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Pictor.ScaledFont ScaledFont
        {
            set
            {
                //NativeMethods.cairo_set_scaled_font(state, value.Handle);
            }

            get
            {
                return new ScaledFont(new IntPtr());
                //return new ScaledFont(NativeMethods.cairo_get_scaled_font(state));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint ReferenceCount
        {
            get { return 0; }
            //get { return NativeMethods.cairo_get_reference_count(state); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public void SetSourceRGB(double r, double g, double b)
        {
            //NativeMethods.cairo_set_source_rgb(state, r, g, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public void SetSourceRGBA(double r, double g, double b, double a)
        {
            //NativeMethods.cairo_set_source_rgba(state, r, g, b, a);
        }

        //[Obsolete ("Use SetSource method (with double parameters)")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetSourceSurface(Surface source, int x, int y)
        {
            //NativeMethods.cairo_set_source_surface(state, source.Handle, x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetSource(Surface source, double x, double y)
        {
            //NativeMethods.cairo_set_source_surface(state, source.Handle, x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public void SetSource(Surface source)
        {
            //NativeMethods.cairo_set_source_surface(state, source.Handle, 0, 0);
        }

        #region Path methods
        /// <summary>
        /// 
        /// </summary>
        public void NewPath()
        {
            //NativeMethods.cairo_new_path(state);
        }

        /// <summary>
        /// 
        /// </summary>
        public void NewSubPath()
        {
            //NativeMethods.cairo_new_sub_path(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public void MoveTo(PointD p)
        {
            MoveTo(p.X, p.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveTo(double x, double y)
        {
            //NativeMethods.cairo_move_to(state, x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public void LineTo(PointD p)
        {
            LineTo(p.X, p.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void LineTo(double x, double y)
        {
            //NativeMethods.cairo_line_to(state, x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public void CurveTo(PointD p1, PointD p2, PointD p3)
        {
            CurveTo(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        public void CurveTo(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            //NativeMethods.cairo_curve_to(state, x1, y1, x2, y2, x3, y3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        public void RelMoveTo(Distance d)
        {
            RelMoveTo(d.Dx, d.Dy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void RelMoveTo(double dx, double dy)
        {
            //NativeMethods.cairo_rel_move_to(state, dx, dy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        public void RelLineTo(Distance d)
        {
            RelLineTo(d.Dx, d.Dy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void RelLineTo(double dx, double dy)
        {
            //NativeMethods.cairo_rel_line_to(state, dx, dy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="d3"></param>
        public void RelCurveTo(Distance d1, Distance d2, Distance d3)
        {
            RelCurveTo(d1.Dx, d1.Dy, d2.Dx, d2.Dy, d3.Dx, d3.Dy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dx1"></param>
        /// <param name="dy1"></param>
        /// <param name="dx2"></param>
        /// <param name="dy2"></param>
        /// <param name="dx3"></param>
        /// <param name="dy3"></param>
        public void RelCurveTo(double dx1, double dy1, double dx2, double dy2, double dx3, double dy3)
        {
            //NativeMethods.cairo_rel_curve_to(state, dx1, dy1, dx2, dy2, dx3, dy3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="radius"></param>
        /// <param name="angle1"></param>
        /// <param name="angle2"></param>
        public void Arc(double xc, double yc, double radius, double angle1, double angle2)
        {
            //NativeMethods.cairo_arc(state, xc, yc, radius, angle1, angle2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="radius"></param>
        /// <param name="angle1"></param>
        /// <param name="angle2"></param>
        public void ArcNegative(double xc, double yc, double radius, double angle1, double angle2)
        {
            //NativeMethods.cairo_arc_negative(state, xc, yc, radius, angle1, angle2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectangle"></param>
        public void Rectangle(Rectangle rectangle)
        {
            Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Rectangle(PointD p, double width, double height)
        {
            Rectangle(p.X, p.Y, width, height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Rectangle(double x, double y, double width, double height)
        {
            //NativeMethods.cairo_rectangle(state, x, y, width, height);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClosePath()
        {
            //NativeMethods.cairo_close_path(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Path CopyPath()
        {
            return null;
            //return new Path(NativeMethods.cairo_copy_path(state));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Path CopyPathFlat()
        {
            return null; // return new Path(NativeMethods.cairo_copy_path_flat(state));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void AppendPath(Path path)
        {
            //NativeMethods.cairo_append_path(state, path.handle);
        }

        #endregion

        #region Painting Methods
        /// <summary>
        /// 
        /// </summary>
        public void Paint()
        {
            //NativeMethods.cairo_paint(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alpha"></param>
        public void PaintWithAlpha(double alpha)
        {
            //NativeMethods.cairo_paint_with_alpha(state, alpha);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        public void Mask(Pattern pattern)
        {
            //NativeMethods.cairo_mask(state, pattern.Pointer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="surface_x"></param>
        /// <param name="surface_y"></param>
        public void MaskSurface(Surface surface, double surface_x, double surface_y)
        {
            //NativeMethods.cairo_mask_surface(state, surface.Handle, surface_x, surface_y);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stroke()
        {
            //NativeMethods.cairo_stroke(state);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StrokePreserve()
        {
            //NativeMethods.cairo_stroke_preserve(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Rectangle StrokeExtents()
        {
            double x1 = 0.0, y1 = 0.0, x2 = 0.0, y2 = 0.0;
            //NativeMethods.cairo_stroke_extents(state, out x1, out y1, out x2, out y2);
            return new Rectangle(x1, y1, x2, y2);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Fill()
        {
            //NativeMethods.cairo_fill(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Rectangle FillExtents()
        {
            double x1 = 0.0, y1 = 0.0, x2 = 0.0, y2 = 0.0;
            //NativeMethods.cairo_fill_extents(state, out x1, out y1, out x2, out y2);
            return new Rectangle(x1, y1, x2, y2);
        }

        /// <summary>
        /// 
        /// </summary>
        public void FillPreserve()
        {
            //NativeMethods.cairo_fill_preserve(state);
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        public void Clip()
        {
            //NativeMethods.cairo_clip(state);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClipPreserve()
        {
            //NativeMethods.cairo_clip_preserve(state);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetClip()
        {
            //NativeMethods.cairo_reset_clip(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool InStroke(double x, double y)
        {
            return true; // NativeMethods.cairo_in_stroke(state, x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool InFill(double x, double y)
        {
            return true; // NativeMethods.cairo_in_fill(state, x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Pattern PopGroup()
        {
            return null; // Pattern.Lookup(NativeMethods.cairo_pop_group(state));
        }

        /// <summary>
        /// 
        /// </summary>
        public void PopGroupToSource()
        {
            //NativeMethods.cairo_pop_group_to_source(state);
        }

        /// <summary>
        /// 
        /// </summary>
        public void PushGroup()
        {
            //NativeMethods.cairo_push_group(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public void PushGroup(Content content)
        {
            //NativeMethods.cairo_push_group_with_content(state, content);
        }

        /// <summary>
        /// 
        /// </summary>
        public Surface GroupTarget
        {
            get
            {
                IntPtr surface = new IntPtr();// = NativeMethods.cairo_get_group_target(state);
                return Surface.LookupSurface(surface);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        public void Rotate(double angle)
        {
            //NativeMethods.cairo_rotate(state, angle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        public void Scale(double sx, double sy)
        {
            //NativeMethods.cairo_scale(state, sx, sy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        public void Translate(double tx, double ty)
        {
            //NativeMethods.cairo_translate(state, tx, ty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        public void Transform(Matrix m)
        {
            //NativeMethods.cairo_transform(state, m);
        }

        #region Methods that will become obsolete in the long term, after 1.2.5 becomes wildly available

        //[Obsolete("Use UserToDevice instead")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void TransformPoint(ref double x, ref double y)
        {
            //NativeMethods.cairo_user_to_device(state, ref x, ref y);
        }

        //[Obsolete("Use UserToDeviceDistance instead")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void TransformDistance(ref double dx, ref double dy)
        {
            //NativeMethods.cairo_user_to_device_distance(state, ref dx, ref dy);
        }

        //[Obsolete("Use InverseTransformPoint instead")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void InverseTransformPoint(ref double x, ref double y)
        {
            //NativeMethods.cairo_device_to_user(state, ref x, ref y);
        }

        //[Obsolete("Use DeviceToUserDistance instead")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void InverseTransformDistance(ref double dx, ref double dy)
        {
            //NativeMethods.cairo_device_to_user_distance(state, ref dx, ref dy);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void UserToDevice(ref double x, ref double y)
        {
            //NativeMethods.cairo_user_to_device(state, ref x, ref y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void UserToDeviceDistance(ref double dx, ref double dy)
        {
            //NativeMethods.cairo_user_to_device_distance(state, ref dx, ref dy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DeviceToUser(ref double x, ref double y)
        {
            //NativeMethods.cairo_device_to_user(state, ref x, ref y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void DeviceToUserDistance(ref double dx, ref double dy)
        {
            //NativeMethods.cairo_device_to_user_distance(state, ref dx, ref dy);
        }

        /// <summary>
        /// /
        /// </summary>
        public Pictor.Matrix Matrix
        {
            set
            {
                //NativeMethods.cairo_set_matrix(state, value);
            }

            get
            {
                Matrix m = new Matrix();
                //NativeMethods.cairo_get_matrix(state, m);
                return m;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        public void SetFontSize(double scale)
        {
            //NativeMethods.cairo_set_font_size(state, scale);
        }

        /// <summary>
        /// 
        /// </summary>
        public void IdentityMatrix()
        {
            //NativeMethods.cairo_identity_matrix(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        [Obsolete("Use SetFontSize() instead.")]
        public void FontSetSize(double scale)
        {
            SetFontSize(scale);
        }

        /// <summary>
        /// 
        /// </summary>
        [Obsolete("Use SetFontSize() instead.")]
        public double FontSize
        {
            set { SetFontSize(value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Matrix FontMatrix
        {
            get
            {
                Matrix m = new Matrix();
                //NativeMethods.cairo_get_font_matrix(state, out m);
                return m;
            }
            //set { NativeMethods.cairo_set_font_matrix(state, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public FontOptions FontOptions
        {
            get
            {
                FontOptions options = new FontOptions();
              //  NativeMethods.cairo_get_font_options(state, options.Handle);
                return options;
            }
            //set { NativeMethods.cairo_set_font_options(state, value.Handle); }
        }

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct NativeGlyph_4byte_longs
        {
            public int index;
            public double x;
            public double y;

            public NativeGlyph_4byte_longs(Glyph source)
            {
                index = (int)source.index;
                x = source.x;
                y = source.y;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="glyphs"></param>
        /// <returns></returns>
        static internal IntPtr FromGlyphToUnManagedMemory(Glyph[] glyphs)
        {
            IntPtr dest = Marshal.AllocHGlobal(native_glyph_size * glyphs.Length);
            long pos = dest.ToInt64();

            if (c_compiler_long_size == 8)
            {
                foreach (Glyph g in glyphs)
                {
                    Marshal.StructureToPtr(g, (IntPtr)pos, false);
                    pos += native_glyph_size;
                }
            }
            else
            {
                foreach (Glyph g in glyphs)
                {
                    NativeGlyph_4byte_longs n = new NativeGlyph_4byte_longs(g);

                    Marshal.StructureToPtr(n, (IntPtr)pos, false);
                    pos += native_glyph_size;
                }
            }

            return dest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="glyphs"></param>
        public void ShowGlyphs(Glyph[] glyphs)
        {
            IntPtr ptr;

            ptr = FromGlyphToUnManagedMemory(glyphs);

            //NativeMethods.cairo_show_glyphs(state, ptr, glyphs.Length);

            Marshal.FreeHGlobal(ptr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="glyphs"></param>
        [Obsolete("The matrix argument was never used, use ShowGlyphs(Glyphs []) instead")]
        public void ShowGlyphs(Matrix matrix, Glyph[] glyphs)
        {
            ShowGlyphs(glyphs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="glyphs"></param>
        [Obsolete("The matrix argument was never used, use GlyphPath(Glyphs []) instead")]
        public void GlyphPath(Matrix matrix, Glyph[] glyphs)
        {
            GlyphPath(glyphs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="glyphs"></param>
        public void GlyphPath(Glyph[] glyphs)
        {
            IntPtr ptr;

            ptr = FromGlyphToUnManagedMemory(glyphs);

            //NativeMethods.cairo_glyph_path(state, ptr, glyphs.Length);

            Marshal.FreeHGlobal(ptr);

        }

        /// <summary>
        /// 
        /// </summary>
        public FontExtents FontExtents
        {
            get
            {
                FontExtents f_extents = new FontExtents();
              //  NativeMethods.cairo_font_extents(state, out f_extents);
                return f_extents;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CopyPage()
        {
            //NativeMethods.cairo_copy_page(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="family"></param>
        /// <param name="slant"></param>
        /// <param name="weight"></param>
        [Obsolete("Use SelectFontFace() instead.")]
        public void FontFace(string family, FontSlant slant, FontWeight weight)
        {
            SelectFontFace(family, slant, weight);
        }

        /// <summary>
        /// 
        /// </summary>
        public FontFace ContextFontFace
        {
            get
            {
                return new FontFace(new IntPtr());
                //return Pictor.FontFace.Lookup(NativeMethods.cairo_get_font_face(state));
            }

            set
            {
                //NativeMethods.cairo_set_font_face(state, value == null ? IntPtr.Zero : value.Handle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="family"></param>
        /// <param name="slant"></param>
        /// <param name="weight"></param>
        public void SelectFontFace(string family, FontSlant slant, FontWeight weight)
        {
            //NativeMethods.cairo_select_font_face(state, family, slant, weight);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowPage()
        {
            //NativeMethods.cairo_show_page(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        public void ShowText(string str)
        {
            //NativeMethods.cairo_show_text(state, str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        public void TextPath(string str)
        {
            //NativeMethods.cairo_text_path(state, str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="utf8"></param>
        /// <returns></returns>
        public TextExtents TextExtents(string utf8)
        {
            TextExtents extents = new TextExtents();
            //NativeMethods.cairo_text_extents(state, utf8, out extents);
            return extents;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="glyphs"></param>
        /// <returns></returns>
        public TextExtents GlyphExtents(Glyph[] glyphs)
        {
            IntPtr ptr = FromGlyphToUnManagedMemory(glyphs);

            TextExtents extents = new TextExtents();

            //NativeMethods.cairo_glyph_extents(state, ptr, glyphs.Length, out extents);

            Marshal.FreeHGlobal(ptr);

            return extents;
        }
    }
}
