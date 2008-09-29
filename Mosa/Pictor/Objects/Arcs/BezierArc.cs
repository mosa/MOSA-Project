/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

namespace Pictor.Objects.Arcs
{
    /// <summary>
    /// 
    /// </summary>
    public class BezierArc : Arc
    {
        /// <summary>
        /// 
        /// </summary>
        private uint _vertex = 26;
        /// <summary>
        /// 
        /// </summary>
        private uint _numberOfVertices = 0;
        /// <summary>
        /// 
        /// </summary>
        protected double[] _vertices = new double[26];
        /// <summary>
        /// 
        /// </summary>
        private PathCommand _pathCommand = PathCommand.PathLineTo;
        /// <summary>
        /// 
        /// </summary>
        public const double BezierArcAngleEpsilon = 0.01;

        /// <summary>
        /// Gets the vertices.
        /// </summary>
        /// <value>The vertices.</value>
        public double[] Vertices
        {
            get
            {
                return _vertices;
            }
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        /// <value>The number of vertices.</value>
        public uint NumberOfVertices
        {
            get
            {
                return _numberOfVertices;
            }
        }

        /// <summary>
        /// Inits the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="rx">The rx.</param>
        /// <param name="ry">The ry.</param>
        /// <param name="startAngle">The startAngle.</param>
        /// <param name="sweepAngle">The sweepAngle.</param>
        public void Init(double x, double y,
                  double rx, double ry,
                  double startAngle,
                  double sweepAngle)
        {
            startAngle = startAngle % (2.0 * System.Math.PI);
            if (sweepAngle >= 2.0 * System.Math.PI) sweepAngle = 2.0 * System.Math.PI;
            if (sweepAngle <= -2.0 * System.Math.PI) sweepAngle = -2.0 * System.Math.PI;

            if (System.Math.Abs(sweepAngle) < 1e-10)
            {
                _numberOfVertices = 4;
                _pathCommand = PathCommand.PathLineTo;
                _vertices[0] = x + rx * System.Math.Cos(startAngle);
                _vertices[1] = y + ry * System.Math.Sin(startAngle);
                _vertices[2] = x + rx * System.Math.Cos(startAngle + sweepAngle);
                _vertices[3] = y + ry * System.Math.Sin(startAngle + sweepAngle);
                return;
            }

            double totalSweep = 0.0;
            double localSweep = 0.0;
            double prevSweep;
            _numberOfVertices = 2;
            _pathCommand = PathCommand.PathCurve4;
            bool done = false;

            do
            {
                if (sweepAngle < 0.0)
                {
                    prevSweep = totalSweep;
                    localSweep = -System.Math.PI * 0.5;
                    totalSweep -= System.Math.PI * 0.5;

                    if (totalSweep <= sweepAngle + BezierArcAngleEpsilon)
                    {
                        localSweep = sweepAngle - prevSweep;
                        done = true;
                    }
                }
                else
                {
                    prevSweep = totalSweep;
                    localSweep = System.Math.PI * 0.5;
                    totalSweep += System.Math.PI * 0.5;
                    if (totalSweep >= sweepAngle - BezierArcAngleEpsilon)
                    {
                        localSweep = sweepAngle - prevSweep;
                        done = true;
                    }
                }

                ArcToBezier(x, y, rx, ry, startAngle, localSweep, ref _vertices, NumberOfVertices - 2);

                _numberOfVertices += 6;
                startAngle += localSweep;
            }
            while (!done && _numberOfVertices < 26);
        }

        /// <summary>
        /// Rewinds the specified foo.
        /// </summary>
        /// <param name="foo">The foo.</param>
        public override void Rewind(uint foo)
        {
            _vertex = 0;
        }

        /// <summary>
        /// Vertexes the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public override PathCommand Vertex(ref double x, ref double y)
        {
            if (_vertex >= _numberOfVertices)
                return PathCommand.PathStop;
            x = Vertices[_vertex];
            y = Vertices[_vertex + 1];
            _vertex += 2;
            return (_vertex == 2) ? PathCommand.PathMoveTo : _pathCommand;
        }

        /// <summary>
        /// Arcs to bezier.
        /// </summary>
        /// <param name="cx">The cx.</param>
        /// <param name="cy">The cy.</param>
        /// <param name="rx">The rx.</param>
        /// <param name="ry">The ry.</param>
        /// <param name="startAngle">The startAngle.</param>
        /// <param name="sweepAngle">The sweepAngle.</param>
        /// <param name="curve">The curve.</param>
        /// <param name="offset">The offset.</param>
        private void ArcToBezier(double cx, double cy, double rx, double ry,
                                 double startAngle, double sweepAngle,
                                 ref double[] curve, uint offset)
        {
            double x0 = System.Math.Cos(sweepAngle / 2.0);
            double y0 = System.Math.Sin(sweepAngle / 2.0);
            double tx = (1.0 - x0) * 4.0 / 3.0;
            double ty = y0 - tx * x0 / y0;
            double[] px = { x0, x0 + tx, x0 + tx, x0 };
            double[] py = { -y0, -ty, ty, y0 };

            double sn = System.Math.Sin(startAngle + sweepAngle / 2.0);
            double cs = System.Math.Cos(startAngle + sweepAngle / 2.0);

            for (int i = 0; i < 4; ++i)
            {
                curve[i * 2 + offset]     = cx + rx * (px[i] * cs - py[i] * sn);
                curve[i * 2 + offset + 1] = cy + ry * (px[i] * sn + py[i] * cs);
            }
        }
    }
}