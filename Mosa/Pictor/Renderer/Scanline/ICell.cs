namespace Pictor.Renderer.Scanline
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>The X.</value>
        int X
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        int Y
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cover.
        /// </summary>
        /// <value>The cover.</value>
        int Cover
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        /// <value>The area.</value>
        int Area
        {
            get;
            set;
        }
    }
}
