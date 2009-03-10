/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */
using System;
using Pictor;

namespace Pictor.Transform
{
    //============================================================trans_affine
    //
    // See Implementation agg_trans_affine.cpp
    //
    // Affine transformation are linear transformations in Cartesian Coordinates
    // (strictly speaking not only in Cartesian, but for the beginning we will 
    // think so). They are Rotation, Scaling, Translation and skewing.  
    // After any affine transformation a Line segment remains a Line segment 
    // and it will never become a curve. 
    //
    // There will be no math about matrix calculations, since it has been 
    // described many times. Ask yourself a very simple question:
    // "why do we need to understand and use some matrix stuff instead of just 
    // rotating, Scaling and so on". The answers are:
    //
    // 1. Any combination of transformations can be done by only 4 multiplications
    //    and 4 additions in floating point.
    // 2. One matrix transformation is equivalent to the number of consecutive
    //    discrete transformations, i.e. the matrix "accumulates" all transformations 
    //    in the order of their settings. Suppose we have 4 transformations: 
    //       * Rotate by 30 degrees,
    //       * Scale X to 2.0, 
    //       * Scale Y to 1.5, 
    //       * move to (100, 100). 
    //    The result will depend on the order of these transformations, 
    //    and the advantage of matrix is that the sequence of discret calls:
    //    Rotate(30), scaleX(2.0), scaleY(1.5), move(100,100) 
    //    will have exactly the same result as the following matrix transformations:
    //   
    //    affine_matrix m;
    //    m *= rotate_matrix(30); 
    //    m *= scaleX_matrix(2.0);
    //    m *= scaleY_matrix(1.5);
    //    m *= move_matrix(100,100);
    //
    //    m.transform_my_point_at_last(x, y);
    //
    // What is the good of it? In real life we will Set-up the matrix only once
    // and then Transform many points, let alone the convenience to Set any 
    // combination of transformations.
    //
    // So, how to use it? Very easy - literally as it's shown above. Not quite,
    // let us write a correct example:
    //
    // Pictor::trans_affine m;
    // m *= Pictor::trans_affine_rotation(30.0 * 3.1415926 / 180.0);
    // m *= Pictor::trans_affine_scaling(2.0, 1.5);
    // m *= Pictor::trans_affine_translation(100.0, 100.0);
    // m.Transform(&x, &y);
    //
    // The affine matrix is all you need to perform any linear transformation,
    // but all transformations have origin point (0,0). It means that we need to 
    // use 2 translations if we want to Rotate someting around (100,100):
    // 
    // m *= Pictor::trans_affine_translation(-100.0, -100.0);         // move to (0,0)
    // m *= Pictor::trans_affine_rotation(30.0 * 3.1415926 / 180.0);  // Rotate
    // m *= Pictor::trans_affine_translation(100.0, 100.0);           // move back to (100,100)
    //----------------------------------------------------------------------
    public struct Affine : ITransform
    {
        static public double affine_epsilon = 1e-14; 

        public double sx, shy, shx, sy, tx, ty;

        //------------------------------------------ Construction
        public Affine(Affine copyFrom)
        {
            sx = copyFrom.sx;
            shy = copyFrom.shy;
            shx = copyFrom.shx;
            sy = copyFrom.sy;
            tx = copyFrom.tx;
            ty = copyFrom.ty;
        }

        // Custom matrix. Usually used in derived classes
        public Affine(double v0, double v1, double v2, 
                     double v3, double v4, double v5)
        {
            sx = v0;
            shy = v1;
            shx = v2;
            sy = v3;
            tx = v4;
            ty = v5;
        }

        // Custom matrix from m[6]
        public Affine(double[] m)
        {
            sx = m[0];
            shy = m[1];
            shx = m[2];
            sy = m[3];
            tx = m[4];
            ty = m[5];
        }

        // Identity matrix
        public static Affine NewIdentity()
        {
            Affine newAffine = new Affine();
            newAffine.sx = 1.0;
            newAffine.shy = 0.0;
            newAffine.shx = 0.0;
            newAffine.sy = 1.0;
            newAffine.tx = 0.0;
            newAffine.ty = 0.0;

            return newAffine;
        }

        //====================================================trans_affine_rotation
        // Rotation matrix. sin() and cos() are calculated twice for the same angle.
        // There's no harm because the performance of sin()/cos() is very good on all
        // modern processors. Besides, this operation is not going to be invoked too 
        // often.
        public static Affine NewRotation(double AngleRadians)
        {
            return new Affine(Math.Cos(AngleRadians), Math.Sin(AngleRadians), -Math.Sin(AngleRadians), Math.Cos(AngleRadians), 0.0, 0.0);
        }

        //====================================================trans_affine_scaling
        // Scaling matrix. x, y - Scale coefficients by X and Y respectively
        public static Affine NewScaling(double Scale)
        {
            return new Affine(Scale, 0.0, 0.0, Scale, 0.0, 0.0);
        }

