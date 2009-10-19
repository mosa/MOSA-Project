/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

namespace Pictor
{
    public interface IGradient
    {
        int Calculate(int x, int y, int d);
    };

    public interface IColorFunction
    {
        int Size
        {
            get;
        }
        RGBA_Bytes this[int v]
        {
            get;
        }
    };

    //==========================================================SpanGradient
    public class SpanGradient : ISpanGenerator
    {
        public const int gradientSubpixelShift = 4;                              //-----gradientSubpixelShift
        public const int gradientSubpixelScale = 1 << gradientSubpixelShift;   //-----gradientSubpixelScale
        public const int gradientSubpixelMask  = gradientSubpixelScale - 1;    //-----gradientSubpixelMask

        public const int  subpixelShift = 8;

        public const int downscale_shift = subpixelShift - gradientSubpixelShift;

        ISpanInterpolator m_interpolator;
        IGradient   m_gradient_function;
        IColorFunction      m_color_function;
        int                m_d1;
        int                m_d2;

        //--------------------------------------------------------------------
        public SpanGradient() {}

        //--------------------------------------------------------------------
        public SpanGradient(ISpanInterpolator inter,
                      IGradient gradient_function,
                      IColorFunction color_function,
                      double d1, double d2)
        {
            m_interpolator = inter;
            m_gradient_function = gradient_function;
            m_color_function = color_function;
            m_d1 = (Basics.Round(d1 * gradientSubpixelScale));
            m_d2 = (Basics.Round(d2 * gradientSubpixelScale));
        }

        //--------------------------------------------------------------------
        public ISpanInterpolator Interpolator
        {
            get { return m_interpolator; }
            set { m_interpolator = value; }
        }

        public IGradient GradientFunction
        {
            get { return m_gradient_function; }
            set { m_gradient_function = value; }
        }

        public IColorFunction ColorFunction
        {
            get { return m_color_function; }
            set { m_color_function = value; }
        }

        public double d1
        {
            get { return (double)(m_d1) / gradientSubpixelScale; }
            set { m_d1 = Basics.Round(value * gradientSubpixelScale); }
        }

        public double d2
        {
            get { return (double)(m_d2) / gradientSubpixelScale; }
            set { m_d2 = Basics.Round(value * gradientSubpixelScale); }
        }

        //--------------------------------------------------------------------
        public void Prepare() {}

        //--------------------------------------------------------------------
        public unsafe void Generate(RGBA_Bytes* span, int x, int y, uint len)
        {   
            int dd = m_d2 - m_d1;
            if(dd < 1) dd = 1;
            m_interpolator.Begin(x + 0.5, y + 0.5, len);
            do
            {
                m_interpolator.Coordinates(out x, out y);
                int d = m_gradient_function.Calculate(x >> downscale_shift, 
                                                       y >> downscale_shift, m_d2);
                d = ((d - m_d1) * (int)m_color_function.Size) / dd;
                if(d < 0) d = 0;
                if (d >= (int)m_color_function.Size)
                {
                    d = m_color_function.Size - 1;
                }

                *span++ = m_color_function[d];
                m_interpolator.Next();
            }
            while(--len != 0);
        }
    };

    //=====================================================LinearColorGradient
    public struct LinearColorGradient : IColorFunction
    {
        RGBA_Bytes m_c1;
        RGBA_Bytes m_c2;
        int m_size;

        public LinearColorGradient(RGBA_Bytes c1, RGBA_Bytes c2)
            : this(c1, c2, 256)
        {
        }

        public LinearColorGradient(RGBA_Bytes c1, RGBA_Bytes c2, int size)
        {
            m_c1=c1;
            m_c2=c2;
            m_size=size;
        }

        public int Size
        {
            get
            {
                return m_size;
            }
        }

        public RGBA_Bytes this[int v] 
        {
            get
            {
                return m_c1.Gradient(m_c2, (double)(v) / (double)(m_size - 1));
            }
        }

        public void Colors(RGBA_Bytes c1, RGBA_Bytes c2)
        {
            Colors(c1, c2, 256);
        }

        public void Colors(RGBA_Bytes c1, RGBA_Bytes c2, int size)
        {
            m_c1 = c1;
            m_c2 = c2;
            m_size = size;
        }
    };

    //==========================================================CircleGradient
    public class CircleGradient : IGradient
    {
        // Actually the same as radial. Just for compatibility
        public int Calculate(int x, int y, int d)
        {
            return (int)(PictorMath.FastSqrt((uint)(x*x + y*y)));
        }
    };


    //==========================================================RadialGradient
    public class RadialGradient : IGradient
    {
        public int Calculate(int x, int y, int d)
        {
            //return (int)(System.Math.Sqrt((uint)(x * x + y * y)));
            return (int)(PictorMath.FastSqrt((uint)(x * x + y * y)));
        }
    };

    //========================================================RadialGradientD
    public class RadialGradientD : IGradient
    {
        public int Calculate(int x, int y, int d)
        {
            return (int)Basics.UnsignedRound(System.Math.Sqrt((double)(x)*(double)(x) + (double)(y)*(double)(y)));
        }
    };

    //====================================================RadialFocusGradient
    public class RadialFocusGradient : IGradient
    {
        int    m_r;
        int    m_fx;
        int    m_fy;
        double m_r2;
        double m_fx2;
        double m_fy2;
        double m_mul;
    
        //---------------------------------------------------------------------
        public RadialFocusGradient()
        {
            m_r = (100 * SpanGradient.gradientSubpixelScale);
            m_fx=(0);
            m_fy=(0);
            UpdateValues();
        }

