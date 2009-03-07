using System;
using System.Runtime.InteropServices;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class Matrix : ICloneable
    {
        /// <summary>
        /// 
        /// </summary>
        public double Xx;

        /// <summary>
        /// 
        /// </summary>
        public double Yx;

        /// <summary>
        /// 
        /// </summary>
        public double Xy;

        /// <summary>
        /// 
        /// </summary>
        public double Yy;

        /// <summary>
        /// 
        /// </summary>
        public double X0;

        /// <summary>
        /// 
        /// </summary>
        public double Y0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yx"></param>
        /// <param name="xy"></param>
        /// <param name="yy"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        public Matrix(double xx, double yx, double xy, double yy,
                double x0, double y0)
        {
            this.Xx = xx; this.Yx = yx; this.Xy = xy;
            this.Yy = yy; this.X0 = x0; this.Y0 = y0;
        }

        /// <summary>
        /// 
        /// </summary>
        public Matrix()
        {
            this.InitIdentity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsIdentity()
        {
            return (this == new Matrix());
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitIdentity()
        {
            // this.Init(1,0,0,1,0,0);
            //NativeMethods.cairo_matrix_init_identity(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yx"></param>
        /// <param name="xy"></param>
        /// <param name="yy"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        public void Init(double xx, double yx, double xy, double yy,
                  double x0, double y0)
        {
            this.Xx = xx; this.Yx = yx; this.Xy = xy;
            this.Yy = yy; this.X0 = x0; this.Y0 = y0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        public void InitTranslate(double tx, double ty)
        {
            //this.Init (1, 0, 0, 1, tx, ty);
            //NativeMethods.cairo_matrix_init_translate(this, tx, ty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        public void Translate(double tx, double ty)
        {
//            NativeMethods.cairo_matrix_translate(this, tx, ty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        public void InitScale(double sx, double sy)
        {
            //this.Init (sx, 0, 0, sy, 0, 0);
            //NativeMethods.cairo_matrix_init_scale(this, sx, sy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        public void Scale(double sx, double sy)
        {
            //NativeMethods.cairo_matrix_scale(this, sx, sy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radians"></param>
        public void InitRotate(double radians)
        {
            /*
            double s, c;
            s = Math.Sin (radians);
            c = Math.Cos (radians);
            this.Init (c, s, -s, c, 0, 0);
            */
            //NativeMethods.cairo_matrix_init_rotate(this, radians);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radians"></param>
        public void Rotate(double radians)
        {
            //NativeMethods.cairo_matrix_rotate(this, radians);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Pictor.Status Invert()
        {
            return Status.InvalidContent;// NativeMethods.cairo_matrix_invert(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        public void Multiply(Matrix b)
        {
            Matrix a = (Matrix)this.Clone();
            //NativeMethods.cairo_matrix_multiply(this, a, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix Multiply(Matrix a, Matrix b)
        {
            Matrix result = new Matrix();
            //NativeMethods.cairo_matrix_multiply(result, a, b);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void TransformDistance(ref double dx, ref double dy)
        {
            //NativeMethods.cairo_matrix_transform_distance(this, ref dx, ref dy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void TransformPoint(ref double x, ref double y)
        {
            //NativeMethods.cairo_matrix_transform_point(this, ref x, ref y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            String s = String.Format("xx:{0:##0.0#} yx:{1:##0.0#} xy:{2:##0.0#} yy:{3:##0.0#} x0:{4:##0.0#} y0:{5:##0.0#}",
                this.Xx, this.Yx, this.Xy, this.Yy, this.X0, this.Y0);
            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(Matrix lhs, Matrix rhs)
        {
            return (lhs.Xx == rhs.Xx &&
                lhs.Xy == rhs.Xy &&
                lhs.Yx == rhs.Yx &&
                lhs.Yy == rhs.Yy &&
                lhs.X0 == rhs.X0 &&
                lhs.Y0 == rhs.Y0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(Matrix lhs, Matrix rhs)
        {
            return !(lhs == rhs);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o)
        {
            if (!(o is Matrix))
                return false;
            else
                return (this == (Matrix)o);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)this.Xx ^ (int)this.Xx >> 32 ^
                (int)this.Xy ^ (int)this.Xy >> 32 ^
                (int)this.Yx ^ (int)this.Yx >> 32 ^
                (int)this.Yy ^ (int)this.Yy >> 32 ^
                (int)this.X0 ^ (int)this.X0 >> 32 ^
                (int)this.Y0 ^ (int)this.Y0 >> 32;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}