/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */
using System;

namespace Pictor.Transform
{
    public sealed class Bilinear : ITransform
    {
        readonly double[,] _matrix = new double[4,2];
        bool   _valid;

        public Bilinear()
        {
            _valid=(false);
        }

        ///<summary>
        ///</summary>
        ///<param name="src"></param>
        ///<param name="dst"></param>
        public Bilinear(double[] src, double[] dst) 
        {
            quad_to_quad(src, dst);
        }

        ///<summary>
        ///</summary>
        ///<param name="x1"></param>
        ///<param name="y1"></param>
        ///<param name="x2"></param>
        ///<param name="y2"></param>
        ///<param name="quad"></param>
        public Bilinear(double x1, double y1, double x2, double y2, double[] quad)
        {
            rect_to_quad(x1, y1, x2, y2, quad);
        }


        //--------------------------------------------------------------------
        // Reverse transformations 
        public Bilinear(double[] quad, 
                       double x1, double y1, double x2, double y2)
        {
            quad_to_rect(quad, x1, y1, x2, y2);
        }


        //--------------------------------------------------------------------
        // Set the transformations using two arbitrary quadrangles.
        public void quad_to_quad(double[] src, double[] dst)
        {
            double[,] left = new double[4,4];
            double[,] right = new double[4,2];

            uint i;
            for(i = 0; i < 4; i++)
            {
                uint ix = i * 2;
                uint iy = ix + 1;
                left[i,0] = 1.0;
                left[i,1] = src[ix] * src[iy];
                left[i,2] = src[ix];
                left[i,3] = src[iy];

                right[i,0] = dst[ix];
                right[i,1] = dst[iy];
            }
            _valid = EquationSimulator.Solve(left, right, _matrix);
        }


        //--------------------------------------------------------------------
        // Set the direct transformations, i.e., rectangle -> quadrangle
        public void rect_to_quad(double x1, double y1, double x2, double y2, 
                          double[] quad)
        {
            double[] src = new double[8];
            src[0] = src[6] = x1;
            src[2] = src[4] = x2;
            src[1] = src[3] = y1;
            src[5] = src[7] = y2;
            quad_to_quad(src, quad);
        }


        //--------------------------------------------------------------------
        // Set the reverse transformations, i.e., quadrangle -> rectangle
        public void quad_to_rect(double[] quad, 
                          double x1, double y1, double x2, double y2)
        {
            double[] dst = new double[8];
            dst[0] = dst[6] = x1;
            dst[2] = dst[4] = x2;
            dst[1] = dst[3] = y1;
            dst[5] = dst[7] = y2;
            quad_to_quad(quad, dst);
        }

        //--------------------------------------------------------------------
        // Check if the equations were solved successfully
        public bool is_valid() { return _valid; }

        //--------------------------------------------------------------------
        // Transform a point (x, y)
        public void Transform(ref double x, ref double y)
        {
            double tx = x;
            double ty = y;
            double xy = tx * ty;
            x = _matrix[0,0] + _matrix[1,0] * xy + _matrix[2,0] * tx + _matrix[3,0] * ty;
            y = _matrix[0,1] + _matrix[1,1] * xy + _matrix[2,1] * tx + _matrix[3,1] * ty;
        }


        //--------------------------------------------------------------------
        public sealed class iterator_x
        {
            double inc_x;
            double inc_y;

            public double x;
            public double y;

            public iterator_x() {}
            public iterator_x(double tx, double ty, double step, double[,] m)
            {
                inc_x = (m[1,0] * step * ty + m[2,0] * step);
                inc_y = (m[1,1] * step * ty + m[2,1] * step);
                x = (m[0,0] + m[1,0] * tx * ty + m[2,0] * tx + m[3,0] * ty);
                y = (m[0,1] + m[1,1] * tx * ty + m[2,1] * tx + m[3,1] * ty);
            }

            public static iterator_x operator++(iterator_x a)
            {
                a.x += a.inc_x;
                a.y += a.inc_y;

                return a;
            }
        };

        public iterator_x begin(double x, double y, double step)
        {
            return new iterator_x(x, y, step, _matrix);
        }
    };
}