        public static Affine NewScaling(double x, double y)
        {
            return new Affine(x, 0.0, 0.0, y, 0.0, 0.0);
        }

        //================================================trans_affine_translation
        // Translation matrix
        public static Affine NewTranslation(double x, double y)
        {
            return new Affine(1.0, 0.0, 0.0, 1.0, x, y);
        }

        //====================================================trans_affine_skewing
        // Skewing (shear) matrix
        public static Affine NewSkewing(double x, double y)
        {
            return new Affine(1.0, Math.Tan(y), Math.Tan(x), 1.0, 0.0, 0.0);
        }

        /*
        //===============================================trans_affine_line_segment
        // Rotate, Scale and Translate, associating 0...dist with Line segment 
        // x1,y1,x2,y2
        public static Affine NewScaling(double x, double y)
        {
            return new Affine(x, 0.0, 0.0, y, 0.0, 0.0);
        }
        public sealed class trans_affine_line_segment : Affine
        {
            public trans_affine_line_segment(double x1, double y1, double x2, double y2,
                                      double dist)
            {
                double dx = x2 - x1;
                double dy = y2 - y1;
                if (dist > 0.0)
                {
                    //Multiply(trans_affine_scaling(sqrt(dx * dx + dy * dy) / dist));
                }
                //Multiply(trans_affine_rotation(Math.Atan2(dy, dx)));
                //Multiply(trans_affine_translation(x1, y1));
            }
        };


        //============================================trans_affine_reflection_unit
        // Reflection matrix. Reflect Coordinates across the Line through 
        // the origin containing the unit vector (ux, uy).
        // Contributed by John Horigan
        public static Affine NewScaling(double x, double y)
        {
            return new Affine(x, 0.0, 0.0, y, 0.0, 0.0);
        }
        public class trans_affine_reflection_unit : Affine
        {
            public trans_affine_reflection_unit(double ux, double uy)
                :
              base(2.0 * ux * ux - 1.0,
                           2.0 * ux * uy,
                           2.0 * ux * uy,
                           2.0 * uy * uy - 1.0,
                           0.0, 0.0)
            { }
        };


        //=================================================trans_affine_reflection
        // Reflection matrix. Reflect Coordinates across the Line through 
        // the origin At the angle a or containing the non-unit vector (x, y).
        // Contributed by John Horigan
        public static Affine NewScaling(double x, double y)
        {
            return new Affine(x, 0.0, 0.0, y, 0.0, 0.0);
        }
        public sealed class trans_affine_reflection : trans_affine_reflection_unit
        {
            public trans_affine_reflection(double a)
                :
              base(Math.Cos(a), Math.Sin(a))
            { }


            public trans_affine_reflection(double x, double y)
                :
              base(x / Math.Sqrt(x * x + y * y), y / Math.Sqrt(x * x + y * y))
            { }
        };
         */

        /*
        // Rectangle to a parallelogram.
        trans_affine(double x1, double y1, double x2, double y2, double* parl)
        {
            rect_to_parl(x1, y1, x2, y2, parl);
        }

        // Parallelogram to a rectangle.
        trans_affine(double* parl, 
                     double x1, double y1, double x2, double y2)
        {
            parl_to_rect(parl, x1, y1, x2, y2);
        }

        // Arbitrary parallelogram transformation.
        trans_affine(double* src, double* dst)
        {
            parl_to_parl(src, dst);
        }

        //---------------------------------- Parellelogram transformations
        // Transform a parallelogram to another one. Src and dst are 
        // pointers to arrays of three points (double[6], x1,y1,...) that 
        // identify three corners of the parallelograms assuming implicit 
        // fourth point. The arguments are arrays of double[6] mapped 
        // to x1,y1, x2,y2, x3,y3  where the Coordinates are:
        //        *-----------------*
        //       /          (x3,y3)/
        //      /                 /
        //     /(x1,y1)   (x2,y2)/
        //    *-----------------*
		trans_affine parl_to_parl(double* src, double* dst)
		{
			sx  = src[2] - src[0];
			shy = src[3] - src[1];
			shx = src[4] - src[0];
			sy  = src[5] - src[1];
			tx  = src[0];
			ty  = src[1];
			Invert();
			Multiply(trans_affine(dst[2] - dst[0], dst[3] - dst[1], 
				dst[4] - dst[0], dst[5] - dst[1],
				dst[0], dst[1]));
			return *this;
		}

		trans_affine rect_to_parl(double x1, double y1, 
			double x2, double y2, 
			double* parl)
		{
			double src[6];
			src[0] = x1; src[1] = y1;
			src[2] = x2; src[3] = y1;
			src[4] = x2; src[5] = y2;
			parl_to_parl(src, parl);
			return *this;
		}

		trans_affine parl_to_rect(double* parl, 
			double x1, double y1, 
			double x2, double y2)
		{
			double dst[6];
			dst[0] = x1; dst[1] = y1;
			dst[2] = x2; dst[3] = y1;
			dst[4] = x2; dst[5] = y2;
			parl_to_parl(parl, dst);
			return *this;
		}

         */

