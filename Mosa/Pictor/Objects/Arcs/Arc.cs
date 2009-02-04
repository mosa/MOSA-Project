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
    /// 
    /// </summary>
    public class Arc : Primitive
    {
        /// <summary>
        /// 
        /// </summary>
        private double _x  = 0.0;
        /// <summary>
        /// 
        /// </summary>
        private double _y  = 0.0;
        /// <summary>
        /// 
        /// </summary>
        private double _rx = 0.0;
        /// <summary>
        /// 
        /// </summary>
        private double _ry = 0.0;
        /// <summary>
        /// 
        /// </summary>
        private double _start = 0.0;
        /// <summary>
        /// 
        /// </summary>
        private double _end = 0.0;
        /// <summary>
        /// 
        /// </summary>
        private double _da = 0.0;
        /// <summary>
        /// 
        /// </summary>
        private double _scale = 0.0;
        /// <summary>
        /// 
        /// </summary>
        private double _angle = 0.0;
        /// <summary>
        /// 
        /// </summary>
        private PathCommand _pathCommand;
        /// <summary>
        /// 
        /// </summary>
        private bool _counterClockWise = false;
        /// <summary>
        /// 
        /// </summary>
        private bool _initialized = false;

        
        /// <summary>
        /// Gets or sets the approximation scale.
        /// </summary>
        /// <value>The approximation scale.</value>
        public override double ApproximationScale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
                if (_initialized)
                {
                    Normalize(_start, _end, _counterClockWise);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Arc"/> class.
        /// </summary>
        public Arc()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Arc"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="rx">The rx.</param>
        /// <param name="ry">The ry.</param>
        /// <param name="a1">The a1.</param>
        /// <param name="a2">The a2.</param>
        /// <param name="ccw">if set to <c>true</c> [CCW].</param>
        public Arc(double x, double y, double rx, double ry, double a1, double a2, bool ccw)
        {
            _x = x;
            _y = y;
            _rx = rx;
            _ry = ry;
            _counterClockWise = ccw;
            _scale = 1.0;
            Normalize(a1, a2, ccw);
        }

        /// <summary>
        /// Vertexes the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public override PathCommand Vertex(ref double x, ref double y)
        {
            if(PathCommand.PathStop == _pathCommand)
                return PathCommand.PathStop;

            if((_angle < _end - _da / 4.0) != _counterClockWise)
            {
                x = _x + System.Math.Cos(_end) * _rx;
                y = _y + System.Math.Sin(_end) * _ry;
                _pathCommand = PathCommand.PathStop;
                return PathCommand.PathLineTo;
            }

            x = _x + System.Math.Cos(_angle) * _rx;
            y = _y + System.Math.Sin(_angle) * _ry;

            _angle += _da;

            PathCommand pf = _pathCommand;
            _pathCommand = PathCommand.PathLineTo;

            return pf;
        }

        /// <summary>
        /// Rewinds the specified x.
        /// </summary>
        /// <param name="foo">The x.</param>
        public override void Rewind(uint foo)
        {
            _pathCommand = PathCommand.PathMoveTo;
            _angle = _start;
        }

        /// <summary>
        /// Normalizes the specified a1.
        /// </summary>
        /// <param name="a1">The a1.</param>
        /// <param name="a2">The a2.</param>
        /// <param name="ccw">if set to <c>true</c> [CCW].</param>
        private void Normalize(double a1, double a2, bool ccw)
        {
            double ra = (System.Math.Abs(_rx) + System.Math.Abs(_ry)) / 2.0;

            _da = System.Math.Acos(ra / (ra + 0.125 / _scale)) * 2;
            if (ccw)
            {
                while (a2 < a1) 
                    a2 += System.Math.PI * 2.0;
            }
            else
            {
                while (a1 < a2)
                    a1 += System.Math.PI * 2.0;
                _da = -_da;
            }
            _counterClockWise = ccw;
            _start = a1;
            _end = a2;
            _initialized = true;
        }
    }
}