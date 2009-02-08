/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

namespace Pictor.Objects.Curves
{
    /// <summary>
    /// 
    /// </summary>
    public class Curve4Points
    {
        /// <summary>
        /// 
        /// </summary>
        public double[] cp = new double[8];

        /// <summary>
        /// Gets or sets the curve points.
        /// </summary>
        /// <value>The curve points.</value>
        public double[] CurvePoints
        {
            get
            {
                return cp;
            }
            set
            {
                cp = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Double"/> at the specified index.
        /// </summary>
        /// <value></value>
        public double this[int index]
        {
            get
            {
                return CurvePoints[index];
            }
            set
            {
                CurvePoints[index] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Curve4Points"/> class.
        /// </summary>
        public Curve4Points() 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Curve4Points"/> class.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="x3">The x3.</param>
        /// <param name="y3">The y3.</param>
        /// <param name="x4">The x4.</param>
        /// <param name="y4">The y4.</param>
        public Curve4Points(double x1, double y1,
                            double x2, double y2,
                            double x3, double y3,
                            double x4, double y4)
        {
            cp[0] = x1; cp[1] = y1; cp[2] = x2; cp[3] = y2;
            cp[4] = x3; cp[5] = y3; cp[6] = x4; cp[7] = y4;
        }
        /// <summary>
        /// Inits the specified x1.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="x3">The x3.</param>
        /// <param name="y3">The y3.</param>
        /// <param name="x4">The x4.</param>
        /// <param name="y4">The y4.</param>
        void Init(double x1, double y1,
                  double x2, double y2,
                  double x3, double y3,
                  double x4, double y4)
        {
            cp[0] = x1; cp[1] = y1; cp[2] = x2; cp[3] = y2;
            cp[4] = x3; cp[5] = y3; cp[6] = x4; cp[7] = y4;
        }
    }
}