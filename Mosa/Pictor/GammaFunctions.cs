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
    public interface IGammaFunction
    {
        double GetGamma(double x);
    };

    public struct GammaNone : IGammaFunction
    {
        public double GetGamma(double x) { return x; }
    };


    //==============================================================GammaPower
    public class GammaPower : IGammaFunction
    {
        public GammaPower() { m_gamma=1.0; }
        public GammaPower(double g)  {m_gamma=g;}

        public void gamma(double g) { m_gamma = g; }
        public double gamma() { return m_gamma; }

        public double GetGamma(double x)
        {
            return Math.Pow(x, m_gamma);
        }

        double m_gamma;
    };


    //==========================================================GammaThreshold
    public class GammaThreshold : IGammaFunction
    {
        public GammaThreshold() {m_threshold=0.5;}
        public GammaThreshold(double t) {m_threshold=t;}

        public void Threshold(double t) { m_threshold = t; }
        public double Threshold() { return m_threshold; }

        public double GetGamma(double x)
        {
            return (x < m_threshold) ? 0.0 : 1.0;
        }

        double m_threshold;
    };


    //============================================================GammaLinear
    public class GammaLinear : IGammaFunction
    {
        public GammaLinear()  
        {
            m_start=(0.0);
            m_end=(1.0);
        }
        public GammaLinear(double s, double e)
        {
            m_start=(s);
            m_end=(e);
        }

        public void Set(double s, double e) { m_start = s; m_end = e; }
        public void Start(double s) { m_start = s; }
        public void End(double e) { m_end = e; }
        public double Start() { return m_start; }
        public double End() { return m_end; }

        public double GetGamma(double x)
        {
            if(x < m_start) return 0.0;
            if(x > m_end) return 1.0;
            double EndMinusStart = m_end - m_start;
            if(EndMinusStart != 0)
                return (x - m_start) / EndMinusStart;
            else
                return 0.0;
        }

        double m_start;
        double m_end;
    };


    //==========================================================GammaMultiply
    public class GammaMultiply : IGammaFunction
    {
        public GammaMultiply() 
        {
            m_mul=(1.0);
        }
        public GammaMultiply(double v) 
        {
            m_mul=(v);
        }

        public void Value(double v) { m_mul = v; }
        public double Value() { return m_mul; }

        public double GetGamma(double x)
        {
            double y = x * m_mul;
            if(y > 1.0) y = 1.0;
            return y;
        }

        double m_mul;
    };
}