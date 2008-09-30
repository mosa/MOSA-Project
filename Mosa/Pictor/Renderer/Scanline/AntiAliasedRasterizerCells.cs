namespace Pictor.Renderer.Scanline
{
    /// <summary>
    /// An internal class that implements the main rasterization algorithm.
    /// Used in the rasterizer. Should not be used direcly.
    /// </summary>
    /// <typeparam name="Cell">The type of the Cell.</typeparam>
    public class AntiAliasedRasterizerCells<Cell>
    {
        /// <summary>
        /// 
        /// </summary>
        enum CellBlockScale
        {
            /// <summary>
            /// 
            /// </summary>
            CellBlockShift = 12,
            /// <summary>
            /// 
            /// </summary>
            CellBlockSize = 1 << CellBlockShift,
            /// <summary>
            /// 
            /// </summary>
            CellBlockMask = CellBlockSize - 1,
            /// <summary>
            /// 
            /// </summary>
            CellBlockPool = 256,
            /// <summary>
            /// 
            /// </summary>
            CellBlockLimit = 1024
        };

        struct SortedY
        {
            /// <summary>
            /// 
            /// </summary>
            public uint start;
            /// <summary>
            /// 
            /// </summary>
            public uint num;

            /// <summary>
            /// Initializes a new instance of the <see cref="AntiAliasedRasterizerCells&lt;Cell&gt;.SortedY"/> struct.
            /// </summary>
            /// <param name="s">The s.</param>
            /// <param name="n">The n.</param>
            public SortedY(uint s, uint n)
            {
                start = s;
                num = n;
            }
        };
    }
}