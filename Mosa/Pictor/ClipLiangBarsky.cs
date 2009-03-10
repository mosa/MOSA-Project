/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

namespace Pictor
{
    public static class ClipLiangBarsky
    {
        //------------------------------------------------------------------------
        enum ClippingFlags
        {
            x1Clipped = 4,
            x2Clipped = 1,
            y1Clipped = 8,
            y2Clipped = 2,
            xClipped = x1Clipped | x2Clipped,
            yClipped = y1Clipped | y2Clipped
        };

        //----------------------------------------------------------GetClippingFlags
        // Determine the clipping code of the Vertex according to the 
        // Cyrus-Beck Line clipping algorithm
        //
        //        |        |
        //  0110  |  0010  | 0011
        //        |        |
        // -------+--------+-------- ClipBox.y2
        //        |        |
        //  0100  |  0000  | 0001
        //        |        |
        // -------+--------+-------- ClipBox.y1
        //        |        |
        //  1100  |  1000  | 1001
        //        |        |
        //  ClipBox.x1  ClipBox.x2
        //
        // 
        //template<class T>
        public static uint GetClippingFlags(int x, int y, RectI clip_box)
        {
            return (uint)(((x > clip_box.x2) ? 1 : 0) 
                | ((y > clip_box.y2) ? 1 << 1 : 0)
                | ((x < clip_box.x1) ? 1 << 2 : 0)
                | ((y < clip_box.y1) ? 1 << 3 : 0));
        }

        //--------------------------------------------------------ClippingFlagsX
        //template<class T>
        public static uint ClippingFlagsX(int x, RectI clip_box)
        {
            return (uint)((x > clip_box.x2 ? 1 : 0) | ((x < clip_box.x1 ? 1 : 0) << 2));
        }


        //--------------------------------------------------------ClippingFlagsY
        //template<class T>
        public static uint ClippingFlagsY(int y, RectI clip_box)
        {
            return (uint)(((y > clip_box.y2 ? 1 : 0) << 1) | ((y < clip_box.y1 ? 1 : 0) << 3));
        }


        //-------------------------------------------------------ClipLiangbarsky
        //template<class T>
        public static uint ClipLiangbarsky(int x1, int y1, int x2, int y2,
                                          RectI clip_box,
                                          int[] x, int[] y)
        {
            uint XIndex = 0;
            uint YIndex = 0;
            double nearzero = 1e-30;

            double deltax = x2 - x1;
            double deltay = y2 - y1; 
            double xin;
            double xout;
            double yin;
            double yout;
            double tinx;
            double tiny;
            double toutx;
            double touty;  
            double tin1;
            double tin2;
            double tout1;
            uint np = 0;

            if(deltax == 0.0) 
            {   
                // bump off of the vertical
                deltax = (x1 > clip_box.x1) ? -nearzero : nearzero;
            }

            if(deltay == 0.0) 
            { 
                // bump off of the horizontal 
                deltay = (y1 > clip_box.y1) ? -nearzero : nearzero;
            }
            
            if(deltax > 0.0) 
            {                
                // points to right
                xin = clip_box.x1;
                xout = clip_box.x2;
            }
            else 
            {
                xin = clip_box.x2;
                xout = clip_box.x1;
            }

            if(deltay > 0.0) 
            {
                // points up
                yin = clip_box.y1;
                yout = clip_box.y2;
            }
            else 
            {
                yin = clip_box.y2;
                yout = clip_box.y1;
            }
            
            tinx = (xin - x1) / deltax;
            tiny = (yin - y1) / deltay;
            
            if (tinx < tiny) 
            {
                // hits x first
                tin1 = tinx;
                tin2 = tiny;
            }
            else
            {
                // hits y first
                tin1 = tiny;
                tin2 = tinx;
            }
            
            if(tin1 <= 1.0) 
            {
                if(0.0 < tin1) 
                {
                    x[XIndex++] = (int)xin;
                    y[YIndex++] = (int)yin;
                    ++np;
                }

                if(tin2 <= 1.0)
                {
                    toutx = (xout - x1) / deltax;
                    touty = (yout - y1) / deltay;
                    
                    tout1 = (toutx < touty) ? toutx : touty;
                    
                    if(tin2 > 0.0 || tout1 > 0.0) 
                    {
                        if(tin2 <= tout1) 
                        {
                            if(tin2 > 0.0) 
                            {
                                if(tinx > tiny) 
                                {
                                    x[XIndex++] = (int)xin;
                                    y[YIndex++] = (int)(y1 + tinx * deltay);
                                }
                                else 
                                {
                                    x[XIndex++] = (int)(x1 + tiny * deltax);
                                    y[YIndex++] = (int)yin;
                                }
                                ++np;
                            }

                            if(tout1 < 1.0) 
                            {
                                if(toutx < touty) 
                                {
                                    x[XIndex++] = (int)xout;
                                    y[YIndex++] = (int)(y1 + toutx * deltay);
                                }
                                else 
                                {
                                    x[XIndex++] = (int)(x1 + touty * deltax);
                                    y[YIndex++] = (int)yout;
                                }
                            }
                            else 
                            {
                                x[XIndex++] = x2;
                                y[YIndex++] = y2;
                            }
                            ++np;
                        }
                        else 
                        {
                            if(tinx > tiny) 
                            {
                                x[XIndex++] = (int)xin;
                                y[YIndex++] = (int)yout;
                            }
                            else 
                            {
                                x[XIndex++] = (int)xout;
                                y[YIndex++] = (int)yin;
                            }
                            ++np;
                        }
                    }
                }
            }
            return np;
        }


