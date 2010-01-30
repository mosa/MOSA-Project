/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */
using System;
using System.IO;

namespace Pictor
{
    static public class DebugFile
    {
        static bool m_FileOpenedOnce = false;

        public static void Print(String message)
        {
            FileStream file;
            if (m_FileOpenedOnce)
            {
                file = new FileStream("test.txt", FileMode.Append, FileAccess.Write);
            }
            else
            {
                file = new FileStream("test.txt", FileMode.Create, FileAccess.Write);
                m_FileOpenedOnce = true;
            }
            StreamWriter sw = new StreamWriter(file);
            sw.Write(message);
            sw.Close();
            file.Close();
        }
    };
    
    static public class Basics
    {
        //----------------------------------------------------------FillingRule
        public enum FillingRule
        {
            NonZero,
            EvenOdd
        };

        public static unsafe void memcpy(Byte* pDest, Byte* pSource, int Count)
        {
            for (uint i = 0; i < Count; i++)
            {
                *pDest++ = *pSource++;
            }
        }

        public static unsafe void memmove(Byte* pDest, Byte* pSource, int Count)
        {
            if (pSource > pDest
                || &pSource[Count] < pDest)
            {
                for (uint i = 0; i < Count; i++)
                {
                    *pDest++ = *pSource++;
                }
            }
            else
            {
                throw new System.NotImplementedException();
            }
            
        }

        public static unsafe void memset(Byte* pDest, byte ByteVal, int Count)
        {
            // the fill is optomized to fill using dwords, you have to pass a valid dword
            uint Val = (uint)((uint)ByteVal << 24) + (uint)(ByteVal << 16) + (uint)(ByteVal << 8) + (uint)ByteVal;

            // dword align to dest
            while (((uint)pDest & 3) != 0
                && Count > 0)
            {
                *pDest++ = ByteVal;
                Count--;
            }

            int NumLongs = Count / 4;

            while (NumLongs-- > 0)
            {
                *((uint*)pDest) = Val;

                pDest += 4;
            }

            switch (Count & 3)
            {
                case 3:
                    pDest[2] = ByteVal;
                    goto case 2;
                case 2:
                    pDest[1] = ByteVal;
                    goto case 1;
                case 1:
                    pDest[0] = ByteVal;
                    break;
            }
        }

        public static unsafe void MemClear(Byte* pDest, int Count)
        {
            // dword align to dest
            while (((uint)pDest & 3) != 0
                && Count > 0)
            {
                *pDest++ = 0;
                Count--;
            }

            int NumLongs = Count / 4;

            while (NumLongs-- > 0)
            {
                *((uint*)pDest) = 0;

                pDest += 4;
            }

            switch (Count & 3)
            {
                case 3:
                    pDest[2] = 0;
                    goto case 2;
                case 2:
                    pDest[1] = 0;
                    goto case 1;
                case 1:
                    pDest[0] = 0;
                    break;
            }
        }

        //------------------------------------------------------------IsEqualEps
        //template<class T> 
        public static bool IsEqualEps(double v1, double v2, double epsilon)
        {
            return Math.Abs(v1 - v2) <= (double)(epsilon);
        }

        //------------------------------------------------------------------Deg2Rad
        public static double Deg2Rad(double deg)
        {
            return deg * Math.PI / 180.0;
        }

        //------------------------------------------------------------------Rad2Deg
        public static double Rad2Deg(double rad)
        {
            return rad * 180.0 / Math.PI;
        }

        public static int Round(double v)
        {
            return (int)((v < 0.0) ? v - 0.5 : v + 0.5);
        }
        
        public static int Round(double v, int saturationLimit)
        {
            if(v < (double)(-saturationLimit)) return -saturationLimit;
            if(v > (double)( saturationLimit)) return  saturationLimit;
            return Round(v);
        }
        
        public static uint UnsignedRound(double v)
        {
            return (uint)(v + 0.5);
        }
           
        public static uint UnsignedFloor(double v)
        {
            return (uint)(v);
        }
            
        public static uint UnsignedCeiling(double v)
        {
            return (uint)(Math.Ceiling(v));
        }

        //----------------------------------------------------PolySubpixelScale
        // These constants determine the subpixel accuracy, to be more precise, 
        // the number of bits of the fractional part of the Coordinates. 
        // The possible coordinate capacity in bits can be calculated by formula:
        // sizeof(int) * 8 - Shift, i.e, for 32-bit integers and
        // 8-bits fractional part the capacity is 24 bits.
        public enum PolySubpixelScale
        {
            Shift = 8,                      //----Shift
            Scale = 1 << Shift, //----Scale 
            Mask = Scale - 1,  //----Mask 
        };

    };

