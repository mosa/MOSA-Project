/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@mosa-project.org>)
 */

namespace Pictor.Math
{
    /// <summary>
    /// 
    /// </summary>
    public class AffineTransformation
    {
        /// <summary>
        /// 
        /// </summary>
        public double sx  = 1.0;
        /// <summary>
        /// 
        /// </summary>
        public double shy = 0.0;
        /// <summary>
        /// 
        /// </summary>
        public double shx = 0.0;
        /// <summary>
        /// 
        /// </summary>
        public double sy  = 1.0;
        /// <summary>
        /// 
        /// </summary>
        public double tx  = 0.0;
        /// <summary>
        /// 
        /// </summary>
        public double ty  = 0.0;

        /// <summary>
        /// Transforms the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void Transform(ref double x, ref double y) 
        {
            double tmp = x;
            x = tmp * sx  + y * shx + tx;
            y = tmp * shy + y * sy  + ty;
        }

        /// <summary>
        /// Transforms the specified vertices1.
        /// </summary>
        /// <param name="vertices1">The vertices1.</param>
        /// <param name="offset1">The offset1.</param>
        /// <param name="vertices2">The vertices2.</param>
        /// <param name="offset2">The offset2.</param>
        public void Transform(ref double[] vertices1, int offset1, ref double[] vertices2, int offset2)
        {
        }

        /// <summary>
        /// Inverses the transform.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void InverseTransform(ref double x, ref double y)
        {
            double d = DeterminantReciprocal();
            double a = (x - tx) * d;
            double b = (y - ty) * d;
            x = a * sy - b * shx;
            y = b * sx - a * shy;
        }

        /// <summary>
        /// Determinants the reciprocal.
        /// </summary>
        /// <returns></returns>
        public double DeterminantReciprocal()
        {
            return 1.0 / (sx * sy - shy * shx);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>The result of the operator.</returns>
        public static AffineTransformation operator*(AffineTransformation lhs, AffineTransformation rhs)
        {
            AffineTransformation result = new AffineTransformation();

            double t0 = lhs.sx  * rhs.sx + lhs.shy * rhs.shx;
            double t2 = lhs.shx * rhs.sx + lhs.sy  * rhs.shx;
            double t4 = lhs.tx  * rhs.sx + lhs.ty  * rhs.shx + rhs.tx;
            result.shy = lhs.sx  * rhs.shy + lhs.shy * rhs.sy;
            result.sy  = lhs.shx * rhs.shy + lhs.sy  * rhs.sy;
            result.ty  = lhs.tx  * rhs.shy + lhs.ty  * rhs.sy + rhs.ty;
            result.sx  = t0;
            result.shx = t2;
            result.tx  = t4;

            return result;
        }
    }
}