/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@mosa-project.org>)
 */

using System.Collections.Generic;

namespace Pictor.Objects.Curves
{
    /// <summary>
    /// 
    /// </summary>
    public class Curve4Div : Curve
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
        private double _cuspLimit = 0.0;
        /// <summary>
        /// 
        /// </summary>
        private int _count = 0;
        /// <summary>
        /// 
        /// </summary>
        private List<Math.Point<double >> _points = null;

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
        public void Init(double x1, double y1,
                          double x2, double y2,
                          double x3, double y3,
                          double x4, double y4)
        {
            _points.Clear();
            _distanceToleranceSquare = 0.5 / ApproximationScale;
            _distanceToleranceSquare *= _distanceToleranceSquare;
            Bezier(x1, y1, x2, y2, x3, y3, x4, y4);
            _count = 0;
        }

        /// <summary>
        /// Rewinds the specified ?.
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
        /// <param name="x4">The x4.</param>
        /// <param name="y4">The y4.</param>
        private void Bezier(double x1, double y1,
                            double x2, double y2,
                            double x3, double y3,
                            double x4, double y4)
        {
            _points.Add(new Math.Point<double>(x1, y1));
            RecursiveBezier(x1, y1, x2, y2, x3, y3, x4, y4, 0);
            _points.Add(new Math.Point<double>(x4, y4));
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
        /// <param name="x4">The x4.</param>
        /// <param name="y4">The y4.</param>
        /// <param name="level">The level.</param>
        private void RecursiveBezier(double x1, double y1,
                                     double x2, double y2,
                                     double x3, double y3,
                                     double x4, double y4,
                                     uint level)
        {
            if(level > CurveRecursionLimit) 
            {
                return;
            }

            // Calculate all the mid-points of the line segments
            double x12   = (x1 + x2) / 2;
            double y12   = (y1 + y2) / 2;
            double x23   = (x2 + x3) / 2;
            double y23   = (y2 + y3) / 2;
            double x34   = (x3 + x4) / 2;
            double y34   = (y3 + y4) / 2;
            double x123  = (x12 + x23) / 2;
            double y123  = (y12 + y23) / 2;
            double x234  = (x23 + x34) / 2;
            double y234  = (y23 + y34) / 2;
            double x1234 = (x123 + x234) / 2;
            double y1234 = (y123 + y234) / 2;


            // Try to approximate the full cubic curve by a single straight line
            double dx = x4-x1;
            double dy = y4-y1;

            double d2 = System.Math.Abs(((x2 - x4) * dy - (y2 - y4) * dx));
            double d3 = System.Math.Abs(((x3 - x4) * dy - (y3 - y4) * dx));
            double da1, da2, k;

            switch ( (int.Parse((d2 > CurveCollinearityEpsilon).ToString()) << 1) + 
                      int.Parse((d3 > CurveCollinearityEpsilon).ToString()))
            {
            case 0:
                // All collinear OR p1==p4
                k = dx*dx + dy*dy;
                if(k == 0)
                {
                    d2 = Math.MathHelper.CalculateSquareDistance(x1, y1, x2, y2);
                    d3 = Math.MathHelper.CalculateSquareDistance(x4, y4, x3, y3);
                }
                else
                {
                    k   = 1 / k;
                    da1 = x2 - x1;
                    da2 = y2 - y1;
                    d2  = k * (da1*dx + da2*dy);
                    da1 = x3 - x1;
                    da2 = y3 - y1;
                    d3  = k * (da1*dx + da2*dy);
                    if(d2 > 0 && d2 < 1 && d3 > 0 && d3 < 1)
                    {
                        // Simple collinear case, 1---2---3---4
                        // We can leave just two endpoints
                        return;
                    }
                         if(d2 <= 0) d2 = Math.MathHelper.CalculateSquareDistance(x2, y2, x1, y1);
                    else if(d2 >= 1) d2 = Math.MathHelper.CalculateSquareDistance(x2, y2, x4, y4);
                    else             d2 = Math.MathHelper.CalculateSquareDistance(x2, y2, x1 + d2*dx, y1 + d2*dy);

                         if(d3 <= 0) d3 = Math.MathHelper.CalculateSquareDistance(x3, y3, x1, y1);
                    else if(d3 >= 1) d3 = Math.MathHelper.CalculateSquareDistance(x3, y3, x4, y4);
                    else             d3 = Math.MathHelper.CalculateSquareDistance(x3, y3, x1 + d3*dx, y1 + d3*dy);
                }
                if(d2 > d3)
                {
                    if(d2 < _distanceToleranceSquare)
                    {
                        _points.Add(new Math.Point<double>(x2, y2));
                        return;
                    }
                }
                else
                {
                    if(d3 < _distanceToleranceSquare)
                    {
                        _points.Add(new Math.Point<double>(x3, y3));
                        return;
                    }
                }
                break;

            case 1:
                // p1,p2,p4 are collinear, p3 is significant
                if(d3 * d3 <= _distanceToleranceSquare * (dx*dx + dy*dy))
                {
                    if(_angleTolerance < CurveAngleToleranceEpsilon)
                    {
                        _points.Add(new Math.Point<double>(x23, y23));
                        return;
                    }

                    // Angle Condition
                    da1 = System.Math.Abs(System.Math.Atan2(y4 - y3, x4 - x3) - System.Math.Atan2(y3 - y2, x3 - x2));
                    if(da1 >= System.Math.PI) da1 = 2 * System.Math.PI - da1;

                    if(da1 < _angleTolerance)
                    {
                        _points.Add(new Math.Point<double>(x2, y2));
                        _points.Add(new Math.Point<double>(x3, y3));
                        return;
                    }

                    if(_cuspLimit != 0.0)
                    {
                        if(da1 > _cuspLimit)
                        {
                            _points.Add(new Math.Point<double>(x3, y3));
                            return;
                        }
                    }
                }
                break;

            case 2:
                // p1,p3,p4 are collinear, p2 is significant
                if(d2 * d2 <= _distanceToleranceSquare * (dx*dx + dy*dy))
                {
                    if(_angleTolerance < CurveAngleToleranceEpsilon)
                    {
                        _points.Add(new Math.Point<double>(x23, y23));
                        return;
                    }

                    // Angle Condition
                    da1 = System.Math.Abs(System.Math.Atan2(y3 - y2, x3 - x2) - System.Math.Atan2(y2 - y1, x2 - x1));
                    if(da1 >= System.Math.PI) da1 = 2 * System.Math.PI - da1;

                    if(da1 < _angleTolerance)
                    {
                        _points.Add(new Math.Point<double>(x2, y2));
                        _points.Add(new Math.Point<double>(x3, y3));
                        return;
                    }

                    if(_cuspLimit != 0.0)
                    {
                        if(da1 > _cuspLimit)
                        {
                            _points.Add(new Math.Point<double>(x2, y2));
                            return;
                        }
                    }
                }
                break;

            case 3: 
                // Regular case
                if((d2 + d3)*(d2 + d3) <= _distanceToleranceSquare * (dx*dx + dy*dy))
                {
                    // If the curvature doesn't exceed the distance_tolerance value
                    // we tend to finish subdivisions.
                    if(_angleTolerance < CurveAngleToleranceEpsilon)
                    {
                        _points.Add(new Math.Point<double>(x23, y23));
                        return;
                    }

                    // Angle & Cusp Condition
                    k = System.Math.Atan2(y3 - y2, x3 - x2);
                    da1 = System.Math.Abs(k - System.Math.Atan2(y2 - y1, x2 - x1));
                    da2 = System.Math.Abs(System.Math.Atan2(y4 - y3, x4 - x3) - k);
                    if(da1 >= System.Math.PI) 
                        da1 = 2 * System.Math.PI - da1;
                    if(da2 >= System.Math.PI) 
                        da2 = 2 * System.Math.PI - da2;

                    if(da1 + da2 < _angleTolerance)
                    {
                        // Finally we can stop the recursion
                        _points.Add(new Math.Point<double>(x23, y23));
                        return;
                    }

                    if(_cuspLimit != 0.0)
                    {
                        if(da1 > _cuspLimit)
                        {
                            _points.Add(new Math.Point<double>(x2, y2));
                            return;
                        }

                        if(da2 > _cuspLimit)
                        {
                            _points.Add(new Math.Point<double>(x3, y3));
                            return;
                        }
                    }
                }
                break;
            }

            // Continue subdivision
            RecursiveBezier(x1,       y1,  x12,  y12, x123, y123, x1234, y1234, level + 1); 
            RecursiveBezier(x1234, y1234, x234, y234,  x34,  y34,    x4,    y4, level + 1); 
        }
    }
}