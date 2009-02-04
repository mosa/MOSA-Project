/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@mosa-project.org>)
 */

using System.Collections.Generic;

namespace Pictor.Objects.Splines
{
    /// <summary>
    /// A very simple class of Bi-cubic Spline interpolation.
    /// First call init(num, x[], y[]) where num - number of source points,
    /// x, y - arrays of X and Y values respectively. Here Y must be a function
    /// of X. It means that all the X-coordinates must be arranged in the ascending
    /// order.
    /// Then call get(x) that calculates a value Y for the respective X.
    /// The class supports extrapolation, i.e. you can call get(x) where x is
    /// outside the given with init() X-range. Extrapolation is a simple linear
    /// function.
    /// </summary>
    public class BSpline : Primitive
    {
        /// <summary>
        /// 
        /// </summary>
        private int             _max = 0;
        /// <summary>
        /// 
        /// </summary>
        private int             _num = 0;
        /// <summary>
        /// 
        /// </summary>
        private double[]        _x = null;
        /// <summary>
        /// 
        /// </summary>
        private double[]        _y = null;
        /// <summary>
        /// 
        /// </summary>
        private List<double>    _am = new List<double>();
        /// <summary>
        /// 
        /// </summary>
        private int             _lastIdx = -1;

        struct SplinePoint
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SplinePoint"/> struct.
            /// </summary>
            /// <param name="_x">The _x.</param>
            /// <param name="_y">The _y.</param>
            public SplinePoint(double _x, double _y)
            {
                x = _x;
                y = _y;
            }

