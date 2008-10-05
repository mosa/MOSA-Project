namespace Pictor.Renderer.Scanline
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="CoordinateType">The type of the oordinate type.</typeparam>
    /// <typeparam name="CellType">The type of the ell type.</typeparam>
    public interface IClipper<CoordinateType, CellType> where CellType : ICell
    {
        /// <summary>
        /// Lines to.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <param name="startx">The startx.</param>
        /// <param name="starty">The starty.</param>
        void LineTo(AntiAliasedRasterizerCells<CellType> cell, CoordinateType startx, CoordinateType starty);
    }
}
