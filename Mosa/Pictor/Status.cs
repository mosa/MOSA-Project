using System;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum Status
    {
        /// <summary>
        /// 
        /// </summary>
        Success         = 0,
        /// <summary>
        /// 
        /// </summary>
        NoMemory,
        /// <summary>
        /// 
        /// </summary>
        InvalidRestore,
        /// <summary>
        /// 
        /// </summary>
        InvalidPopGroup,
        /// <summary>
        /// 
        /// </summary>
        NoCurrentPoint,
        /// <summary>
        /// 
        /// </summary>
        InvalidMatrix,
        /// <summary>
        /// 
        /// </summary>
        InvalidStatus,
        /// <summary>
        /// 
        /// </summary>
        NullPointer,
        /// <summary>
        /// 
        /// </summary>
        InvalidString,
        /// <summary>
        /// 
        /// </summary>
        InvalidPathData,
        /// <summary>
        /// 
        /// </summary>
        ReadError,
        /// <summary>
        /// 
        /// </summary>
        WriteError,
        /// <summary>
        /// 
        /// </summary>
        SurfaceFinished,
        /// <summary>
        /// 
        /// </summary>
        SurfaceTypeMismatch,
        /// <summary>
        /// 
        /// </summary>
        PatternTypeMismatch,
        /// <summary>
        /// 
        /// </summary>
        InvalidContent,
        /// <summary>
        /// 
        /// </summary>
        InvalidFormat,
        /// <summary>
        /// 
        /// </summary>
        InvalidVisual,
        /// <summary>
        /// 
        /// </summary>
        FileNotFound,
        /// <summary>
        /// 
        /// </summary>
        InvalidDash,
        /// <summary>
        /// 
        /// </summary>
        InvalidDscComment,
        /// <summary>
        /// 
        /// </summary>
        InvalidIndex,
        /// <summary>
        /// 
        /// </summary>
        ClipNotRepresentable,
    }
}
