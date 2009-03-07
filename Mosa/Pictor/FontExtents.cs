using System;
using System.Runtime.InteropServices;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FontExtents
    {
        /// <summary>
        /// 
        /// </summary>
        double ascent;

        /// <summary>
        /// 
        /// </summary>
        double descent;

        /// <summary>
        /// 
        /// </summary>
        double height;

        /// <summary>
        /// 
        /// </summary>
        double maxXAdvance;

        /// <summary>
        /// 
        /// </summary>
        double maxYAdvance;

        /// <summary>
        /// 
        /// </summary>
        public double Ascent
        {
            get { return ascent; }
            set { ascent = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Descent
        {
            get { return descent; }
            set { descent = value; }
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
        public double MaxXAdvance
        {
            get { return maxXAdvance; }
            set { maxXAdvance = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double MaxYAdvance
        {
            get { return maxYAdvance; }
            set { maxYAdvance = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ascent"></param>
        /// <param name="descent"></param>
        /// <param name="height"></param>
        /// <param name="maxXAdvance"></param>
        /// <param name="maxYAdvance"></param>
        public FontExtents(double ascent, double descent, double height, double maxXAdvance, double maxYAdvance)
        {
            this.ascent = ascent;
            this.descent = descent;
            this.height = height;
            this.maxXAdvance = maxXAdvance;
            this.maxYAdvance = maxYAdvance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is FontExtents)
                return this == (FontExtents)obj;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)Ascent ^ (int)Descent ^ (int)Height ^ (int)MaxXAdvance ^ (int)MaxYAdvance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extents"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator ==(FontExtents extents, FontExtents other)
        {
            return extents.Ascent == other.Ascent && extents.Descent == other.Descent && extents.Height == other.Height && extents.MaxXAdvance == other.MaxXAdvance && extents.MaxYAdvance == other.MaxYAdvance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extents"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator !=(FontExtents extents, FontExtents other)
        {
            return !(extents == other);
        }
    }
}
