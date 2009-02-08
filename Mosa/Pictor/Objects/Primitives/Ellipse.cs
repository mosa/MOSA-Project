/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

namespace Pictor.Objects.Primitives
{
    /// <summary>
    /// 
    /// </summary>
    public class Ellipse : Primitive
    {
        /// <summary>
        /// 
        /// </summary>
        private double _x;
        /// <summary>
        /// 
        /// </summary>
        private double _y;
        /// <summary>
        /// 
        /// </summary>
        private double _rx;
        /// <summary>
        /// 
        /// </summary>
        private double _ry;
        /// <summary>
        /// 
        /// </summary>
        private uint _num;
        /// <summary>
        /// 
        /// </summary>
        private uint _step;
        /// <summary>
        /// 
        /// </summary>
        private bool _cw;

        /// <summary>
        /// Inits the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="rx">The rx.</param>
        /// <param name="ry">The ry.</param>
        public void Init(double x, double y, double rx, double ry)
        {
            Init(x, y, rx, ry, 0, false);
        }

        /// <summary>
        /// Inits the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="rx">The rx.</param>
        /// <param name="ry">The ry.</param>
        /// <param name="num_steps">The num_steps.</param>
        public void Init(double x, double y, double rx, double ry,
                  uint num_steps)
        {
            Init(x, y, rx, ry, num_steps, false);
        }

        /// <summary>
        /// Inits the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="rx">The rx.</param>
        /// <param name="ry">The ry.</param>
        /// <param name="num_steps">The num_steps.</param>
        /// <param name="cw">if set to <c>true</c> [cw].</param>
        public void Init(double x, double y, double rx, double ry,
                  uint num_steps, bool cw)
        {
            _x = x;
            _y = y;
            _rx = rx;
            _ry = ry;
            _num = num_steps;
            _step = 0;
            _cw = cw;
            if (_num == 0) 
                CalculateNumberOfSteps();
        }

        /// <summary>
        /// Rewinds the specified path_id.
        /// </summary>
        /// <param name="path_id">The path_id.</param>
        public override void Rewind(uint path_id)
        {
            _step = 0;
        }

        /// <summary>
        /// Vertexes the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public override PathCommand Vertex(ref double x, ref double y)
        {
            if (_step == _num)
            {
                ++_step;
                return PathCommand.PathEndPolygon;// | path_flags_close | path_flags_ccw;
            }
            if (_step > _num)
                return PathCommand.PathStop;
            double angle = (double)(_step) / (double)(_num) * 2.0 * System.Math.PI;
            if (_cw)
                angle = 2.0 * System.Math.PI - angle;
            x = _x + System.Math.Cos(angle) * _rx;
            y = _y + System.Math.Sin(angle) * _ry;
            _step++;
            return ((_step == 1) ? PathCommand.PathMoveTo : PathCommand.PathLineTo);
        }

        /// <summary>
        /// Calculates the number of steps.
        /// </summary>
        private void CalculateNumberOfSteps()
        {
            double ra = (System.Math.Abs(_rx) + System.Math.Abs(_ry)) / 2.0;
            double da = System.Math.Acos(ra / (ra + 0.125 / ApproximationScale)) * 2;
            _num = (uint)System.Math.Round(2 * System.Math.PI / da);
        }
    }
}