        //----------------------------------------------------------------------------
        //template<class T>
        public static bool ClipMovePoint(int x1, int y1, int x2, int y2, 
                             RectI clip_box, 
                             ref int x, ref int y, uint flags)
        {
           int bound;

           if ((flags & (uint)ClippingFlags.xClipped) != 0)
           {
               if(x1 == x2)
               {
                   return false;
               }
               bound = ((flags & (uint)ClippingFlags.x1Clipped) != 0) ? clip_box.x1 : clip_box.x2;
               y = (int)((double)(bound - x1) * (y2 - y1) / (x2 - x1) + y1);
               x = bound;
           }

           flags = ClippingFlagsY(y, clip_box);
           if ((flags & (uint)ClippingFlags.yClipped) != 0)
           {
               if(y1 == y2)
               {
                   return false;
               }
               bound = ((flags & (uint)ClippingFlags.x1Clipped) != 0) ? clip_box.y1 : clip_box.y2;
               x = (int)((double)(bound - y1) * (x2 - x1) / (y2 - y1) + x1);
               y = bound;
           }
           return true;
        }

        //-------------------------------------------------------ClipLineSegment
        // Returns: ret >= 4        - Fully clipped
        //          (ret & 1) != 0  - First point has been moved
        //          (ret & 2) != 0  - Second point has been moved
        //
        //template<class T>
        public static uint ClipLineSegment(ref int x1, ref int y1, ref int x2, ref int y2,
                                   RectI clip_box)
        {
            uint f1 = GetClippingFlags(x1, y1, clip_box);
            uint f2 = GetClippingFlags(x2, y2, clip_box);
            uint ret = 0;

            if((f2 | f1) == 0)
            {
                // Fully visible
                return 0;
            }

            if ((f1 & (uint)ClippingFlags.xClipped) != 0 &&
               (f1 & (uint)ClippingFlags.xClipped) == (f2 & (uint)ClippingFlags.xClipped))
            {
                // Fully clipped
                return 4;
            }

            if ((f1 & (uint)ClippingFlags.yClipped) != 0 &&
               (f1 & (uint)ClippingFlags.yClipped) == (f2 & (uint)ClippingFlags.yClipped))
            {
                // Fully clipped
                return 4;
            }

            int tx1 = x1;
            int ty1 = y1;
            int tx2 = x2;
            int ty2 = y2;
            if(f1 != 0) 
            {   
                if(!ClipMovePoint(tx1, ty1, tx2, ty2, clip_box, ref x1, ref y1, f1)) 
                {
                    return 4;
                }
                if(x1 == x2 && y1 == y2) 
                {
                    return 4;
                }
                ret |= 1;
            }
            if(f2 != 0) 
            {
                if(!ClipMovePoint(tx1, ty1, tx2, ty2, clip_box, ref x2, ref y2, f2))
                {
                    return 4;
                }
                if(x1 == x2 && y1 == y2) 
                {
                    return 4;
                }
                ret |= 2;
            }
            return ret;
        }
    }
}


//#endif
