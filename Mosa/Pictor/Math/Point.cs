namespace Pictor.Math
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="BaseType">The type of the ase type.</typeparam>
    public class Point<BaseType>
    {
        /// <summary>
        /// 
        /// </summary>
        private BaseType _x;
        /// <summary>
        /// 
        /// </summary>
        private BaseType _y;

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>The x.</value>
        public BaseType x
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
        public BaseType y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point&lt;BaseType&gt;"/> class.
        /// </summary>
        /// <param name="__x">The __X.</param>
        /// <param name="__y">The __y.</param>
        public Point(BaseType __x, BaseType __y)
        {
            x = __x;
            y = __y;
        }
    }
}