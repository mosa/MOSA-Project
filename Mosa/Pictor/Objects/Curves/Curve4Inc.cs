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
    public class Curve4Inc : Curve
    {
        /// <summary>
        /// 
        /// </summary>
        private int _numberOfSteps;
        /// <summary>
        /// 
        /// </summary>
        private int _step;
        /// <summary>
        /// 
        /// </summary>
        private double _start_x;
        /// <summary>
        /// 
        /// </summary>
        private double _start_y;
        /// <summary>
        /// 
        /// </summary>
        private double _end_x;
        /// <summary>
        /// 
        /// </summary>
        private double _end_y;
        /// <summary>
        /// 
        /// </summary>
        private double _fx;
        /// <summary>
        /// 
        /// </summary>
        private double _fy;
        /// <summary>
        /// 
        /// </summary>
        private double _dfx;
        /// <summary>
        /// 
        /// </summary>
        private double _dfy;
        /// <summary>
        /// 
        /// </summary>
        private double _ddfx;
        /// <summary>
        /// 
        /// </summary>
        private double _ddfy;
        /// <summary>
        /// 
        /// </summary>
        private double _dddfx;
        /// <summary>
        /// 
        /// </summary>
        private double _dddfy;
        /// <summary>
        /// 
        /// </summary>
        private double _saved_fx;
        /// <summary>
        /// 
        /// </summary>
        private double _saved_fy;
        /// <summary>
        /// 
        /// </summary>
        private double _saved_dfx;
        /// <summary>
        /// 
        /// </summary>
        private double _saved_dfy;
        /// <summary>
        /// 
        /// </summary>
        private double _saved_ddfx;
        /// <summary>
        /// 
        /// </summary>
        private double _saved_ddfy;

        /// <summary>
        /// Inits the specified cp.
        /// </summary>
        /// <param name="cp">The cp.</param>
        public void Init(Curve4Points cp)
        {
            Init(cp[0], cp[1], cp[2], cp[3], cp[4], cp[5], cp[6], cp[7]);
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
        public void Init(double x1, double y1,
                         double x2, double y2,
                         double x3, double y3,
                         double x4, double y4)
        {
            _start_x = x1;
            _start_y = y1;
            _end_x = x4;
            _end_y = y4;

            double dx1 = x2 - x1;
            double dy1 = y2 - y1;
            double dx2 = x3 - x2;
            double dy2 = y3 - y2;
            double dx3 = x4 - x3;
            double dy3 = y4 - y3;

            double len = (System.Math.Sqrt(dx1 * dx1 + dy1 * dy1) +
                          System.Math.Sqrt(dx2 * dx2 + dy2 * dy2) +
                          System.Math.Sqrt(dx3 * dx3 + dy3 * dy3)) * 0.25 * ApproximationScale;

            _numberOfSteps = (int)System.Math.Round(len);

            if (_numberOfSteps < 4)
            {
                _numberOfSteps = 4;
            }

            double subdivide_step = 1.0 / _numberOfSteps;
            double subdivide_step2 = subdivide_step * subdivide_step;
            double subdivide_step3 = subdivide_step * subdivide_step * subdivide_step;

            double pre1 = 3.0 * subdivide_step;
            double pre2 = 3.0 * subdivide_step2;
            double pre4 = 6.0 * subdivide_step2;
            double pre5 = 6.0 * subdivide_step3;

            double tmp1x = x1 - x2 * 2.0 + x3;
            double tmp1y = y1 - y2 * 2.0 + y3;

            double tmp2x = (x2 - x3) * 3.0 - x1 + x4;
            double tmp2y = (y2 - y3) * 3.0 - y1 + y4;

            _saved_fx = _fx = x1;
            _saved_fy = _fy = y1;

            _saved_dfx = _dfx = (x2 - x1) * pre1 + tmp1x * pre2 + tmp2x * subdivide_step3;
            _saved_dfy = _dfy = (y2 - y1) * pre1 + tmp1y * pre2 + tmp2y * subdivide_step3;

            _saved_ddfx = _ddfx = tmp1x * pre4 + tmp2x * pre5;
            _saved_ddfy = _ddfy = tmp1y * pre4 + tmp2y * pre5;

            _dddfx = tmp2x * pre5;
            _dddfy = tmp2y * pre5;

            _step = _numberOfSteps;
        }

        /// <summary>
        /// Rewinds the specified path id.
        /// </summary>
        /// <param name="pathId">The path id.</param>
        public override void Rewind(uint pathId)
        {
            if (_numberOfSteps == 0)
            {
                _step = -1;
                return;
            }
            _step = _numberOfSteps;
            _fx = _saved_fx;
            _fy = _saved_fy;
            _dfx = _saved_dfx;
            _dfy = _saved_dfy;
            _ddfx = _saved_ddfx;
            _ddfy = _saved_ddfy;
        }

        /// <summary>
        /// Vertexes the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public override PathCommand Vertex(ref double x, ref double y)
        {
            if (_step < 0) 
                return PathCommand.PathStop;
            if (_step == _numberOfSteps)
            {
                x = _start_x;
                y = _start_y;
                --_step;
                return PathCommand.PathMoveTo;
            }

            if (_step == 0)
            {
                x = _end_x;
                y = _end_y;
                --_step;
                return PathCommand.PathLineTo;
            }

            _fx += _dfx;
            _fy += _dfy;
            _dfx += _ddfx;
            _dfy += _ddfy;
            _ddfx += _dddfx;
            _ddfy += _dddfy;

            x = _fx;
            y = _fy;
            --_step;
            return PathCommand.PathLineTo;
        }
    }
}