/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

namespace Pictor.Objects.Curves
{
    /// <summary>
    /// 
    /// </summary>
    public class Curve : Arcs.Arc
    {
        /// <summary>
        /// 
        /// </summary>
        public const double CurveDistanceEpsilon        = 1e-30;
        /// <summary>
        /// 
        /// </summary>
        public const double CurveCollinearityEpsilon    = 1e-30;
        /// <summary>
        /// 
        /// </summary>
        public const double CurveAngleToleranceEpsilon  = 0.01;
        /// <summary>
        /// 
        /// </summary>
        public const int    CurveRecursionLimit         = 32;

        /// <summary>
        /// Gets or sets the approximation method.
        /// </summary>
        /// <value>The approximation method.</value>
        public CurveApproximationMethod ApproximationMethod
        {
            get
            {
                return CurveApproximationMethod.CurveNotSet;
            }
            set
            {
            }
        }
    }
}