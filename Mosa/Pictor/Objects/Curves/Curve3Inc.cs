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
    public class Curve3Inc : Curve
    {
        /// <summary>
        /// 
        /// </summary>
        private int _numberOfSteps = 0;
        /// <summary>
        /// 
        /// </summary>
        private int _step = 0;
        /// <summary>
        /// 
        /// </summary>
        private double _startX;
        /// <summary>
        /// 
        /// </summary>
        private double _startY;
        /// <summary>
        /// 
        /// </summary>
        private double _endX;
        /// <summary>
        /// 
        /// </summary>
        private double _endY;
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
        private double _savedFx;
        /// <summary>
        /// 
        /// </summary>
        private double _savedFy;
        /// <summary>
        /// 
        /// </summary>
        private double _savedDfx;
        /// <summary>
        /// 
        /// </summary>
        private double _savedDfy;

        /// <summary>
        /// Gets or sets the approximation method.
        /// </summary>
        /// <value>The approximation method.</value>
        public new CurveApproximationMethod ApproximationMethod
        {
            get
            {
                return CurveApproximationMethod.CurveInc;
            }
            set
            {
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Curve3Inc"/> class.
        /// </summary>
        public Curve3Inc()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Curve3Inc"/> class.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="x3">The x3.</param>
        /// <param name="y3">The y3.</param>
        public Curve3Inc(double x1, double y1, 
                         double x2, double y2, 
                         double x3, double y3)
        { 
            Init(x1, y1, x2, y2, x3, y3);
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
            _startX = x1;
            _startY = y1;
            _endX   = x3;
            _endY   = y3;

            double dx1 = x2 - x1;
            double dy1 = y2 - y1;
            double dx2 = x3 - x2;
            double dy2 = y3 - y2;

            double len = System.Math.Sqrt(dx1 * dx1 + dy1 * dy1) + System.Math.Sqrt(dx2 * dx2 + dy2 * dy2);

            _numberOfSteps = (int)System.Math.Round(len * 0.25 * ApproximationScale);

            if (_numberOfSteps < 4)
            {
                _numberOfSteps = 4;
            }

            double subdivide_step = 1.0 / _numberOfSteps;
            double subdivide_step2 = subdivide_step * subdivide_step;

            double tmpx = (x1 - x2 * 2.0 + x3) * subdivide_step2;
            double tmpy = (y1 - y2 * 2.0 + y3) * subdivide_step2;

            _savedFx = _fx = x1;
            _savedFy = _fy = y1;

            _savedDfx = _dfx = tmpx + (x2 - x1) * (2.0 * subdivide_step);
            _savedDfy = _dfy = tmpy + (y2 - y1) * (2.0 * subdivide_step);

            _ddfx = tmpx * 2.0;
            _ddfy = tmpy * 2.0;

            _step = _numberOfSteps;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset() 
        { 
            _numberOfSteps = 0; 
            _step = -1; 
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
            _fx   = _savedFx;
            _fy   = _savedFy;
            _dfx  = _savedDfx;
            _dfy  = _savedDfy;
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
                x = _startX;
                y = _startY;
                --_step;
                return PathCommand.PathMoveTo;
            }
            if (_step == 0)
            {
                x = _endX;
                y = _endY;
                --_step;
                return PathCommand.PathLineTo;
            }

            _fx += _dfx;
            _fy += _dfy;
            _dfx += _ddfx;
            _dfy += _ddfy;
            x = _fx;
            y = _fy;
            --_step;

            return PathCommand.PathLineTo;
        }

    }
}