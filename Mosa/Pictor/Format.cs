using System;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum Format
    {
        /// <summary>
        /// 
        /// </summary>
        Argb32      = 0,
        /// <summary>
        /// 
        /// </summary>
        Rgb24       = 1,
        /// <summary>
        /// 
        /// </summary>
        A8          = 2,
        /// <summary>
        /// 
        /// </summary>
        A1          = 3,
        /// <summary>
        /// 
        /// </summary>
        Rgb16565    = 4,

        //[Obsolete ("Use Argb32")]
        /// <summary>
        /// 
        /// </summary>
        ARGB32      = Argb32,
        //[Obsolete ("Use Rgb24")]
        /// <summary>
        /// 
        /// </summary>
        RGB24       = Rgb24,
    }
}
