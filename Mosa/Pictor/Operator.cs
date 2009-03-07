using System;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum Operator
    {
        /// <summary>
        /// 
        /// </summary>
        Clear,
        /// <summary>
        /// 
        /// </summary>
        Source,
        /// <summary>
        /// 
        /// </summary>
        Over,
        /// <summary>
        /// 
        /// </summary>
        In,
        /// <summary>
        /// 
        /// </summary>
        Out,
        /// <summary>
        /// 
        /// </summary>
        Atop,

        /// <summary>
        /// 
        /// </summary>
        Dest,
        /// <summary>
        /// 
        /// </summary>
        DestOver,
        /// <summary>
        /// 
        /// </summary>
        DestIn,
        /// <summary>
        /// 
        /// </summary>
        DestOut,
        /// <summary>
        /// 
        /// </summary>
        DestAtop,

        /// <summary>
        /// 
        /// </summary>
        Xor,
        /// <summary>
        /// 
        /// </summary>
        Add,
        /// <summary>
        /// 
        /// </summary>
        Saturate,
    }
}
