/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

namespace Pictor.Renderer.ColorTypes
{
    /// <summary>
    /// 
    /// </summary>
    public class RgbColor<ValueType> : IColorType
    {
        /// <summary>
        /// 
        /// </summary>
        public ValueType r;
        /// <summary>
        /// 
        /// </summary>
        public ValueType g;
        /// <summary>
        /// 
        /// </summary>
        public ValueType b;
    }
}