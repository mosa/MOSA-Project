using System;
using System.Collections.Generic;
using System.Text;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        /// 
        /// </summary>
        double x;

        /// <summary>
        /// 
        /// </summary>
        double y;

        /// <summary>
        /// 
        /// </summary>
        double width;

        /// <summary>
        /// 
        /// </summary>
        double height;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle(Point point, double width, double height)
        {
            x = point.X;
            y = point.Y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// 
        /// </summary>
        public double X
        {
            get { return x; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Y
        {
            get { return y; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Width
        {
            get { return width; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Height
        {
            get { return height; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Rectangle)
                return this == (Rectangle)obj;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)(x + y + width + height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("x:{0} y:{1} w:{2} h:{3}", x, y, width, height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator ==(Rectangle rectangle, Rectangle other)
        {
            return rectangle.X == other.X && rectangle.Y == other.Y && rectangle.Width == other.Width && rectangle.Height == other.Height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator !=(Rectangle rectangle, Rectangle other)
        {
            return !(rectangle == other);
        }
    }
}
