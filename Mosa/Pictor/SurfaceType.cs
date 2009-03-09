using System;

namespace Pictor
{
    /// <summary>
    /// An enumeration of various target platforms.
    /// This is obsolete and not used internally, as
    /// MOSA doesn't support Win32 or BeOS targets for
    /// example, as we provide our own Server target.
    /// </summary>
    [Serializable]
    public enum SurfaceType
    {
        /// <summary>
        /// 
        /// </summary>
        Image,
        /// <summary>
        /// 
        /// </summary>
        Pdf,
        /// <summary>
        /// 
        /// </summary>
        PS,
        /// <summary>
        /// 
        /// </summary>
        Xlib,
        /// <summary>
        /// 
        /// </summary>
        Xcb,
        /// <summary>
        /// 
        /// </summary>
        Glitz,
        /// <summary>
        /// 
        /// </summary>
        Quartz,
        /// <summary>
        /// 
        /// </summary>
        Win32,
        /// <summary>
        /// 
        /// </summary>
        BeOS,
        /// <summary>
        /// 
        /// </summary>
        DirectFB,
        /// <summary>
        /// 
        /// </summary>
        Svg,
    }
}
