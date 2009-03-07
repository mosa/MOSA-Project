using System;
using System.Runtime.InteropServices;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Glyph
    {
        /// <summary>
        /// 
        /// </summary>
        internal long index;

        /// <summary>
        /// 
        /// </summary>
        internal double x;

        /// <summary>
        /// 
        /// </summary>
        internal double y;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Glyph(long index, double x, double y)
        {
            this.index = index;
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Glyph)
                return this == (Glyph)obj;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)Index ^ (int)X ^ (int)Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="glyphs"></param>
        /// <returns></returns>
        internal static IntPtr GlyphsToIntPtr(Glyph[] glyphs)
        {
            int size = Marshal.SizeOf(glyphs[0]);
            IntPtr dest = Marshal.AllocHGlobal(size * glyphs.Length);
            long pos = dest.ToInt64();
            for (int i = 0; i < glyphs.Length; i++, pos += size)
                Marshal.StructureToPtr(glyphs[i], (IntPtr)pos, false);
            return dest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="glyph"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator ==(Glyph glyph, Glyph other)
        {
            return glyph.Index == other.Index && glyph.X == other.X && glyph.Y == other.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="glyph"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator !=(Glyph glyph, Glyph other)
        {
            return !(glyph == other);
        }
    }
}
