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
    //=======================================================trans_perspective
    public sealed class Perspective : ITransform
    {
        static public double affine_epsilon = 1e-14;
        public double sx, shy, w0, shx, sy, w1, tx, ty, w2;

        //-------------------------------------------------------ruction
        // Identity matrix
        public Perspective()
        {
            sx =(1); shy=(0); w0=(0);
            shx=(0); sy =(1); w1=(0); 
            tx =(0); ty =(0); w2=(1);
        }

        // Custom matrix
        public Perspective(double v0, double v1, double v2, 
                          double v3, double v4, double v5,
                          double v6, double v7, double v8)
        {
            sx =(v0); shy=(v1); w0=(v2);
            shx=(v3); sy =(v4); w1=(v5);
            tx =(v6); ty =(v7); w2=(v8);
        }

        // Custom matrix from m[9]
        public Perspective(double[] m)
        {
           sx =(m[0]); shy=(m[1]); w0=(m[2]); 
           shx=(m[3]); sy =(m[4]); w1=(m[5]); 
           tx =(m[6]); ty =(m[7]); w2=(m[8]);
        }

        // from affine
        public Perspective(Affine a)
        {
            sx = (a.sx); shy = (a.shy); w0 = (0);
            shx = (a.shx); sy = (a.sy); w1 = (0);
            tx = (a.tx); ty = (a.ty); w2 = (1);
        }

        // from trans_perspective
        public Perspective(Perspective a)
        {
            sx = (a.sx); shy = (a.shy); w0 = a.w0;
            shx = (a.shx); sy = (a.sy); w1 = a.w1;
            tx = (a.tx); ty = (a.ty); w2 = a.w2;
        }

        // Rectangle to quadrilateral
        public Perspective(double x1, double y1, double x2, double y2, double[] quad)
        {
            RectangleToQuad(x1, y1, x2, y2, quad);
        }

        // Quadrilateral to rectangle
        public Perspective(double[] quad, double x1, double y1, double x2, double y2)
        {
            QuadToRectangle(quad, x1, y1, x2, y2);
        }

        // Arbitrary quadrilateral transformations
        public Perspective(double[] src, double[] dst)
        {
            QuadToQuad(src, dst);
        }

        public void Set(Perspective Other)
        {
            sx = Other.sx;
            shy = Other.shy;
            w0 = Other.w0;
            shx = Other.shx;
            sy = Other.sy;
            w1 = Other.w1;
            tx = Other.tx;
            ty = Other.ty;
            w2 = Other.w2;
        }

        //-------------------------------------- Quadrilateral transformations
        // The arguments are double[8] that are mapped to quadrilaterals:
        // x1,y1, x2,y2, x3,y3, x4,y4
        public bool QuadToQuad(double[] qs, double[] qd)
        {
            Perspective p = new Perspective();
            if (!QuadToSquare(qs)) return false;
            if (!p.SquareToQuad(qd)) return false;
            Multiply(p);
            return true;
        }

        public bool RectangleToQuad(double x1, double y1, double x2, double y2, double[] q)
        {
            double[] r = new double[8];
            r[0] = r[6] = x1;
            r[2] = r[4] = x2;
            r[1] = r[3] = y1;
            r[5] = r[7] = y2;
            return QuadToQuad(r, q);
        }

        public bool QuadToRectangle(double[] q, double x1, double y1, double x2, double y2)
        {
            double[] r = new double[8];
            r[0] = r[6] = x1;
            r[2] = r[4] = x2;
            r[1] = r[3] = y1;
            r[5] = r[7] = y2;
            return QuadToQuad(q, r);
        }

        // Map square (0,0,1,1) to the quadrilateral and vice versa
        public bool SquareToQuad(double[] q)
        {
            double dx = q[0] - q[2] + q[4] - q[6];
            double dy = q[1] - q[3] + q[5] - q[7];
            if (dx == 0.0 && dy == 0.0)
            {
                // Affine case (parallelogram)
                //---------------
                sx = q[2] - q[0];
                shy = q[3] - q[1];
                w0 = 0.0;
                shx = q[4] - q[2];
                sy = q[5] - q[3];
                w1 = 0.0;
                tx = q[0];
                ty = q[1];
                w2 = 1.0;
            }
            else
            {
                double dx1 = q[2] - q[4];
                double dy1 = q[3] - q[5];
                double dx2 = q[6] - q[4];
                double dy2 = q[7] - q[5];
                double den = dx1 * dy2 - dx2 * dy1;
                if (den == 0.0)
                {
                    // Singular case
                    //---------------
                    sx = shy = w0 = shx = sy = w1 = tx = ty = w2 = 0.0;
                    return false;
                }
                // General case
                //---------------
                double u = (dx * dy2 - dy * dx2) / den;
                double v = (dy * dx1 - dx * dy1) / den;
                sx = q[2] - q[0] + u * q[2];
                shy = q[3] - q[1] + u * q[3];
                w0 = u;
                shx = q[6] - q[0] + v * q[6];
                sy = q[7] - q[1] + v * q[7];
                w1 = v;
                tx = q[0];
                ty = q[1];
                w2 = 1.0;
            }
            return true;
        }

        public bool QuadToSquare(double[] q)
        {
            if (!SquareToQuad(q)) return false;
            Invert();
            return true;
        }


        //--------------------------------------------------------- Operations
        public Perspective FromAffine(Affine a)
        {
            sx  = a.sx;  shy = a.shy; w0 = 0; 
            shx = a.shx; sy  = a.sy;  w1 = 0;
            tx  = a.tx;  ty  = a.ty;  w2 = 1;
            return this;
        }

        // Reset - load an Identity matrix
        public Perspective Reset()
        {
            sx = 1; shy = 0; w0 = 0;
            shx = 0; sy = 1; w1 = 0;
            tx = 0; ty = 0; w2 = 1;
            return this;
        }

        // Invert matrix. Returns false in degenerate case
        public bool Invert()
        {
            double d0 = sy * w2 - w1 * ty;
            double d1 = w0 * ty - shy * w2;
            double d2 = shy * w1 - w0 * sy;
            double d = sx * d0 + shx * d1 + tx * d2;
            if (d == 0.0)
            {
                sx = shy = w0 = shx = sy = w1 = tx = ty = w2 = 0.0;
                return false;
            }
            d = 1.0 / d;
            Perspective a = new Perspective(this);
            sx = d * d0;
            shy = d * d1;
            w0 = d * d2;
            shx = d * (a.w1 * a.tx - a.shx * a.w2);
            sy = d * (a.sx * a.w2 - a.w0 * a.tx);
            w1 = d * (a.w0 * a.shx - a.sx * a.w1);
            tx = d * (a.shx * a.ty - a.sy * a.tx);
            ty = d * (a.shy * a.tx - a.sx * a.ty);
            w2 = d * (a.sx * a.sy - a.shy * a.shx);
            return true;
        }

        // Direct transformations operations
        public Perspective Translate(double x, double y)
        {
            tx += x;
            ty += y;
            return this;
        }

        public Perspective Rotate(double a)
        {
            Multiply(Affine.NewRotation(a));
            return this;
        }

        public Perspective Scale(double s)
        {
            Multiply(Affine.NewScaling(s));
            return this;
        }

        public Perspective Scale(double x, double y)
        {
            Multiply(Affine.NewScaling(x, y));
            return this;
        }

        public Perspective Multiply(Perspective a)
        {
            Perspective b = new Perspective(this);
            sx  = a.sx *b.sx  + a.shx*b.shy + a.tx*b.w0;
            shx = a.sx *b.shx + a.shx*b.sy  + a.tx*b.w1;
            tx  = a.sx *b.tx  + a.shx*b.ty  + a.tx*b.w2;
            shy = a.shy*b.sx  + a.sy *b.shy + a.ty*b.w0;
            sy  = a.shy*b.shx + a.sy *b.sy  + a.ty*b.w1;
            ty  = a.shy*b.tx  + a.sy *b.ty  + a.ty*b.w2;
            w0  = a.w0 *b.sx  + a.w1 *b.shy + a.w2*b.w0;
            w1  = a.w0 *b.shx + a.w1 *b.sy  + a.w2*b.w1;
            w2  = a.w0 *b.tx  + a.w1 *b.ty  + a.w2*b.w2;
            return this;
        }

        //------------------------------------------------------------------------
        public Perspective Multiply(Affine a)
        {
            Perspective b = new Perspective(this);
            sx  = a.sx *b.sx  + a.shx*b.shy + a.tx*b.w0;
            shx = a.sx *b.shx + a.shx*b.sy  + a.tx*b.w1;
            tx  = a.sx *b.tx  + a.shx*b.ty  + a.tx*b.w2;
            shy = a.shy*b.sx  + a.sy *b.shy + a.ty*b.w0;
            sy  = a.shy*b.shx + a.sy *b.sy  + a.ty*b.w1;
            ty  = a.shy*b.tx  + a.sy *b.ty  + a.ty*b.w2;
            return this;
        }

        //------------------------------------------------------------------------
        public Perspective PreMultiply(Perspective b)
        {
            Perspective a = new Perspective(this);
            sx  = a.sx *b.sx  + a.shx*b.shy + a.tx*b.w0;
            shx = a.sx *b.shx + a.shx*b.sy  + a.tx*b.w1;
            tx  = a.sx *b.tx  + a.shx*b.ty  + a.tx*b.w2;
            shy = a.shy*b.sx  + a.sy *b.shy + a.ty*b.w0;
            sy  = a.shy*b.shx + a.sy *b.sy  + a.ty*b.w1;
            ty  = a.shy*b.tx  + a.sy *b.ty  + a.ty*b.w2;
            w0  = a.w0 *b.sx  + a.w1 *b.shy + a.w2*b.w0;
            w1  = a.w0 *b.shx + a.w1 *b.sy  + a.w2*b.w1;
            w2  = a.w0 *b.tx  + a.w1 *b.ty  + a.w2*b.w2;
            return this;
        }

        //------------------------------------------------------------------------
        public Perspective PreMultiply(Affine b)
        {
            Perspective a = new Perspective(this);
            sx  = a.sx *b.sx  + a.shx*b.shy;
            shx = a.sx *b.shx + a.shx*b.sy;
            tx  = a.sx *b.tx  + a.shx*b.ty  + a.tx;
            shy = a.shy*b.sx  + a.sy *b.shy;
            sy  = a.shy*b.shx + a.sy *b.sy;
            ty  = a.shy*b.tx  + a.sy *b.ty  + a.ty;
            w0  = a.w0 *b.sx  + a.w1 *b.shy;
            w1  = a.w0 *b.shx + a.w1 *b.sy;
            w2  = a.w0 *b.tx  + a.w1 *b.ty  + a.w2;
            return this;
        }

        //------------------------------------------------------------------------
        public Perspective MultiplyInverse(Perspective m)
        {
            Perspective t = m;
            t.Invert();
            return Multiply(t);
        }

        //------------------------------------------------------------------------
        public Perspective TransPerspectiveMultiplyInverse(Affine m)
        {
            Affine t = m;
            t.Invert();
            return Multiply(t);
        }

        //------------------------------------------------------------------------
        public Perspective PreMultiplyInverse(Perspective m)
        {
            Perspective t = m;
            t.Invert();
            Set(t.Multiply(this));
            return this;
        }

        // Multiply inverse of "m" by "this" and assign the result to "this"
        public Perspective PreMultiplyInverse(Affine m)
        {
            Perspective t=new Perspective(m);
            t.Invert();
            Set(t.Multiply(this));
            return this;
        }

        //--------------------------------------------------------- Load/Store
        public void StoreTo(double[] m)
        {
            m[0] = sx;  m[1] = shy; m[2] = w0;
            m[3] = shx; m[4] = sy;  m[5] = w1;
            m[6] = tx;  m[7] = ty;  m[8] = w2;
        }

        //------------------------------------------------------------------------
        public Perspective LoadFrom(double[] m)
        {
            sx = m[0]; shy = m[1]; w0 = m[2];
            shx = m[3]; sy = m[4]; w1 = m[5];
            tx = m[6]; ty = m[7]; w2 = m[8];
            return this;
        }

        //---------------------------------------------------------- Operators
        // Multiply the matrix by another one and return the result in a separate matrix.
        public static Perspective operator *(Perspective a, Perspective b)
        {
            Perspective temp = a;
            temp.Multiply(b);

            return temp;
        }

        // Multiply the matrix by another one and return the result in a separate matrix.
        public static Perspective operator *(Perspective a, Affine b)
        {
            Perspective temp = a;
            temp.Multiply(b);

            return temp;
        }

        // Multiply the matrix by inverse of another one and return the result in a separate matrix.
        public static Perspective operator/(Perspective a, Perspective b)
        {
            Perspective temp = a;
            temp.MultiplyInverse(b);

            return temp;
        }

        // Calculate and return the inverse matrix
        public static Perspective operator~(Perspective b)
        {
            Perspective ret = b;
            ret.Invert();
            return ret;
        }

        // Equal operator with default epsilon
        public static bool operator==(Perspective a, Perspective b)
        {
            return a.IsEqual(b, affine_epsilon);
        }

        // Not Equal operator with default epsilon
        public static bool operator !=(Perspective a, Perspective b)
        {
            return !a.IsEqual(b, affine_epsilon);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //---------------------------------------------------- Transformations
        // Direct transformation of x and y
        public void Transform(ref double px, ref double py)
        {
            double x = px;
            double y = py;
            double m = 1.0 / (x * w0 + y * w1 + w2);
            px = m * (x * sx + y * shx + tx);
            py = m * (x * shy + y * sy + ty);
        }

        // Direct transformation of x and y, affine part only
        public void TransformAffine(ref double x, ref double y)
        {
            double tmp = x;
            x = tmp * sx + y * shx + tx;
            y = tmp * shy + y * sy + ty;
        }

        // Direct transformation of x and y, 2x2 matrix only, no Translation
        public void Transform2x2(ref double x, ref double y)
        {
            double tmp = x;
            x = tmp * sx + y * shx;
            y = tmp * shy + y * sy;
        }

        // Inverse transformation of x and y. It works slow because
        // it explicitly inverts the matrix on every call. For massive 
        // operations it's better to Invert() the matrix and then use 
        // direct transformations. 
        public void InverseTransform(ref double x, ref double y)
        {
            Perspective t = new Perspective(this);
            if(t.Invert()) t.Transform(ref x, ref y);
        }


        //---------------------------------------------------------- Auxiliary
        public double Determinant
        {
            get
            {
                return sx * (sy * w2 - ty * w1) +
                       shx * (ty * w0 - shy * w2) +
                       tx * (shy * w1 - sy * w0);
            }
        }
        public double ReciprocalDeterminant
        {
            get { return 1.0 / Determinant; }
        }

        public bool IsValid() { return IsValid(affine_epsilon); }
        public bool IsValid(double epsilon)
        {
            return Math.Abs(sx) > epsilon && Math.Abs(sy) > epsilon && Math.Abs(w2) > epsilon;
        }

        public bool IsIdentity() { return IsIdentity(affine_epsilon); }
        public bool IsIdentity(double epsilon)
        {
            return Basics.IsEqualEps(sx, 1.0, epsilon) &&
                   Basics.IsEqualEps(shy, 0.0, epsilon) &&
                   Basics.IsEqualEps(w0, 0.0, epsilon) &&
                   Basics.IsEqualEps(shx, 0.0, epsilon) &&
                   Basics.IsEqualEps(sy, 1.0, epsilon) &&
                   Basics.IsEqualEps(w1, 0.0, epsilon) &&
                   Basics.IsEqualEps(tx, 0.0, epsilon) &&
                   Basics.IsEqualEps(ty, 0.0, epsilon) &&
                   Basics.IsEqualEps(w2, 1.0, epsilon);
        }

        public bool IsEqual(Perspective m) 
        {
            return IsEqual(m, affine_epsilon); 
        }

        public bool IsEqual(Perspective m, double epsilon)
        {
            return Basics.IsEqualEps(sx, m.sx, epsilon) &&
                   Basics.IsEqualEps(shy, m.shy, epsilon) &&
                   Basics.IsEqualEps(w0, m.w0, epsilon) &&
                   Basics.IsEqualEps(shx, m.shx, epsilon) &&
                   Basics.IsEqualEps(sy, m.sy, epsilon) &&
                   Basics.IsEqualEps(w1, m.w1, epsilon) &&
                   Basics.IsEqualEps(tx, m.tx, epsilon) &&
                   Basics.IsEqualEps(ty, m.ty, epsilon) &&
                   Basics.IsEqualEps(w2, m.w2, epsilon);
        }

        // Determine the major affine parameters. Use with caution 
        // considering possible degenerate cases.
        public double Scale()
        {
            double x = 0.707106781 * sx + 0.707106781 * shx;
            double y = 0.707106781 * shy + 0.707106781 * sy;
            return Math.Sqrt(x * x + y * y);
        }
        public double Rotation()
        {
            double x1 = 0.0;
            double y1 = 0.0;
            double x2 = 1.0;
            double y2 = 0.0;
            Transform(ref x1, ref y1);
            Transform(ref x2, ref y2);
            return Math.Atan2(y2 - y1, x2 - x1);
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
            Perspective t=new Perspective(this);
            t *= Affine.NewRotation(-Rotation());
            t.Transform(ref x1, ref y1);
            t.Transform(ref x2, ref y2);
            x = x2 - x1;
            y = y2 - y1;
        }
        public void AbsoluteScaling(out double x, out double y)
        {
            x = Math.Sqrt(sx * sx + shx * shx);
            y = Math.Sqrt(shy * shy + sy * sy);
        }    

        //--------------------------------------------------------------------
        public sealed class IteratorX
        {
            double den;
            double den_step;
            double nom_x;
            double nom_x_step;
            double nom_y;
            double nom_y_step;

            public double x;
            public double y;

            public IteratorX() {}
            public IteratorX(double px, double py, double step, Perspective m)
            {
                den=(px * m.w0 + py * m.w1 + m.w2);
                den_step=(m.w0 * step);
                nom_x=(px * m.sx + py * m.shx + m.tx);
                nom_x_step=(step * m.sx);
                nom_y=(px * m.shy + py * m.sy + m.ty);
                nom_y_step=(step * m.shy);
                x=(nom_x / den);
                y=(nom_y / den);
            }

            public static IteratorX operator++(IteratorX a)
            {
                a.den   += a.den_step;
                a.nom_x += a.nom_x_step;
                a.nom_y += a.nom_y_step;
                double d = 1.0 / a.den;
                a.x = a.nom_x * d;
                a.y = a.nom_y * d;

                return a;
            }
        };

        //--------------------------------------------------------------------
        public IteratorX Begin(double x, double y, double step)
        {
            return new IteratorX(x, y, step, this);
        }
    };
}