        //------------------------------------------ Operations
        // Reset - load an Identity matrix
		public void Identity()
		{
			sx  = sy  = 1.0; 
			shy = shx = tx = ty = 0.0;
		}

        // Direct transformations operations
        public void Translate(double x, double y) 
        { 
            tx += x;
            ty += y; 
        }

        public void Rotate(double AngleRadians) 
        {
            double ca = Math.Cos(AngleRadians); 
            double sa = Math.Sin(AngleRadians);
            double t0 = sx  * ca - shy * sa;
            double t2 = shx * ca - sy * sa;
            double t4 = tx  * ca - ty * sa;
            shy = sx  * sa + shy * ca;
            sy  = shx * sa + sy * ca; 
            ty  = tx  * sa + ty * ca;
            sx  = t0;
            shx = t2;
            tx  = t4;
        }

        public void Scale(double x, double y) 
        {
            double mm0 = x; // Possible hint for the optimizer
            double mm3 = y; 
            sx  *= mm0;
            shx *= mm0;
            tx  *= mm0;
            shy *= mm3;
            sy  *= mm3;
            ty  *= mm3;
        }

        public void Scale(double scaleAmount) 
        {
            sx  *= scaleAmount;
            shx *= scaleAmount;
            tx  *= scaleAmount;
            shy *= scaleAmount;
            sy  *= scaleAmount;
            ty  *= scaleAmount;
        }

        // Multiply matrix to another one
		void Multiply(Affine m)
		{
			double t0 = sx  * m.sx + shy * m.shx;
			double t2 = shx * m.sx + sy  * m.shx;
			double t4 = tx  * m.sx + ty  * m.shx + m.tx;
			shy = sx  * m.shy + shy * m.sy;
			sy  = shx * m.shy + sy  * m.sy;
			ty  = tx  * m.shy + ty  * m.sy + m.ty;
			sx  = t0;
			shx = t2;
			tx  = t4;
		}
        /*

        // Multiply "m" to "this" and assign the result to "this"
        trans_affine PreMultiply(trans_affine m)
        {
            trans_affine t = m;
            return *this = t.Multiply(*this);
        }

        // Multiply matrix to inverse of another one
        trans_affine MultiplyInverse(trans_affine m)
        {
            trans_affine t = m;
            t.Invert();
            return Multiply(t);
        }

        // Multiply inverse of "m" to "this" and assign the result to "this"
        trans_affine PreMultiplyInverse(trans_affine m)
        {
            trans_affine t = m;
            t.Invert();
            return *this = t.Multiply(*this);
        }
         */

        // Invert matrix. Do not try to Invert degenerate matrices, 
        // there's no check for validity. If you Set Scale to 0 and 
        // then try to Invert matrix, expect unpredictable result.
		public void Invert()
		{
			double d  = DeterminantReciprocal();

			double t0  =  sy  * d;
			sy  =  sx  * d;
			shy = -shy * d;
			shx = -shx * d;

			double t4 = -tx * t0  - ty * shx;
			ty = -tx * shy - ty * sy;

			sx = t0;
			tx = t4;
		}

        /*

        // Mirroring around X
		trans_affine flip_x()
		{
			sx  = -sx;
			shy = -shy;
			tx  = -tx;
			return *this;
		}

        // Mirroring around Y
		trans_affine flip_y()
		{
			shx = -shx;
			sy  = -sy;
			ty  = -ty;
			return *this;
		}

        //------------------------------------------- Load/Store
        // Store matrix to an array [6] of double
        void StoreTo(double* m)
        {
            *m++ = sx; *m++ = shy; *m++ = shx; *m++ = sy; *m++ = tx; *m++ = ty;
        }

        // Load matrix from an array [6] of double
        trans_affine LoadFrom(double* m)
        {
            sx = *m++; shy = *m++; shx = *m++; sy = *m++; tx = *m++;  ty = *m++;
            return *this;
        }

        //------------------------------------------- Operators
        
         */
        // Multiply the matrix by another one and return
        // the result in a separete matrix.
        public static Affine operator*(Affine a, Affine b)
        {
            Affine temp = new Affine(a);
            temp.Multiply(b);
            return temp;
        }
        /*

        // Multiply the matrix by inverse of another one 
        // and return the result in a separete matrix.
        static trans_affine operator / (trans_affine a, trans_affine b)
        {
            return new trans_affine(a).MultiplyInverse(b);
        }

        // Calculate and return the inverse matrix
        static trans_affine operator ~ (trans_affine a)
        {
            new trans_affine(a).Invert();
        }

        // Equal operator with default epsilon
        static bool operator == (trans_affine a, trans_affine b)
        {
            return a.IsEqual(b, affine_epsilon);
        }

        // Not Equal operator with default epsilon
        static bool operator != (trans_affine a, trans_affine b)
        {
            return !a.IsEqual(b, affine_epsilon);
        }

         */
        //-------------------------------------------- Transformations
        // Direct transformation of x and y
        public void Transform(ref double x, ref double y)
        {
            double tmp = x;
            x = tmp * sx  + y * shx + tx;
            y = tmp * shy + y * sy  + ty;
        }

