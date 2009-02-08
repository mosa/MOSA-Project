/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System.Collections.Generic;

namespace Pictor.Objects.Curves
{
    /// <summary>
    /// 
    /// </summary>
    public class Curve3Div : Curve
    {
        /// <summary>
        /// 
        /// </summary>
        private double _distanceToleranceSquare = 0.0;
        /// <summary>
        /// 
        /// </summary>
        private double _angleTolerance = 0.0;
        /// <summary>
        /// 
        /// </summary>
        private int _count = 0;
        /// <summary>
        /// 
        /// </summary>
        private List<Math.Point<double> > _points = null;

        /// <summary>
        /// Gets or sets the approximation method.
        /// </summary>
        /// <value>The approximation method.</value>
        public new CurveApproximationMethod ApproximationMethod
        {
            get
            {
                return CurveApproximationMethod.CurveDiv;
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the angle tolerance.
        /// </summary>
        /// <value>The angle tolerance.</value>
        public double AngleTolerance
        {
            get 
            { 
                return _angleTolerance; 
            }
            set 
            {
                _angleTolerance = value; 
            }
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
        public void Init(double x1, double y1,
                         double x2, double y2,
                         double x3, double y3)
        {
            _points.Clear();
            _distanceToleranceSquare  = 0.5 / ApproximationScale;
            _distanceToleranceSquare *= _distanceToleranceSquare;
            Bezier(x1, y1, x2, y2, x3, y3);
            _count = 0;
        }

        /// <summary>
        /// Rewinds the curve.
        /// </summary>
        /// <param name="pathId">The path id.</param>
        public override void Rewind(uint pathId)
        {
            _count = 0;
        }

        /// <summary>
        /// Vertexes the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public override PathCommand Vertex(ref double x, ref double y)
        {
            if (_count >= _points.Count) 
                return PathCommand.PathStop;

            x = _points[_count].x;
            y = _points[_count++].y;

            return (_count == 1) ? PathCommand.PathMoveTo : PathCommand.PathLineTo;
        }

        /// <summary>
        /// Beziers the specified x1.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="x3">The x3.</param>
        /// <param name="y3">The y3.</param>
        private void Bezier(double x1, double y1,
                            double x2, double y2,
                            double x3, double y3)
        {
            _points.Add(new Math.Point<double>(x1, y1));
            RecursiveBezier(x1, y1, x2, y2, x3, y3, 0);
            _points.Add(new Math.Point<double>(x3, y3));
        }

        /// <summary>
        /// Recursives the bezier.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="x3">The x3.</param>
        /// <param name="y3">The y3.</param>
        /// <param name="level">The level.</param>
        private void RecursiveBezier(double x1, double y1,
                                      double x2, double y2,
                                      double x3, double y3,
                                      uint level)
        {
            if (level > CurveRecursionLimit)
            {
                return;
            }

            // Calculate all the mid-points of the line segments
            //----------------------
            double x12 = (x1 + x2) / 2;
            double y12 = (y1 + y2) / 2;
            double x23 = (x2 + x3) / 2;
            double y23 = (y2 + y3) / 2;
            double x123 = (x12 + x23) / 2;
            double y123 = (y12 + y23) / 2;

            double dx = x3 - x1;
            double dy = y3 - y1;
            double d = System.Math.Abs(((x2 - x3) * dy - (y2 - y3) * dx));
            double da;

            if (d > CurveCollinearityEpsilon)
            {
                // Regular 
                if (d * d <= _distanceToleranceSquare * (dx * dx + dy * dy))
                {
                    // If the curvature doesn't exceed the distance_tolerance value
                    // we tend to finish subdivisions.
                    if (_angleTolerance < CurveAngleToleranceEpsilon)
                    {
                        _points.Add(new Math.Point<double>(x123, y123));
                        return;
                    }

                    // Angle & Cusp Condition
                    da = System.Math.Abs(System.Math.Atan2(y3 - y2, x3 - x2) - System.Math.Atan2(y2 - y1, x2 - x1));
                    if (da >= System.Math.PI) 
                        da = 2 * System.Math.PI - da;

                    if (da < _angleTolerance)
                    {
                        // Finally we can stop the recursion
                        _points.Add(new Math.Point<double>(x123, y123));
                        return;
                    }
                }
            }
            else
            {
                // Collinear case
                da = dx * dx + dy * dy;
                if (da == 0)
                {
                    d = Math.MathHelper.CalculateSquareDistance(x1, y1, x2, y2);
                }
                else
                {
                    d = ((x2 - x1) * dx + (y2 - y1) * dy) / da;
                    if (d > 0 && d < 1)
                    {
                        // Simple collinear case, 1---2---3
                        // We can leave just two endpoints
                        return;
                    }
                    if (d <= 0) 
                        d = Math.MathHelper.CalculateSquareDistance(x2, y2, x1, y1);
                    else if (d >= 1) 
                        d = Math.MathHelper.CalculateSquareDistance(x2, y2, x3, y3);
                    else 
                        d = Math.MathHelper.CalculateSquareDistance(x2, y2, x1 + d * dx, y1 + d * dy);
                }
                if (d < _distanceToleranceSquare)
                {
                    _points.Add(new Math.Point<double>(x2, y2));
                    return;
                }
            }

            // Continue subdivision
            RecursiveBezier(x1,     y1, x12, y12, x123, y123, level + 1);
            RecursiveBezier(x123, y123, x23, y23,   x3,   y3, level + 1); 
        }
    }
}