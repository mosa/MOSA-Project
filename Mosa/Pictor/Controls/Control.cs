namespace Pictor.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Control
    {
        /// <summary>
        /// 
        /// </summary>
        protected double _x1;
        /// <summary>
        /// 
        /// </summary>
        protected double _y1;
        /// <summary>
        /// 
        /// </summary>
        protected double _x2;
        /// <summary>
        /// 
        /// </summary>
        protected double _y2;

        /// <summary>
        /// 
        /// </summary>
        private bool _flipY = false;
        /// <summary>
        /// 
        /// </summary>
        private Math.AffineTransformation _mtx = null;

        /// <summary>
        /// Transforms the XY.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void TransformXY(ref double x, ref double y)
        {
            if (_flipY) 
                y = _y1 + _y2 - y;
            if (_mtx != null) 
                _mtx.Transform(ref x, ref y);
        }

        /// <summary>
        /// Inverses the transform XY.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void InverseTransformXY(ref double x, ref double y)
        {
            if (_mtx != null) 
                _mtx.InverseTransform(ref x, ref y);
            if (_flipY) 
                y = _y1 + _y2 - y;
        }

        /// <summary>
        /// Determines whether [is in rect] [the specified x].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        /// 	<c>true</c> if [is in rect] [the specified x]; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsInRect(double x, double y);
        /// <summary>
        /// Called when [mouse button down].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public abstract bool OnMouseButtonDown(double x, double y);
        /// <summary>
        /// Called when [mouse button up].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public abstract bool OnMouseButtonUp(double x, double y);
        /// <summary>
        /// Called when [mouse move].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="button_flag">if set to <c>true</c> [button_flag].</param>
        /// <returns></returns>
        public abstract bool OnMouseMove(double x, double y, bool button_flag);
        /// <summary>
        /// Called when [arrow keys].
        /// </summary>
        /// <param name="left">if set to <c>true</c> [left].</param>
        /// <param name="right">if set to <c>true</c> [right].</param>
        /// <param name="down">if set to <c>true</c> [down].</param>
        /// <param name="up">if set to <c>true</c> [up].</param>
        /// <returns></returns>
        public abstract bool OnArrowKeys(bool left, bool right, bool down, bool up);
    }
}