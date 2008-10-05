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
    public class Curve3 : Curve
    {
        /// <summary>
        /// 
        /// </summary>
        private Curve3Inc _curveInc = new Curve3Inc();
        /// <summary>
        /// 
        /// </summary>
        private Curve3Div _curveDiv = new Curve3Div();
        /// <summary>
        /// 
        /// </summary>
        private Curve _currentCurve;
        /// <summary>
        /// 
        /// </summary>
        private CurveApproximationMethod _approximationMethod = CurveApproximationMethod.CurveDiv;

        /// <summary>
        /// Gets or sets the approximation method.
        /// </summary>
        /// <value>The approximation method.</value>
        public new CurveApproximationMethod ApproximationMethod
        {
            get
            {
                return _approximationMethod;
            }
            set
            {
                _approximationMethod = value;

                if (CurveApproximationMethod.CurveDiv == ApproximationMethod)
                    _currentCurve = _curveDiv;
                else if (CurveApproximationMethod.CurveInc == ApproximationMethod)
                    _currentCurve = _curveInc;
            }
        }

        /// <summary>
        /// Rewinds the specified path id.
        /// </summary>
        /// <param name="pathId">The path id.</param>
        public override void Rewind(uint pathId)
        {
            _currentCurve.Rewind(pathId);
        }

        /// <summary>
        /// Vertexes the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public override PathCommand Vertex(ref double x, ref double y)
        {
            return _currentCurve.Vertex(ref x, ref y);
        }
    }
}