        public void Transform(ref PointD pointToTransform)
        {
            Transform(ref pointToTransform.x, ref pointToTransform.y);
        }
        /*

        // Direct transformation of x and y, 2x2 matrix only, no Translation
        void Transform2x2(double* x, double* y)
        {
            register double tmp = *x;
            *x = tmp * sx  + *y * shx;
            *y = tmp * shy + *y * sy;
        }

         */
        // Inverse transformation of x and y. It works slower than the 
        // direct transformation. For massive operations it's better to 
        // Invert() the matrix and then use direct transformations. 
        public void InverseTransform(ref double x, ref double y)
        {
            double d = DeterminantReciprocal();
            double a = (x - tx) * d;
            double b = (y - ty) * d;
            x = a * sy - b * shx;
            y = b * sx - a * shy;
        }
        /*

        //-------------------------------------------- Auxiliary
        // Calculate the Determinant of matrix
        double Determinant()
        {
            return sx * sy - shy * shx;
        }

         */
        // Calculate the reciprocal of the Determinant
        double DeterminantReciprocal()
        {
            return 1.0 / (sx * sy - shy * shx);
        }

        // Get the average Scale (by X and Y). 
        // Basically used to Calculate the ApproximationScale when
        // decomposinting curves into Line segments.
        public double GetScale()
        {
            double x = 0.707106781 * sx  + 0.707106781 * shx;
            double y = 0.707106781 * shy + 0.707106781 * sy;
            return Math.Sqrt(x*x + y*y);
        }

        // Check to see if the matrix is not degenerate
        public bool IsValid(double epsilon)
		{
            return Math.Abs(sx) > epsilon && Math.Abs(sy) > epsilon;
		}

        // Check to see if it's an Identity matrix
        public bool IsIdentity()
        {
            return IsIdentity(affine_epsilon);
        }

		public bool IsIdentity(double epsilon)
		{
			return Basics.IsEqualEps(sx,  1.0, epsilon) &&
                Basics.IsEqualEps(shy, 0.0, epsilon) &&
                Basics.IsEqualEps(shx, 0.0, epsilon) &&
                Basics.IsEqualEps(sy, 1.0, epsilon) &&
                Basics.IsEqualEps(tx, 0.0, epsilon) &&
                Basics.IsEqualEps(ty, 0.0, epsilon);
		}

        // Check to see if two matrices are equal
        public bool IsEqual(Affine m, double epsilon)
		{
            return Basics.IsEqualEps(sx, m.sx, epsilon) &&
                Basics.IsEqualEps(shy, m.shy, epsilon) &&
                Basics.IsEqualEps(shx, m.shx, epsilon) &&
                Basics.IsEqualEps(sy, m.sy, epsilon) &&
                Basics.IsEqualEps(tx, m.tx, epsilon) &&
                Basics.IsEqualEps(ty, m.ty, epsilon);
		}

        // Determine the major parameters. Use with caution considering 
        // possible degenerate cases.
        public double Rotation()
		{
			double x1 = 0.0;
			double y1 = 0.0;
			double x2 = 1.0;
			double y2 = 0.0;
            Transform(ref x1, ref y1);
            Transform(ref x2, ref y2);
			return Math.Atan2(y2-y1, x2-x1);
		}
        
        public void Translation(out double dx, out double dy)
		{
			dx = tx;
			dy = ty;
		}
		
        public void Scaling(out double x, out double y)
		{
			double x1 = 0.0;
			double y1 = 0.0;
			double x2 = 1.0;
			double y2 = 1.0;
            Affine t = new Affine(this);
			t *= NewRotation(-Rotation());
			t.Transform(ref x1, ref y1);
			t.Transform(ref x2, ref y2);
			x = x2 - x1;
			y = y2 - y1;
		}

        public void ScalingAbs(out double x, out double y)
        {
            // Used to Calculate Scaling coefficients in image resampling. 
            // When there is considerable shear this method gives us much
            // better estimation than just sx, sy.
            x = Math.Sqrt(sx  * sx  + shx * shx);
            y = Math.Sqrt(shy * shy + sy  * sy);
        }
    };
}