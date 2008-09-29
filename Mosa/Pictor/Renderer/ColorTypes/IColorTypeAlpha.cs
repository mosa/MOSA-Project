namespace Pictor.Renderer.ColorTypes
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="BaseType">The type of the ase type.</typeparam>
    public interface IColorTypeAlpha<BaseType> : IColorType
    {
        /// <summary>
        /// Gets or sets the alpha.
        /// </summary>
        /// <value>The alpha.</value>
        BaseType Alpha
        {
            get;
            set;
        }
    }
}