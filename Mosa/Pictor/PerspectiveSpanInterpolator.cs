/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */
using System;

namespace Pictor
{
    /*
    //===========================================span_interpolator_persp_exact
    //template<uint SubpixelShift = 8> 
    class span_interpolator_persp_exact
    {
    public:
        typedef trans_perspective trans_type;
        typedef trans_perspective::IteratorX iterator_type;
        enum ESubpixelScale
        {
            subpixel_shift = SubpixelShift,
            Scale = 1 << subpixel_shift
        };

        //--------------------------------------------------------------------
        span_interpolator_persp_exact() {}

        //--------------------------------------------------------------------
        // Arbitrary quadrangle transformations
        span_interpolator_persp_exact(double[] src, double[] dst) 
        {
            QuadToQuad(src, dst);
        }

        //--------------------------------------------------------------------
        // Direct transformations 
        span_interpolator_persp_exact(double x1, double y1, 
                                      double x2, double y2, 
                                      double[] quad)
        {
            RectangleToQuad(x1, y1, x2, y2, quad);
        }

        //--------------------------------------------------------------------
        // Reverse transformations 
        span_interpolator_persp_exact(double[] quad, 
                                      double x1, double y1, 
                                      double x2, double y2)
        {
            QuadToRectangle(quad, x1, y1, x2, y2);
        }

        //--------------------------------------------------------------------
        // Set the transformations using two arbitrary quadrangles.
        void QuadToQuad(double[] src, double[] dst)
        {
            m_trans_dir.QuadToQuad(src, dst);
            m_trans_inv.QuadToQuad(dst, src);
        }

        //--------------------------------------------------------------------
        // Set the direct transformations, i.e., rectangle -> quadrangle
        void RectangleToQuad(double x1, double y1, double x2, double y2, 
                          double[] quad)
        {
            double src[8];
            src[0] = src[6] = x1;
            src[2] = src[4] = x2;
            src[1] = src[3] = y1;
            src[5] = src[7] = y2;
            QuadToQuad(src, quad);
        }


        //--------------------------------------------------------------------
        // Set the reverse transformations, i.e., quadrangle -> rectangle
        void QuadToRectangle(double[] quad, 
                          double x1, double y1, double x2, double y2)
        {
            double dst[8];
            dst[0] = dst[6] = x1;
            dst[2] = dst[4] = x2;
            dst[1] = dst[3] = y1;
            dst[5] = dst[7] = y2;
            QuadToQuad(quad, dst);
        }

        //--------------------------------------------------------------------
        // Check if the equations were solved successfully
        bool IsValid() { return m_trans_dir.IsValid(); }

        //----------------------------------------------------------------
        void Begin(double x, double y, uint len)
        {
            m_iterator = m_trans_dir.Begin(x, y, 1.0);
            double xt = m_iterator.x;
            double yt = m_iterator.y;

            double dx;
            double dy;
            double delta = 1/(double)Scale;
            dx = xt + delta;
            dy = yt;
            m_trans_inv.Transform(&dx, &dy);
            dx -= x;
            dy -= y;
            int sx1 = Basics.UnsignedRound(Scale/Math.Sqrt(dx*dx + dy*dy)) >> subpixel_shift;
            dx = xt;
            dy = yt + delta;
            m_trans_inv.Transform(&dx, &dy);
            dx -= x;
            dy -= y;
            int sy1 = Basics.UnsignedRound(Scale/Math.Sqrt(dx*dx + dy*dy)) >> subpixel_shift;

            x += len;
            xt = x;
            yt = y;
            m_trans_dir.Transform(&xt, &yt);

            dx = xt + delta;
            dy = yt;
            m_trans_inv.Transform(&dx, &dy);
            dx -= x;
            dy -= y;
            int sx2 = Basics.UnsignedRound(Scale/Math.Sqrt(dx*dx + dy*dy)) >> subpixel_shift;
            dx = xt;
            dy = yt + delta;
            m_trans_inv.Transform(&dx, &dy);
            dx -= x;
            dy -= y;
            int sy2 = Basics.UnsignedRound(Scale/Math.Sqrt(dx*dx + dy*dy)) >> subpixel_shift;

            m_scale_x = Dda2LineInterpolator(sx1, sx2, len);
            m_scale_y = Dda2LineInterpolator(sy1, sy2, len);
        }


        //----------------------------------------------------------------
        void ReSynchronize(double xe, double ye, uint len)
        {
            // Assume x1,y1 are equal to the ones At the previous End point 
            int sx1 = m_scale_x.y();
            int sy1 = m_scale_y.y();

            // Calculate transformed Coordinates At x2,y2 
            double xt = xe;
            double yt = ye;
            m_trans_dir.Transform(&xt, &yt);

            double delta = 1/(double)Scale;
            double dx;
            double dy;

            // Calculate Scale by X At x2,y2
            dx = xt + delta;
            dy = yt;
            m_trans_inv.Transform(&dx, &dy);
            dx -= xe;
            dy -= ye;
            int sx2 = Basics.UnsignedRound(Scale/Math.Sqrt(dx*dx + dy*dy)) >> subpixel_shift;

            // Calculate Scale by Y At x2,y2
            dx = xt;
            dy = yt + delta;
            m_trans_inv.Transform(&dx, &dy);
            dx -= xe;
            dy -= ye;
            int sy2 = Basics.UnsignedRound(Scale/Math.Sqrt(dx*dx + dy*dy)) >> subpixel_shift;

            // Initialize the interpolators
            m_scale_x = Dda2LineInterpolator(sx1, sx2, len);
            m_scale_y = Dda2LineInterpolator(sy1, sy2, len);
        }



        //----------------------------------------------------------------
        void operator++()
        {
            ++m_iterator;
            ++m_scale_x;
            ++m_scale_y;
        }

        //----------------------------------------------------------------
        void Coordinates(int* x, int* y)
        {
            *x = Basics.Round(m_iterator.x * Scale);
            *y = Basics.Round(m_iterator.y * Scale);
        }

        //----------------------------------------------------------------
        void LocalScale(int* x, int* y)
        {
            *x = m_scale_x.y();
            *y = m_scale_y.y();
        }

        //----------------------------------------------------------------
        void Transform(double[] x, double[] y)
        {
            m_trans_dir.Transform(x, y);
        }
        
    private:
        trans_type             m_trans_dir;
        trans_type             m_trans_inv;
        iterator_type          m_iterator;
        Dda2LineInterpolator m_scale_x;
        Dda2LineInterpolator m_scale_y;
    };
     */