    public struct RectI
    {
        private int m_x1, m_y1, m_x2, m_y2;

        public RectI(int x1_, int y1_, int x2_, int y2_)
        {
            m_x1 = x1_;
            m_y1 = y1_;
            m_x2 = x2_;
            m_y2 = y2_;
        }

        public void Init(int x1_, int y1_, int x2_, int y2_) 
        {
            x1 = x1_;
            y1 = y1_;
            x2 = x2_;
            y2 = y2_;
        }

        public int x1
        {
            get
            {
                return m_x1;
            }
            set
            {
                m_x1 = value;
            }
        }

        public int y1
        {
            get
            {
                return m_y1;
            }
            set
            {
                m_y1 = value;
            }
        }

        public int x2
        {
            get
            {
                return m_x2;
            }
            set
            {
                m_x2 = value;
            }
        }

        public int y2
        {
            get
            {
                return m_y2;
            }
            set
            {
                m_y2 = value;
            }
        }

        public RectI Normalize()
        {
            int t;
            if (x1 > x2) { t = x1; x1 = x2; x2 = t; }
            if (y1 > y2) { t = y1; y1 = y2; y2 = t; }
            return this;
        }

        public bool Clip(RectI r)
        {
            if (x2 > r.x2) x2 = r.x2;
            if (y2 > r.y2) y2 = r.y2;
            if (x1 < r.x1) x1 = r.x1;
            if (y1 < r.y1) y1 = r.y1;
            return x1 <= x2 && y1 <= y2;
        }

        public bool IsValid
        {
            get { return x1 <= x2 && y1 <= y2; }
        }

        public bool hit_test(int x, int y)
        {
            return (x >= x1 && x <= x2 && y >= y1 && y <= y2);
        }

        //-----------------------------------------------------IntersectRectangles
        public void IntersectRectangles(RectI r1, RectI r2)
        {
            x1 = r1.x1;
            y1 = r1.y1;
            x2 = r1.x2;
            x2 = r1.y2;
            // First process m_x2,m_y2 because the other order 
            // results in Internal Compiler Error under 
            // Microsoft Visual C++ .NET 2003 69462-335-0000007-18038 in 
            // case of "Maximize Speed" optimization option.
            //-----------------
            if (x2 > r2.x2) x2 = r2.x2;
            if (y2 > r2.y2) y2 = r2.y2;
            if (x1 < r2.x1) x1 = r2.x1;
            if (y1 < r2.y1) y1 = r2.y1;
        }


        //---------------------------------------------------------UniteRectangles
        public void UniteRectangles(RectI r1, RectI r2)
        {
            x1 = r1.x1;
            y1 = r1.y1;
            x2 = r1.x2;
            x2 = r1.y2;
            if (x2 < r2.x2) x2 = r2.x2;
            if (y2 < r2.y2) y2 = r2.y2;
            if (x1 > r2.x1) x1 = r2.x1;
            if (y1 > r2.y1) y1 = r2.y1;
        }
    };

    public struct RectD
    {
        private double m_x1, m_y1, m_x2, m_y2;

        public RectD(double left, double bottom, double right, double top)
        {
            m_x1 = left;
            m_y1 = bottom;
            m_x2 = right;
            m_y2 = top;
        }

        public void Init(double left, double bottom, double right, double top)
        {
            m_x1 = left;
            m_y1 = bottom;
            m_x2 = right;
            m_y2 = top;
        }

        public double x1
        {
            get
            {
                return m_x1;
            }
            set
            {
                m_x1 = value;
            }
        }

        public double y1
        {
            get
            {
                return m_y1;
            }
            set
            {
                m_y1 = value;
            }
        }

        public double x2
        {
            get
            {
                return m_x2;
            }
            set
            {
                m_x2 = value;
            }
        }

        public double y2
        {
            get
            {
                return m_y2;
            }
            set
            {
                m_y2 = value;
            }
        }

        public double Left
        {
            get
            {
                return m_x1;
            }
            set
            {
                m_x1 = value;
            }
        }

        public double Bottom
        {
            get
            {
                return m_y1;
            }
            set
            {
                m_y1 = value;
            }
        }

        public double Right
        {
            get
            {
                return m_x2;
            }
            set
            {
                m_x2 = value;
            }
        }

