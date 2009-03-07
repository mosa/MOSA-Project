using System;
using System.Runtime.InteropServices;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TextExtents
    {
        /// <summary>
        /// 
        /// </summary>
        double xbearing;

        /// <summary>
        /// 
        /// </summary>
        double ybearing;

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
        double xadvance;

        /// <summary>
        /// 
        /// </summary>
        double yadvance;

        /// <summary>
        /// 
        /// </summary>
        public double XBearing
        {
            get { return xbearing; }
            set { xbearing = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double YBearing
        {
            get { return ybearing; }
            set { ybearing = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double XAdvance
        {
            get { return xadvance; }
            set { xadvance = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double YAdvance
        {
            get { return yadvance; }
            set { yadvance = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is TextExtents)
                return this == (TextExtents)obj;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)XBearing ^ (int)YBearing ^ (int)Width ^ (int)Height ^ (int)XAdvance ^ (int)YAdvance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extents"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator ==(TextExtents extents, TextExtents other)
        {
            return extents.XBearing == other.XBearing && extents.YBearing == other.YBearing && extents.Width == other.Width && extents.Height == other.Height && extents.XAdvance == other.XAdvance && extents.YAdvance == other.YAdvance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extents"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator !=(TextExtents extents, TextExtents other)
        {
            return !(extents == other);
        }
    }
}