    //============================================PerspectiveLerpSpanInterpolator
    //template<uint SubpixelShift = 8> 
    public class PerspectiveLerpSpanInterpolator : ISpanInterpolator
    {
        Transform.Perspective             m_trans_dir;
        Transform.Perspective m_trans_inv;
        Dda2LineInterpolator m_coord_x;
        Dda2LineInterpolator m_coord_y;
        Dda2LineInterpolator m_scale_x;
        Dda2LineInterpolator m_scale_y;

        const int subpixel_shift = 8;
        const int subpixel_scale = 1 << subpixel_shift;

        //--------------------------------------------------------------------
        public PerspectiveLerpSpanInterpolator()
        {
            m_trans_dir = new Transform.Perspective();
            m_trans_inv = new Transform.Perspective();
        }

        //--------------------------------------------------------------------
        // Arbitrary quadrangle transformations
        public PerspectiveLerpSpanInterpolator(double[] src, double[] dst) 
            : this()
        {
            QuadToQuad(src, dst);
        }

        //--------------------------------------------------------------------
        // Direct transformations 
        public PerspectiveLerpSpanInterpolator(double x1, double y1, 
                                     double x2, double y2,
                                     double[] quad)
            : this()
        {
            RectangleToQuad(x1, y1, x2, y2, quad);
        }

        //--------------------------------------------------------------------
        // Reverse transformations 
        public PerspectiveLerpSpanInterpolator(double[] quad, 
                                     double x1, double y1,
                                     double x2, double y2)
            : this()
        {
            QuadToRectangle(quad, x1, y1, x2, y2);
        }

        //--------------------------------------------------------------------
        // Set the transformations using two arbitrary quadrangles.
        public void QuadToQuad(double[] src, double[] dst)
        {
            m_trans_dir.QuadToQuad(src, dst);
            m_trans_inv.QuadToQuad(dst, src);
        }

        //--------------------------------------------------------------------
        // Set the direct transformations, i.e., rectangle -> quadrangle
        public void RectangleToQuad(double x1, double y1, double x2, double y2, double[] quad)
        {
            double[] src = new double[8];
            src[0] = src[6] = x1;
            src[2] = src[4] = x2;
            src[1] = src[3] = y1;
            src[5] = src[7] = y2;
            QuadToQuad(src, quad);
        }


        //--------------------------------------------------------------------
        // Set the reverse transformations, i.e., quadrangle -> rectangle
        public void QuadToRectangle(double[] quad, 
                          double x1, double y1, double x2, double y2)
        {
            double[] dst = new double[8];
            dst[0] = dst[6] = x1;
            dst[2] = dst[4] = x2;
            dst[1] = dst[3] = y1;
            dst[5] = dst[7] = y2;
            QuadToQuad(quad, dst);
        }

        //--------------------------------------------------------------------
        // Check if the equations were solved successfully
        public bool IsValid
        {
            get
            {
                return m_trans_dir.IsValid();
            }
        }

