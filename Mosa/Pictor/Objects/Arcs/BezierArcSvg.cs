/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@mosa-project.org>)
 */

namespace Pictor.Objects.Arcs
{
    /// <summary>
    /// Compute an SVG-style bezier arc. 
    ///
    /// Computes an elliptical arc from (x1, y1) to (x2, y2). The size and 
    /// orientation of the ellipse are defined by two radii (rx, ry) 
    /// and an x-axis-rotation, which indicates how the ellipse as a whole 
    /// is rotated relative to the current coordinate system. The center 
    /// (cx, cy) of the ellipse is calculated automatically to satisfy the 
    /// constraints imposed by the other parameters. 
    /// large-arc-flag and sweep-flag contribute to the automatic calculations 
    /// and help determine how the arc is drawn.
    /// </summary>
    public class BezierArcSvg : BezierArc
    {
        /// <summary>
        /// 
        /// </summary>
        private bool _radiiOk = false;

        /// <summary>
        /// Gets a value indicating whether [radii ok].
        /// </summary>
        /// <value><c>true</c> if [radii ok]; otherwise, <c>false</c>.</value>
        public bool RadiiOk
        {
            get
            {
                return _radiiOk;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BezierArcSvg"/> class.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="rx">The rx.</param>
        /// <param name="ry">The ry.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="largeArcFlag">if set to <c>true</c> [large arc flag].</param>
        /// <param name="sweepFlag">if set to <c>true</c> [sweep flag].</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        public BezierArcSvg(double x1, double y1, 
                       double rx, double ry, 
                       double angle,
                       bool largeArcFlag,
                       bool sweepFlag,
                       double x2, double y2) 
        {
            Init(x1, y1, rx, ry, angle, largeArcFlag, sweepFlag, x2, y2);
        }

        /// <summary>
        /// Inits the specified x0.
        /// </summary>
        /// <param name="x0">The x0.</param>
        /// <param name="y0">The y0.</param>
        /// <param name="rx">The rx.</param>
        /// <param name="ry">The ry.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="largeArcFlag">if set to <c>true</c> [largeArcFlag].</param>
        /// <param name="sweepFlag">if set to <c>true</c> [sweepFlag].</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        public void Init(double x0, double y0,
                         double rx, double ry,
                         double angle,
                         bool largeArcFlag,
                         bool sweepFlag,
                         double x2, double y2)
        {
            _radiiOk = true;

            if (rx < 0.0) rx = -rx;
            if (ry < 0.0) ry = -rx;

            // Calculate the middle point between 
            // the current and the final points
            double dx2 = (x0 - x2) / 2.0;
            double dy2 = (y0 - y2) / 2.0;

            double cos_a = System.Math.Cos(angle);
            double sin_a = System.Math.Sin(angle);

            // Calculate (x1, y1)
            double x1 = cos_a * dx2 + sin_a * dy2;
            double y1 = -sin_a * dx2 + cos_a * dy2;

            // Ensure radii are large enough
            double prx = rx * rx;
            double pry = ry * ry;
            double px1 = x1 * x1;
            double py1 = y1 * y1;

            // Check that radii are large enough
            double radii_check = px1 / prx + py1 / pry;
            if (radii_check > 1.0)
            {
                rx = System.Math.Sqrt(radii_check) * rx;
                ry = System.Math.Sqrt(radii_check) * ry;
                prx = rx * rx;
                pry = ry * ry;
                if (radii_check > 10.0) 
                    _radiiOk = false;
            }

            // Calculate (cx1, cy1)
            double sign = (largeArcFlag == sweepFlag) ? -1.0 : 1.0;
            double sq = (prx * pry - prx * py1 - pry * px1) / (prx * py1 + pry * px1);
            double coef = sign * System.Math.Sqrt((sq < 0) ? 0 : sq);
            double cx1 = coef * ((rx * y1) / ry);
            double cy1 = coef * -((ry * x1) / rx);

            //
            // Calculate (cx, cy) from (cx1, cy1)
            double sx2 = (x0 + x2) / 2.0;
            double sy2 = (y0 + y2) / 2.0;
            double cx = sx2 + (cos_a * cx1 - sin_a * cy1);
            double cy = sy2 + (sin_a * cx1 + cos_a * cy1);

            // Calculate the start_angle (angle1) and the sweep_angle (dangle)
            double ux = (x1 - cx1) / rx;
            double uy = (y1 - cy1) / ry;
            double vx = (-x1 - cx1) / rx;
            double vy = (-y1 - cy1) / ry;
            double p, n;

            // Calculate the angle start
            n = System.Math.Sqrt(ux * ux + uy * uy);
            p = ux; // (1 * ux) + (0 * uy)
            sign = (uy < 0) ? -1.0 : 1.0;
            double v = p / n;
            if (v < -1.0) 
                v = -1.0;
            if (v > 1.0) 
                v = 1.0;
            double start_angle = sign * System.Math.Acos(v);

            // Calculate the sweep angle
            n = System.Math.Sqrt((ux * ux + uy * uy) * (vx * vx + vy * vy));
            p = ux * vx + uy * vy;
            sign = (ux * vy - uy * vx < 0) ? -1.0 : 1.0;
            v = p / n;
            if (v < -1.0) 
                v = -1.0;
            if (v > 1.0) 
                v = 1.0;
            double sweep_angle = sign * System.Math.Acos(v);
            if (!sweepFlag && sweep_angle > 0)
            {
                sweep_angle -= System.Math.PI * 2.0;
            }
            else if (sweepFlag && sweep_angle < 0)
            {
                sweep_angle += System.Math.PI * 2.0;
            }

            // We can now build and transform the resulting arc
            Init(0.0, 0.0, rx, ry, start_angle, sweep_angle);
            Math.AffineTransformation mtx = new Math.AffineRotationTransformation(angle);
            mtx *= new Math.AffineTranslationTransformation(cx, cy);

            for (int i = 2; i < NumberOfVertices - 2; i += 2)
            {
                mtx.Transform(ref _vertices, i, ref _vertices, i + 1);
            }

            // We must make sure that the starting and ending points
            // exactly coincide with the initial (x0,y0) and (x2,y2)
            Vertices[0] = x0;
            Vertices[1] = y0;

            if (NumberOfVertices > 2)
            {
                Vertices[NumberOfVertices - 2] = x2;
                Vertices[NumberOfVertices - 1] = y2;
            }
        }
    }
}