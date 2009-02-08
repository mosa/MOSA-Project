/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

namespace Pictor.Objects
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Primitive
    {
        /// <summary>
        /// 
        /// </summary>
        private double _scale = 1.0;
        /// <summary>
        /// Vertexes the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public abstract PathCommand Vertex(ref double x, ref double y);
        /// <summary>
        /// Rewinds the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public abstract void Rewind(uint id);

        /// <summary>
        /// Gets or sets the approximation scale.
        /// </summary>
        /// <value>The approximation scale.</value>
        public virtual double ApproximationScale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
            }
        }
    }
}