        //----------------------------------------------------------------
        public void Begin(double x, double y, uint len)
        {
            // Calculate transformed Coordinates At x1,y1 
            double xt = x;
            double yt = y;
            m_trans_dir.Transform(ref xt, ref yt);
            int x1 = Basics.Round(xt * subpixel_scale);
            int y1 = Basics.Round(yt * subpixel_scale);

            double dx;
            double dy;
            double delta = 1/(double)subpixel_scale;

            // Calculate Scale by X At x1,y1
            dx = xt + delta;
            dy = yt;
            m_trans_inv.Transform(ref dx, ref dy);
            dx -= x;
            dy -= y;
            int sx1 = (int)Basics.UnsignedRound(subpixel_scale / Math.Sqrt(dx * dx + dy * dy)) >> subpixel_shift;

            // Calculate Scale by Y At x1,y1
            dx = xt;
            dy = yt + delta;
            m_trans_inv.Transform(ref dx, ref dy);
            dx -= x;
            dy -= y;
            int sy1 = (int)Basics.UnsignedRound(subpixel_scale / Math.Sqrt(dx * dx + dy * dy)) >> subpixel_shift;

            // Calculate transformed Coordinates At x2,y2 
            x += len;
            xt = x;
            yt = y;
            m_trans_dir.Transform(ref xt, ref yt);
            int x2 = Basics.Round(xt * subpixel_scale);
            int y2 = Basics.Round(yt * subpixel_scale);

            // Calculate Scale by X At x2,y2
            dx = xt + delta;
            dy = yt;
            m_trans_inv.Transform(ref dx, ref dy);
            dx -= x;
            dy -= y;
            int sx2 = (int)Basics.UnsignedRound(subpixel_scale / Math.Sqrt(dx * dx + dy * dy)) >> subpixel_shift;

            // Calculate Scale by Y At x2,y2
            dx = xt;
            dy = yt + delta;
            m_trans_inv.Transform(ref dx, ref dy);
            dx -= x;
            dy -= y;
            int sy2 = (int)Basics.UnsignedRound(subpixel_scale / Math.Sqrt(dx * dx + dy * dy)) >> subpixel_shift;

            // Initialize the interpolators
            m_coord_x = new Dda2LineInterpolator(x1,  x2,  (int)len);
            m_coord_y = new Dda2LineInterpolator(y1, y2, (int)len);
            m_scale_x = new Dda2LineInterpolator(sx1, sx2, (int)len);
            m_scale_y = new Dda2LineInterpolator(sy1, sy2, (int)len);
        }


        //----------------------------------------------------------------
        public void ReSynchronize(double xe, double ye, uint len)
        {
            // Assume x1,y1 are equal to the ones At the previous End point 
            int x1  = m_coord_x.y();
            int y1  = m_coord_y.y();
            int sx1 = m_scale_x.y();
            int sy1 = m_scale_y.y();

            // Calculate transformed Coordinates At x2,y2 
            double xt = xe;
            double yt = ye;
            m_trans_dir.Transform(ref xt, ref yt);
            int x2 = Basics.Round(xt * subpixel_scale);
            int y2 = Basics.Round(yt * subpixel_scale);

            double delta = 1 / (double)subpixel_scale;
            double dx;
            double dy;

            // Calculate Scale by X At x2,y2
            dx = xt + delta;
            dy = yt;
            m_trans_inv.Transform(ref dx, ref dy);
            dx -= xe;
            dy -= ye;
            int sx2 = (int)Basics.UnsignedRound(subpixel_scale/Math.Sqrt(dx*dx + dy*dy)) >> subpixel_shift;

            // Calculate Scale by Y At x2,y2
            dx = xt;
            dy = yt + delta;
            m_trans_inv.Transform(ref dx, ref dy);
            dx -= xe;
            dy -= ye;
            int sy2 = (int)Basics.UnsignedRound(subpixel_scale/Math.Sqrt(dx*dx + dy*dy)) >> subpixel_shift;

            // Initialize the interpolators
            m_coord_x = new Dda2LineInterpolator(x1,  x2,  (int)len);
            m_coord_y = new Dda2LineInterpolator(y1, y2, (int)len);
            m_scale_x = new Dda2LineInterpolator(sx1, sx2, (int)len);
            m_scale_y = new Dda2LineInterpolator(sy1, sy2, (int)len);
        }

        public Transform.ITransform Transformer
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        //----------------------------------------------------------------
        public void Next()
        {
            m_coord_x.Next();
            m_coord_y.Next();
            m_scale_x.Next();
            m_scale_y.Next();
        }

        //----------------------------------------------------------------
        public void Coordinates(out int x, out int y)
        {
            x = m_coord_x.y();
            y = m_coord_y.y();
        }

        //----------------------------------------------------------------
        public void LocalScale(out int x, out int y)
        {
            x = m_scale_x.y();
            y = m_scale_y.y();
        }

        //----------------------------------------------------------------
        public void Transform(ref double x, ref double y)
        {
            m_trans_dir.Transform(ref x, ref y);
        }
    };
}