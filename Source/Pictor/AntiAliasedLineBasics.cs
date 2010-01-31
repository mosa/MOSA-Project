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
    //---------------------------------------------------------------LineCoordinate
    public struct LineCoordinate
    {
        public static int Convert(double x)
        {
            return (int)Math.Round(x * LineAABasics.line_subpixel_scale);
        }
    };

    //-----------------------------------------------------------SaturatedLineCoordinate
    public struct SaturatedLineCoordinate
    {
        public static int Convert(double x)
        {
            return Basics.Round(x * LineAABasics.line_subpixel_scale, LineAABasics.line_max_coord);
        }
    };

    //==========================================================LineParameters
    public struct LineParameters
    {
        //---------------------------------------------------------------------
        public int x1, y1, x2, y2, dx, dy, sx, sy;
        public bool vertical;
        public int inc;
        public int len;
        public int octant;

        //-------------------------------------------------------------------------
        // The number of the octant is determined as a 3-bit Value as follows:
        // bit 0 = vertical flag
        // bit 1 = sx < 0
        // bit 2 = sy < 0
        //
        // [N] shows the number of the orthogonal quadrant
        // <M> shows the number of the diagonal quadrant
        //               <1>
        //   [1]          |          [0]
        //       . (3)011 | 001(1) .
        //         .      |      .
        //           .    |    . 
        //             .  |  . 
        //    (2)010     .|.     000(0)
        // <2> ----------.+.----------- <0>
        //    (6)110   .  |  .   100(4)
        //           .    |    .
        //         .      |      .
        //       .        |        .
        //         (7)111 | 101(5) 
        //   [2]          |          [3]
        //               <3> 
        //                                                        0,1,2,3,4,5,6,7 
        public static byte[] s_orthogonal_quadrant = { 0, 0, 1, 1, 3, 3, 2, 2 };
        public static byte[] s_diagonal_quadrant = { 0, 1, 2, 1, 0, 3, 2, 3 };

        //---------------------------------------------------------------------
        public LineParameters(int x1_, int y1_, int x2_, int y2_, int len_)
        {
            x1=(x1_);
            y1=(y1_);
            x2=(x2_);
            y2=(y2_);
            dx=(Math.Abs(x2_ - x1_));
            dy=(Math.Abs(y2_ - y1_));
            sx=((x2_ > x1_) ? 1 : -1);
            sy=((y2_ > y1_) ? 1 : -1);
            vertical=(dy >= dx);
            inc=(vertical ? sy : sx);
            len=(len_);
            octant=((sy & 4) | (sx & 2) | (vertical ? 1 : 0));
        }

        //---------------------------------------------------------------------
        public uint OrthogonalQuadrant
        {
            get { return s_orthogonal_quadrant[octant]; }
        }
        public uint DiagonalQuadrant
        {
            get { return s_diagonal_quadrant[octant]; }
        }

        //---------------------------------------------------------------------
        public bool SameOrthogonalQuadrant(LineParameters lp)
        {
            return s_orthogonal_quadrant[octant] == s_orthogonal_quadrant[lp.octant];
        }

        //---------------------------------------------------------------------
        public bool SameDiagonalQuadrant(LineParameters lp)
        {
            return s_diagonal_quadrant[octant] == s_diagonal_quadrant[lp.octant];
        }

        //---------------------------------------------------------------------
        public void Divide(LineParameters lp1, LineParameters lp2)
        {
            int xmid = (x1 + x2) >> 1;
            int ymid = (y1 + y2) >> 1;
            int len2 = len >> 1;

            lp1 = this; // it is a struct so this is a copy
            lp2 = this; // it is a struct so this is a copy

            lp1.x2 = xmid;
            lp1.y2 = ymid;
            lp1.len = len2;
            lp1.dx = Math.Abs(lp1.x2 - lp1.x1);
            lp1.dy = Math.Abs(lp1.y2 - lp1.y1);

            lp2.x1 = xmid;
            lp2.y1 = ymid;
            lp2.len = len2;
            lp2.dx = Math.Abs(lp2.x2 - lp2.x1);
            lp2.dy = Math.Abs(lp2.y2 - lp2.y1);
        }
    };

    public static class LineAABasics
    {
        public const int line_subpixel_shift = 8;                          //----line_subpixel_shift
        public const int line_subpixel_scale = 1 << line_subpixel_shift;  //----line_subpixel_scale
        public const int line_subpixel_mask = line_subpixel_scale - 1;    //----line_subpixel_mask
        public const int line_max_coord = (1 << 28) - 1;              //----line_max_coord
        public const int line_max_length = 1 << (line_subpixel_shift + 10); //----line_max_length

        //-------------------------------------------------------------------------
        public const int line_mr_subpixel_shift = 4;                           //----line_mr_subpixel_shift
        public const int line_mr_subpixel_scale = 1 << line_mr_subpixel_shift; //----line_mr_subpixel_scale 
        public const int line_mr_subpixel_mask = line_mr_subpixel_scale - 1;   //----line_mr_subpixel_mask 

        //------------------------------------------------------------------line_mr
        public static int line_mr(int x)
        {
            return x >> (line_subpixel_shift - line_mr_subpixel_shift);
        }

        //-------------------------------------------------------------------line_hr
        public static int line_hr(int x)
        {
            return x << (line_subpixel_shift - line_mr_subpixel_shift);
        }

        //---------------------------------------------------------------line_dbl_hr
        public static int line_dbl_hr(int x)
        {
            return x << line_subpixel_shift;
        }

        //-------------------------------------------------------------------------
        public static void bisectrix(LineParameters l1,
                   LineParameters l2,
                   out int x, out int y)
        {
            double k = (double)(l2.len) / (double)(l1.len);
            double tx = l2.x2 - (l2.x1 - l1.x1) * k;
            double ty = l2.y2 - (l2.y1 - l1.y1) * k;

            //All bisectrices must be on the right of the Line
            //If the next point is on the left (l1 => l2.2)
            //then the bisectix should be rotated by 180 degrees.
            if ((double)(l2.x2 - l2.x1) * (double)(l2.y1 - l1.y1) <
               (double)(l2.y2 - l2.y1) * (double)(l2.x1 - l1.x1) + 100.0)
            {
                tx -= (tx - l2.x1) * 2.0;
                ty -= (ty - l2.y1) * 2.0;
            }

            // Check if the bisectrix is too short
            double dx = tx - l2.x1;
            double dy = ty - l2.y1;
            if ((int)Math.Sqrt(dx * dx + dy * dy) < line_subpixel_scale)
            {
                x = (l2.x1 + l2.x1 + (l2.y1 - l1.y1) + (l2.y2 - l2.y1)) >> 1;
                y = (l2.y1 + l2.y1 - (l2.x1 - l1.x1) - (l2.x2 - l2.x1)) >> 1;
                return;
            }
            
            x = Basics.Round(tx);
            y = Basics.Round(ty);
        }

        //-------------------------------------------fix_degenerate_bisectrix_start
        public static void fix_degenerate_bisectrix_start(LineParameters lp,
                                               ref int x, ref int y)
        {
            int d = Basics.Round(((double)(x - lp.x2) * (double)(lp.y2 - lp.y1) -
                            (double)(y - lp.y2) * (double)(lp.x2 - lp.x1)) / lp.len);
            if (d < line_subpixel_scale / 2)
            {
                x = lp.x1 + (lp.y2 - lp.y1);
                y = lp.y1 - (lp.x2 - lp.x1);
            }
        }

        //---------------------------------------------fix_degenerate_bisectrix_end
        public static void fix_degenerate_bisectrix_end(LineParameters lp,
                                             ref int x, ref int y)
        {
            int d = Basics.Round(((double)(x - lp.x2) * (double)(lp.y2 - lp.y1) -
                            (double)(y - lp.y2) * (double)(lp.x2 - lp.x1)) / lp.len);
            if (d < line_subpixel_scale / 2)
            {
                x = lp.x2 + (lp.y2 - lp.y1);
                y = lp.y2 - (lp.x2 - lp.x1);
            }
        }
    };
}