        //---------------------------------------------------------------------
        public RadialFocusGradient(double r, double fx, double fy)
        {
            m_r = (Basics.Round(r * SpanGradient.gradientSubpixelScale));
            m_fx = (Basics.Round(fx * SpanGradient.gradientSubpixelScale));
            m_fy = (Basics.Round(fy * SpanGradient.gradientSubpixelScale));
            UpdateValues();
        }

        //---------------------------------------------------------------------
        public void Init(double r, double fx, double fy)
        {
            m_r = Basics.Round(r * SpanGradient.gradientSubpixelScale);
            m_fx = Basics.Round(fx * SpanGradient.gradientSubpixelScale);
            m_fy = Basics.Round(fy * SpanGradient.gradientSubpixelScale);
            UpdateValues();
        }

        //---------------------------------------------------------------------
        public double Radius()  { return (double)(m_r)  / SpanGradient.gradientSubpixelScale; }
        public double focus_x() { return (double)(m_fx) / SpanGradient.gradientSubpixelScale; }
        public double focus_y() { return (double)(m_fy) / SpanGradient.gradientSubpixelScale; }

        //---------------------------------------------------------------------
        public int Calculate(int x, int y, int d)
        {
            double dx = x - m_fx;
            double dy = y - m_fy;
            double d2 = dx * m_fy - dy * m_fx;
            double d3 = m_r2 * (dx * dx + dy * dy) - d2 * d2;
            return Basics.Round((dx * m_fx + dy * m_fy + System.Math.Sqrt(System.Math.Abs(d3))) * m_mul);
        }

        //---------------------------------------------------------------------
        private void UpdateValues()
        {
            // Calculate the invariant values. In case the focal center
            // lies exactly on the Gradient circle the divisor degenerates
            // into zero. In this case we just move the focal center by
            // one subpixel unit possibly in the direction to the origin (0,0)
            // and Calculate the values again.
            //-------------------------
            m_r2  = (double)(m_r)  * (double)(m_r);
            m_fx2 = (double)(m_fx) * (double)(m_fx);
            m_fy2 = (double)(m_fy) * (double)(m_fy);
            double d = (m_r2 - (m_fx2 + m_fy2));
            if(d == 0)
            {
                if(m_fx != 0) 
                {
                    if(m_fx < 0) ++m_fx; else --m_fx; 
                }

                if(m_fy != 0)
                {
                    if(m_fy < 0) ++m_fy; else --m_fy; 
                }

                m_fx2 = (double)(m_fx) * (double)(m_fx);
                m_fy2 = (double)(m_fy) * (double)(m_fy);
                d = (m_r2 - (m_fx2 + m_fy2));
            }
            m_mul = m_r / d;
        }
    };


    //==============================================================xGradient
    public class xGradient : IGradient
    {
        public int Calculate(int x, int y, int d) { return x; }
    };


    //==============================================================yGradient
    public class yGradient : IGradient
    {
        public int Calculate(int x, int y, int d) { return y; }
    };

    //========================================================DiamondGradient
    public class DiamondGradient : IGradient
    {
        public int Calculate(int x, int y, int d) 
        { 
            int ax = System.Math.Abs(x);
            int ay = System.Math.Abs(y);
            return ax > ay ? ax : ay; 
        }
    };

    //=============================================================xyGradient
    public class xyGradient : IGradient
    {
        public int Calculate(int x, int y, int d) 
        {
            return System.Math.Abs(x) * System.Math.Abs(y) / d; 
        }
    };

    //========================================================SquareRootXYGradient
    public class SquareRootXYGradient : IGradient
    {
        public int Calculate(int x, int y, int d) 
        {
            //return (int)System.Math.Sqrt((uint)(System.Math.Abs(x) * System.Math.Abs(y)));
            return (int)PictorMath.FastSqrt((uint)(System.Math.Abs(x) * System.Math.Abs(y))); 
        }
    };

    //==========================================================ConicGradient
    public class ConicGradient : IGradient
    {
        public int Calculate(int x, int y, int d) 
        { 
            return (int)Basics.UnsignedRound(System.Math.Abs(System.Math.Atan2((double)(y), (double)(x))) * (double)(d) / System.Math.PI);
        }
    };

    //=================================================GradientRepeatAdaptor
    public class GradientRepeatAdaptor : IGradient
    {
        IGradient m_gradient;

        public GradientRepeatAdaptor(IGradient gradient)
        {
            m_gradient= gradient;
        }
            

        public int Calculate(int x, int y, int d)
        {
            int ret = m_gradient.Calculate(x, y, d) % d;
            if(ret < 0) ret += d;
            return ret;
        }
    };

    //================================================GradientReflectAdaptor
    public class GradientReflectAdaptor : IGradient
    {
        IGradient m_gradient;

        public GradientReflectAdaptor(IGradient gradient)
        {
            m_gradient = gradient;
        }

        public int Calculate(int x, int y, int d)
        {
            int d2 = d << 1;
            int ret = m_gradient.Calculate(x, y, d) % d2;
            if(ret <  0) ret += d2;
            if(ret >= d) ret  = d2 - ret;
            return ret;
        }
    };

    public class GradientClampAdaptor : IGradient
    {
        IGradient m_gradient;

        public GradientClampAdaptor(IGradient gradient)
        {
            m_gradient = gradient;
        }

        public int Calculate(int x, int y, int d)
        {
            int ret = m_gradient.Calculate(x, y, d);
            if (ret < 0) ret = 0;
            if (ret > d) ret = d;
            return ret;
        }
    };
}