        public double Top
        {
            get
            {
                return m_y2;
            }
            set
            {
                m_y2 = value;
            }
        }

        // This function assumes the Rectangle is normalized
        public double Width
        {
            get
            {
                return Right - Left;
            }
        }

        // This function assumes the Rectangle is normalized
        public double Height
        {
            get
            {
                return Top - Bottom;
            }
        }

        public RectD Normalize()
        {
            double t;
            if (m_x1 > m_x2) { t = m_x1; m_x1 = m_x2; m_x2 = t; }
            if (m_y1 > m_y2) { t = m_y1; m_y1 = m_y2; m_y2 = t; }
            return this;
        }

        public bool Clip(RectD r)
        {
            if (m_x2 > r.x2) m_x2 = r.x2;
            if (m_y2 > r.y2) m_y2 = r.y2;
            if (m_x1 < r.x1) m_x1 = r.x1;
            if (m_y1 < r.y1) m_y1 = r.y1;
            return m_x1 <= m_x2 && m_y1 <= m_y2;
        }

        public bool IsValid
        {
            get { return m_x1 <= m_x2 && m_y1 <= m_y2; }
        }

        public bool HitTest(double x, double y)
        {
            return (x >= m_x1 && x <= m_x2 && y >= m_y1 && y <= m_y2);
        }

        //-----------------------------------------------------IntersectRectangles
        public void IntersectRectangles(RectD r1, RectD r2)
        {
            m_x1 = r1.x1;
            m_y1 = r1.y1;
            m_x2 = r1.x2;
            m_x2 = r1.y2;
            // First process m_x2,m_y2 because the other order 
            // results in Internal Compiler Error under 
            // Microsoft Visual C++ .NET 2003 69462-335-0000007-18038 in 
            // case of "Maximize Speed" optimization option.
            //-----------------
            if (m_x2 > r2.x2) m_x2 = r2.x2;
            if (m_y2 > r2.y2) m_y2 = r2.y2;
            if (m_x1 < r2.x1) m_x1 = r2.x1;
            if (m_y1 < r2.y1) m_y1 = r2.y1;
        }


        //---------------------------------------------------------UniteRectangles
        public void UniteRectangles(RectD r1, RectD r2)
        {
            m_x1 = r1.x1;
            m_y1 = r1.y1;
            m_x2 = r1.x2;
            m_x2 = r1.y2;
            if (m_x2 < r2.x2) m_x2 = r2.x2;
            if (m_y2 < r2.y2) m_y2 = r2.y2;
            if (m_x1 > r2.x1) m_x1 = r2.x1;
            if (m_y1 > r2.y1) m_y1 = r2.y1;
        }

        public void Inflate(double inflateSize)
        {
            Left = Left - inflateSize;
            Bottom = Bottom - inflateSize;
            Right = Right + inflateSize;
            Top = Top + inflateSize;
        }
    };

    public static class Path
    {
        //---------------------------------------------------------EPathCommands
        public enum EPathCommands
        {
            Stop     = 0,        //----Stop    
            MoveTo  = 1,        //----MoveTo 
            LineTo  = 2,        //----LineTo 
            Curve3   = 3,        //----Curve3  
            Curve4   = 4,        //----Curve4  
            CurveN   = 5,        //----CurveN
            Catrom   = 6,        //----Catrom
            UBSpline = 7,        //----UBSpline
            EndPoly = 0x0F,     //----EndPoly
            Mask     = 0x0F      //----Mask    
        };

        //------------------------------------------------------------EPathFlags
        public enum EPathFlags
        {
            None  = 0,         //----None 
            CounterClockwise   = 0x10,      //----CounterClockwise  
            Clockwise    = 0x20,      //----Clockwise   
            Close = 0x40,      //----Close
            Mask  = 0xF0       //----Mask 
        };

        //---------------------------------------------------------------IsVertex
        public static bool IsVertex(uint c)
        {
            return (uint)c >= (uint)EPathCommands.MoveTo 
                && (uint)c < (uint)EPathCommands.EndPoly;
        }

        //--------------------------------------------------------------IsDrawing
        public static bool IsDrawing(uint c)
        {
            return c >= (uint)EPathCommands.LineTo && c < (uint)EPathCommands.EndPoly;
        }

        //-----------------------------------------------------------------IsStop
        public static bool IsStop(uint c)
        {
            return c == (uint)EPathCommands.Stop;
        }

        //--------------------------------------------------------------IsMoveTo
        public static bool IsMoveTo(uint c)
        {
            return c == (uint)EPathCommands.MoveTo;
        }