            /// <summary>
            /// 
            /// </summary>
            double x;
            /// <summary>
            /// 
            /// </summary>
            double y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BSpline"/> class.
        /// </summary>
        public BSpline()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BSpline"/> class.
        /// </summary>
        /// <param name="num">The num.</param>
        public BSpline(int num)
        {
            Init(num);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BSpline"/> class.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public BSpline(int num, double[] x, double[] y)
        {
            Init(num, x, y);
        }

        /// <summary>
        /// Inits the specified num.
        /// </summary>
        /// <param name="max">The max.</param>
        public void Init(int max)
        {
            if (max > 2 && max > _max)
            {
                for (int i = 0; i < 3 * max; ++i)
                    _am.Add(0.0);
                _max = max;
            }
            _num = 0;
            _lastIdx = -1;
        }

        /// <summary>
        /// Adds the point.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void AddPoint(double x, double y)
        {
            if (_num < _max)
            {
                _am[_max     + _num] = x;
                _am[_max * 2 + _num] = y;
                ++_num;
            }
        }

        /// <summary>
        /// Prepares this instance.
        /// </summary>
        public void Prepare()
        {
            if(_num > 2)
            {
                int i, k, n1;
                double h, p, d, f, e;
        
                for(k = 0; k < _num; k++) 
                {
                    _am[k] = 0.0;
                }

                n1 = 3 * _num;

                List<double> al = new List<double>(n1);

                for(k = 0; k < n1; k++) 
                {
                    al.Add(0.0);
                }

                n1 = _num - 1;
                d = _am[_max + 1] - _am[_max + 0];
                e = (_am[_max * 2 + 1] - _am[_max * 2 + 0]) / d;

                for(k = 1; k < n1; k++) 
                {
                    h     = d;
                    d = _am[_max + k + 1] - _am[_max + k];
                    f     = e;
                    e = (_am[_max * 2 + k + 1] - _am[_max * 2 + k]) / d;
                    al[k] = d / (d + h);
                    al[_num + k] = 1.0 - al[k];
                    al[_num * 2 + k] = 6.0 * (e - f) / (h + d);
                }

                for(k = 1; k < n1; k++) 
                {
                    p = 1.0 / (al[_num + k] * al[k - 1] + 2.0);
                    al[k] *= -p;
                    al[_num * 2 + k] = (al[_num * 2 + k] - al[_num + k] * al[_num * 2 + (k - 1)]) * p; 
                }

                _am[n1]     = 0.0;
                al[n1 - 1]  = al[_num * 2 + (n1 - 1)];
                _am[n1 - 1] = al[n1 - 1];

                for(k = n1 - 2, i = 0; i < _num - 2; i++, k--) 
                {
                    al[k] = al[k] * al[k + 1] + al[_num * 2 + k];
                    _am[k] = al[k];
                }
            }
            _lastIdx = -1;
        }

        /// <summary>
        /// Inits the specified num.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void Init(int num, double[] x, double[] y)
        {
            if (num > 2)
            {
                Init(num);
                for (int i = 0; i < num; i++)
                {
                    AddPoint(x[i], y[i]);
                }
                Prepare();
            }
            _lastIdx = -1;
        }

        /// <summary>
        /// Vertexes the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public override PathCommand Vertex(ref double x, ref double y)
        {
            return PathCommand.PathStop;
        }

        /// <summary>
        /// Rewinds the specified foo.
        /// </summary>
        /// <param name="foo">The foo.</param>
        public override void Rewind(uint foo)
        {
        }

        /// <summary>
        /// Gets the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public double Get(double x) 
        {
            if (_num > 2)
            {
                int i = 0;

                // Extrapolation on the left
                if (x < _am[_max]) 
                    return ExtrapolationLeft(x);

                // Extrapolation on the right
                if (x >= _am[_max + (_num - 1)]) 
                    return ExtrapolationRight(x);

                // Interpolation
                BSearch(_num, _am.GetRange(_max, _am.Count - _max).ToArray(), x, ref i);
                return Interpolation(x, i);
            }
            return 0.0;
        }

        /// <summary>
        /// Gets the stateful.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public double GetStateful(double x)
        {
            if (_num > 2)
            {
                // Extrapolation on the left
                if (x < _am[_max]) 
                    return ExtrapolationLeft(x);

                // Extrapolation on the right
                if (x >= _am[_max + (_num - 1)]) return ExtrapolationRight(x);

                if (_lastIdx >= 0)
                {
                    // Check if x is not in current range
                    if (x < _x[_lastIdx] || x > _x[_lastIdx + 1])
                    {
                        // Check if x between next points (most probably)
                        if (_lastIdx < _num - 2 &&
                           x >= _am[_max + (_lastIdx + 1)] &&
                           x <= _am[_max + (_lastIdx + 2)])
                        {
                            ++_lastIdx;
                        }
                        else
                            if (_lastIdx > 0 &&
                               x >= _am[_max + (_lastIdx - 1)] &&
                               x <= _am[_max + _lastIdx])
                            {
                                // x is between pevious points
                                --_lastIdx;
                            }
                            else
                            {
                                // Else perform full search
                                BSearch(_num, _am.GetRange(_max, _am.Count - _max).ToArray(), x, ref _lastIdx);
                            }
                    }
                    return Interpolation(x, _lastIdx);
                }
                else
                {
                    // Interpolation
                    BSearch(_num, _am.GetRange(_max, _am.Count - _max).ToArray(), x, ref _lastIdx);
                    return Interpolation(x, _lastIdx);
                }
            }
            return 0.0;
        }

        /// <summary>
        /// Bsearches the specified n.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <param name="x">The x.</param>
        /// <param name="x0">The x0.</param>
        /// <param name="i">The i.</param>
        private static void BSearch(int n, double[] x, double x0, ref int i)
        {
            int j = n - 1;
            int k;

            for (i = 0; (j - i) > 1; )
            {
                if (x0 < x[k = (i + j) >> 1]) 
                    j = k;
                else 
                    i = k;
            }
        }

        /// <summary>
        /// Extrapolation_lefts the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        private double ExtrapolationLeft(double x) 
        {
            double d = _am[_max + 1] - _am[_max];
            return (-d * _am[1] / 6 + (_y[1] - _y[0]) / d) *
                   (x - _am[_max]) +
                   _y[0];
        }

        /// <summary>
        /// Extrapolation_rights the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        private double ExtrapolationRight(double x) 
        {
            double d = _am[_max + (_num - 1)] - _am[_max + (_num - 2)];
            return (d * _am[_num - 2] / 6 + (_am[_max * 2 + _num - 1] - _am[_max * 2 + _num - 2]) / d) *
                   (x - _am[_max + (_num - 1)]) +
                   _am[_max * 2 + (_num - 1)];
        }

        /// <summary>
        /// Interpolations the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        private double Interpolation(double x, int i)
        {
            int j = i + 1;
            double d = _am[_max + i] - _am[_max + j];
            double h = x - _am[_max + j];
            double r = _am[_max + i] - x;
            double p = d * d / 6.0;
            return (_am[j] * r * r * r + _am[i] * h * h * h) / 6.0 / d +
                   ((_am[_max * 2 + j] - _am[j] * p) * r + (_am[_max * 2 + i] - _am[i] * p) * h) / d;
        }
    }
}