namespace Pictor.Math
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="BaseType">The type of the ase type.</typeparam>
    public class Rect<BaseType> where BaseType : System.IComparable
    {
        /// <summary>
        /// 
        /// </summary>
        private BaseType _x1;
        /// <summary>
        /// 
        /// </summary>
        private BaseType _y1;
        /// <summary>
        /// 
        /// </summary>
        private BaseType _x2;
        /// <summary>
        /// 
        /// </summary>
        private BaseType _y2;

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>The left.</value>
        public BaseType Left
        {
            get
            {
                return _x1;
            }
            set
            {
                _x1 = value;
            }
        }
        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>The top.</value>
        public BaseType Top
        {
            get
            {
                return _y1;
            }
            set
            {
                _y1 = value;
            }
        }

        /// <summary>
        /// Gets or sets the right.
        /// </summary>
        /// <value>The right.</value>
        public BaseType Right
        {
            get
            {
                return _x2;
            }
            set
            {
                _x2 = value;
            }
        }

        /// <summary>
        /// Gets or sets the bottom.
        /// </summary>
        /// <value>The bottom.</value>
        public BaseType Bottom
        {
            get
            {
                return _y2;
            }
            set
            {
                _y2 = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect&lt;BaseType&gt;"/> class.
        /// </summary>
        public Rect()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect&lt;BaseType&gt;"/> class.
        /// </summary>
        /// <param name="x1_">The X1_.</param>
        /// <param name="y1_">The y1_.</param>
        /// <param name="x2_">The X2_.</param>
        /// <param name="y2_">The y2_.</param>
        public Rect(BaseType x1_, BaseType y1_, BaseType x2_, BaseType y2_)
        {
            Left = x1_;
            Top = y1_;
            Right = x2_;
            Bottom = y2_;
        }

        /// <summary>
        /// Inits the specified X1_.
        /// </summary>
        /// <param name="x1_">The X1_.</param>
        /// <param name="y1_">The y1_.</param>
        /// <param name="x2_">The X2_.</param>
        /// <param name="y2_">The y2_.</param>
        void Init(BaseType x1_, BaseType y1_, BaseType x2_, BaseType y2_)
        {
            Left   = x1_; 
            Top    = y1_; 
            Right  = x2_; 
            Bottom = y2_;
        }

        /// <summary>
        /// Normalizes this instance.
        /// </summary>
        /// <returns></returns>
        public Rect<BaseType> Normalize()
        {
            BaseType t;
            if (Left.CompareTo(Right) > 0)
            {
                t = Left;
                Left = Right;
                Right = t;
            }
            if (Top.CompareTo(Bottom) > 0)
            {
                t = Top;
                Top = Bottom;
                Bottom = t;
            }
            return this;
        }

        /// <summary>
        /// Clips the specified r.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        public bool Clip(Rect<BaseType> r)
        {
            if (Right.CompareTo(r.Right) > 0)
                Right = r.Right;
            if (Bottom.CompareTo(r.Bottom) > 0)
                Bottom = r.Bottom;
            if (Left.CompareTo(r.Left) < 0)
                Left = r.Left;
            if (Top.CompareTo(r.Top) < 0)
                Top = r.Top;
            return (Left.CompareTo(Right) <= 0) && (Top.CompareTo(Bottom) <= 0);
        }

        /// <summary>
        /// Is_valids this instance.
        /// </summary>
        /// <returns></returns>
        public bool IsValid
        {
            get
            {
                return (Left.CompareTo(Right) <= 0) && (Top.CompareTo(Bottom) <= 0);
            }
        }

        /// <summary>
        /// Hits the test.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool HitTest(BaseType x, BaseType y)
        {
            return (x.CompareTo(Left) >= 0) && (x.CompareTo(Right) <= 0) && (y.CompareTo(Top) >= 0) && (y.CompareTo(Bottom) <= 0);
        }
    }
}