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
	///<summary>
	///
	/// See Implementation agg_trans_affine.cpp
	///
	/// Affine transformation are linear transformations in Cartesian Coordinates
	/// (strictly speaking not only in Cartesian, but for the beginning we will 
	/// think so). They are Rotation, Scaling, Translation and skewing.  
	/// After any affine transformation a Line segment remains a Line segment 
	/// and it will never become a curve. 
	///
	/// There will be no math about matrix calculations, since it has been 
	/// described many times. Ask yourself a very simple question:
	/// "why do we need to understand and use some matrix stuff instead of just 
	/// rotating, Scaling and so on". The answers are:
	///
	/// 1. Any combination of transformations can be done by only 4 multiplications
	///    and 4 additions in floating point.
	/// 2. One matrix transformation is equivalent to the number of consecutive
	///    discrete transformations, i.e. the matrix "accumulates" all transformations 
	///    in the order of their settings. Suppose we have 4 transformations: 
	///       * Rotate by 30 degrees,
	///       * Scale X to 2.0, 
	///       * Scale Y to 1.5, 
	///       * move to (100, 100). 
	///    The result will depend on the order of these transformations, 
	///    and the advantage of matrix is that the sequence of discret calls:
	///    Rotate(30), scaleX(2.0), scaleY(1.5), move(100,100) 
	///    will have exactly the same result as the following matrix transformations:
	///   
	///    affine_matrix m;
	///    m *= rotate_matrix(30); 
	///    m *= scaleX_matrix(2.0);
	///    m *= scaleY_matrix(1.5);
	///    m *= move_matrix(100,100);
	///
	///    m.transform_my_point_at_last(x, y);
	///
	/// What is the good of it? In real life we will Set-up the matrix only once
	/// and then Transform many points, let alone the convenience to Set any 
	/// combination of transformations.
	///
	/// So, how to use it? Very easy - literally as it's shown above. Not quite,
	/// let us write a correct example:
	///
	/// Pictor::trans_affine m;
	/// m *= Pictor::trans_affine_rotation(30.0 * 3.1415926 / 180.0);
	/// m *= Pictor::trans_affine_scaling(2.0, 1.5);
	/// m *= Pictor::trans_affine_translation(100.0, 100.0);
	/// m.Transform(x, y);
	///
	/// The affine matrix is all you need to perform any linear transformation,
	/// but all transformations have origin point (0,0). It means that we need to 
	/// use 2 translations if we want to Rotate someting around (100,100):
	/// 
	/// m *= Pictor::trans_affine_translation(-100.0, -100.0);         // move to (0,0)
	/// m *= Pictor::trans_affine_rotation(30.0 * 3.1415926 / 180.0);  // Rotate
	/// m *= Pictor::trans_affine_translation(100.0, 100.0);           // move back to (100,100)
	///</summary>
	public struct Affine : ITransform
	{
		///<summary>
		///</summary>
		static public double AffineEpsilon = 1e-14;

		public double sx, shy, shx, sy, tx, ty;

		///<summary>
		///</summary>
		///<param name="copyFrom"></param>
		public Affine(Affine copyFrom)
		{
			sx = copyFrom.sx;
			shy = copyFrom.shy;
			shx = copyFrom.shx;
			sy = copyFrom.sy;
			tx = copyFrom.tx;
			ty = copyFrom.ty;
		}

		///<summary>
		///</summary>
		///<param name="v0"></param>
		///<param name="v1"></param>
		///<param name="v2"></param>
		///<param name="v3"></param>
		///<param name="v4"></param>
		///<param name="v5"></param>
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

		///<summary>
		///</summary>
		///<param name="m"></param>
		public Affine(double[] m)
		{
			sx = m[0];
			shy = m[1];
			shx = m[2];
			sy = m[3];
			tx = m[4];
			ty = m[5];
		}

		///<summary>
		///</summary>
		///<returns></returns>
		public static Affine NewIdentity()
		{
			Affine newAffine = new Affine { sx = 1.0, shy = 0.0, shx = 0.0, sy = 1.0, tx = 0.0, ty = 0.0 };

			return newAffine;
		}

		///<summary>
		///</summary>
		///<param name="angleRadians"></param>
		///<returns></returns>
		public static Affine NewRotation(double angleRadians)
		{
			return new Affine(Math.Cos(angleRadians), Math.Sin(angleRadians), -Math.Sin(angleRadians), Math.Cos(angleRadians), 0.0, 0.0);
		}

		///<summary>
		///</summary>
		///<param name="scale"></param>
		///<returns></returns>
		public static Affine NewScaling(double scale)
		{
			return new Affine(scale, 0.0, 0.0, scale, 0.0, 0.0);
		}

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		///<returns></returns>
		public static Affine NewScaling(double x, double y)
		{
			return new Affine(x, 0.0, 0.0, y, 0.0, 0.0);
		}

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		///<returns></returns>
		public static Affine NewTranslation(double x, double y)
		{
			return new Affine(1.0, 0.0, 0.0, 1.0, x, y);
		}

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		///<returns></returns>
		public static Affine NewSkewing(double x, double y)
		{
			return new Affine(1.0, Math.Tan(y), Math.Tan(x), 1.0, 0.0, 0.0);
		}

		///<summary>
		///</summary>
		public void Identity()
		{
			sx = sy = 1.0;
			shy = shx = tx = ty = 0.0;
		}

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		public void Translate(double x, double y)
		{
			tx += x;
			ty += y;
		}

		///<summary>
		///</summary>
		///<param name="angleRadians"></param>
		public void Rotate(double angleRadians)
		{
			double ca = Math.Cos(angleRadians);
			double sa = Math.Sin(angleRadians);
			double t0 = sx * ca - shy * sa;
			double t2 = shx * ca - sy * sa;
			double t4 = tx * ca - ty * sa;
			shy = sx * sa + shy * ca;
			sy = shx * sa + sy * ca;
			ty = tx * sa + ty * ca;
			sx = t0;
			shx = t2;
			tx = t4;
		}

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		public void Scale(double x, double y)
		{
			double mm0 = x; // Possible hint for the optimizer
			double mm3 = y;
			sx *= mm0;
			shx *= mm0;
			tx *= mm0;
			shy *= mm3;
			sy *= mm3;
			ty *= mm3;
		}

		///<summary>
		///</summary>
		///<param name="scaleAmount"></param>
		public void Scale(double scaleAmount)
		{
			sx *= scaleAmount;
			shx *= scaleAmount;
			tx *= scaleAmount;
			shy *= scaleAmount;
			sy *= scaleAmount;
			ty *= scaleAmount;
		}

		// Multiply matrix to another one
		void Multiply(Affine m)
		{
			double t0 = sx * m.sx + shy * m.shx;
			double t2 = shx * m.sx + sy * m.shx;
			double t4 = tx * m.sx + ty * m.shx + m.tx;
			shy = sx * m.shy + shy * m.sy;
			sy = shx * m.shy + sy * m.sy;
			ty = tx * m.shy + ty * m.sy + m.ty;
			sx = t0;
			shx = t2;
			tx = t4;
		}

		///<summary>
		///</summary>
		public void Invert()
		{
			double d = DeterminantReciprocal;

			double t0 = sy * d;
			sy = sx * d;
			shy = -shy * d;
			shx = -shx * d;

			double t4 = -tx * t0 - ty * shx;
			ty = -tx * shy - ty * sy;

			sx = t0;
			tx = t4;
		}


		///<summary>
		///</summary>
		///<param name="a"></param>
		///<param name="b"></param>
		///<returns></returns>
		public static Affine operator *(Affine a, Affine b)
		{
			Affine temp = new Affine(a);
			temp.Multiply(b);
			return temp;
		}

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		public void Transform(ref double x, ref double y)
		{
			double tmp = x;
			x = tmp * sx + y * shx + tx;
			y = tmp * shy + y * sy + ty;
		}

		///<summary>
		///</summary>
		///<param name="pointToTransform"></param>
		public void Transform(ref PointD pointToTransform)
		{
			Transform(ref pointToTransform.x, ref pointToTransform.y);
		}

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		public void InverseTransform(ref double x, ref double y)
		{
			double d = DeterminantReciprocal;
			double a = (x - tx) * d;
			double b = (y - ty) * d;
			x = a * sy - b * shx;
			y = b * sx - a * shy;
		}

		private double DeterminantReciprocal
		{
			get { return 1.0 / (sx * sy - shy * shx); }
		}


		public double GetScale()
		{
			double x = 0.707106781 * sx + 0.707106781 * shx;
			double y = 0.707106781 * shy + 0.707106781 * sy;
			return Math.Sqrt(x * x + y * y);
		}

		///<summary>
		/// Check to see if the matrix is not degenerate
		///</summary>
		///<param name="epsilon"></param>
		///<returns></returns>
		public bool IsValid(double epsilon)
		{
			return Math.Abs(sx) > epsilon && Math.Abs(sy) > epsilon;
		}

		/// <summary>
		/// Check to see if it's an Identity matrix
		/// </summary>
		/// <returns></returns>
		public bool IsIdentity()
		{
			return IsIdentity(AffineEpsilon);
		}

		///<summary>
		///</summary>
		///<param name="epsilon"></param>
		///<returns></returns>
		public bool IsIdentity(double epsilon)
		{
			return Basics.IsEqualEps(sx, 1.0, epsilon) &&
				Basics.IsEqualEps(shy, 0.0, epsilon) &&
				Basics.IsEqualEps(shx, 0.0, epsilon) &&
				Basics.IsEqualEps(sy, 1.0, epsilon) &&
				Basics.IsEqualEps(tx, 0.0, epsilon) &&
				Basics.IsEqualEps(ty, 0.0, epsilon);
		}

		///<summary>
		///</summary>
		///<param name="m"></param>
		///<param name="epsilon"></param>
		///<returns></returns>
		public bool IsEqual(Affine m, double epsilon)
		{
			return Basics.IsEqualEps(sx, m.sx, epsilon) &&
				Basics.IsEqualEps(shy, m.shy, epsilon) &&
				Basics.IsEqualEps(shx, m.shx, epsilon) &&
				Basics.IsEqualEps(sy, m.sy, epsilon) &&
				Basics.IsEqualEps(tx, m.tx, epsilon) &&
				Basics.IsEqualEps(ty, m.ty, epsilon);
		}

		///<summary>
		///</summary>
		///<returns></returns>
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

		///<summary>
		///</summary>
		///<param name="dx"></param>
		///<param name="dy"></param>
		public void Translation(out double dx, out double dy)
		{
			dx = tx;
			dy = ty;
		}

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
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

		///<summary>
		/// Used to Calculate Scaling coefficients in image resampling. 
		/// When there is considerable shear this method gives us much
		/// better estimation than just sx, sy.
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		public void ScalingAbs(out double x, out double y)
		{
			x = Math.Sqrt(sx * sx + shx * shx);
			y = Math.Sqrt(shy * shy + sy * sy);
		}
	};
}