        //--------------------------------------------------------------IsLineTo
        public static bool IsLineTo(uint c)
        {
            return c == (uint)EPathCommands.LineTo;
        }

        //----------------------------------------------------------------IsCurve
        public static bool IsCurve(uint c)
        {
            return c == (uint)EPathCommands.Curve3 
                || c == (uint)EPathCommands.Curve4;
        }

        //---------------------------------------------------------------IsCurve3
        public static bool IsCurve3(uint c)
        {
            return c == (uint)EPathCommands.Curve3;
        }

        //---------------------------------------------------------------IsCurve4
        public static bool IsCurve4(uint c)
        {
            return c == (uint)EPathCommands.Curve4;
        }

        //-------------------------------------------------------------IsEndPoly
        public static bool IsEndPoly(uint c)
        {
            return (c & (uint)EPathCommands.Mask) == (uint)EPathCommands.EndPoly;
        }

        //----------------------------------------------------------------IsClose
        public static bool IsClose(uint c)
        {
            return ((int)c & ~(int)(EPathFlags.Clockwise | EPathFlags.CounterClockwise)) ==
                   ((uint)EPathCommands.EndPoly | (uint)EPathFlags.Close); 
        }

        //------------------------------------------------------------IsNextPoly
        public static bool IsNextPoly(uint c)
        {
            return IsStop(c) || IsMoveTo(c) || IsEndPoly(c);
        }

        //-------------------------------------------------------------------IsClockwise
        public static bool IsClockwise(uint c)
        {
            return (c & (uint)EPathFlags.Clockwise) != 0;
        }

        //------------------------------------------------------------------IsCounterClockwise
        public static bool IsCounterClockwise(uint c)
        {
            return (c & (uint)EPathFlags.CounterClockwise) != 0;
        }

        //-------------------------------------------------------------IsOriented
        public static bool IsOriented(uint c)
        {
            return (c & ((uint)EPathFlags.Clockwise | (uint)EPathFlags.CounterClockwise)) != 0; 
        }

        //---------------------------------------------------------------IsClosed
        public static bool IsClosed(EPathFlags c)
        {
            return (c & EPathFlags.Close) != 0; 
        }

        //----------------------------------------------------------GetCloseFlag
        public static EPathFlags GetCloseFlag(uint c)
        {
            return (EPathFlags)(c & (uint)EPathFlags.Close); 
        }

        //-------------------------------------------------------ClearOrientation
        public static EPathFlags ClearOrientation(EPathFlags c)
        {
            return c & ~(EPathFlags.Clockwise | EPathFlags.CounterClockwise);
        }

        //---------------------------------------------------------GetOrientation
        public static EPathFlags GetOrientation(EPathFlags c)
        {
            return c & (EPathFlags.Clockwise | EPathFlags.CounterClockwise);
        }

        /*
        //---------------------------------------------------------set_orientation
        public static EPathCommands set_orientation(uint c, EPathFlags o)
        {
            return ClearOrientation(c) | o;
        }
         */

        static public void ShortenPath(Pictor.vertex_sequence vs, double s)
        {
            ShortenPath(vs, s, 0);
        }

        static public void ShortenPath(vertex_sequence vs, double s, uint closed)
        {
            //typedef typename VertexSequence::value_type vertex_type;

            if(s > 0.0 && vs.Size() > 1)
            {
                double d;
                int n = (int)(vs.Size() - 2);
                while(n != 0)
                {
                    d = vs[n].dist;
                    if(d > s) break;
                    vs.RemoveLast();
                    s -= d;
                    --n;
                }
                if(vs.Size() < 2)
                {
                    vs.RemoveAll();
                }
                else
                {
                    n = (int)vs.Size() - 1;
                    VertexDistance prev = vs[n - 1];
                    VertexDistance last = vs[n];
                    d = (prev.dist - s) / prev.dist;
                    double x = prev.x + (last.x - prev.x) * d;
                    double y = prev.y + (last.y - prev.y) * d;
                    last.x = x;
                    last.y = y;
                    if (!prev.IsEqual(last)) vs.RemoveLast();
                    vs.close(closed != 0);
                }
            }
        }
    };

    public struct Point
    {
        private int m_X;
        private int m_Y;

        public Point(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        public int X
        {
            get { return m_X; }
            set { m_X = value; }
        }

        public int Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }
    };

    public struct PointD
    {
        public double x, y;
        public PointD(double x_, double y_)
        {
            x = x_;
            y = y_;
        }
    };
}
