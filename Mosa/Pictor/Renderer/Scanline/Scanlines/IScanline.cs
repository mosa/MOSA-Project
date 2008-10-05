using System;
using System.Collections.Generic;

namespace Pictor.Renderer.Scanline.Scanlines
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="BaseType">The type of the ase type.</typeparam>
    public class ISpan<BaseType>
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseType x;
        /// <summary>
        /// 
        /// </summary>
        public BaseType length;
        /// <summary>
        /// 
        /// </summary>
        public byte cover;
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IScanline
    {
        /// <summary>
        /// Resets the scanline
        /// </summary>
        /// <param name="minX">Mininum to begin at</param>
        /// <param name="maxX">Maximum to end at</param>
        void Reset(int minX, int maxX);
        /// <summary>
        /// Adds a new cell to the scanline
        /// </summary>
        /// <param name="x">Position to start at</param>
        /// <param name="cell">The ?.</param>
        void AddCell(int x, uint cell);
        /// <summary>
        /// Adds a span to the scanline.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="length">The length.</param>
        /// <param name="span">The ?.</param>
        void AddSpan(int x, uint length, uint span);
        /// <summary>
        /// Adds the cells.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="length">The length.</param>
        /// <param name="cells">The cells.</param>
        void AddCells(int x, uint length, object[] cells);
        /// <summary>
        /// Finalizes the specified y.
        /// </summary>
        /// <param name="y">The y.</param>
        void Finalize(int y);
        /// <summary>
        /// Resets all spans.
        /// </summary>
        void ResetSpans();
        
        /// <summary>
        /// Gets the Y.
        /// </summary>
        /// <value>The Y.</value>
        int Y
        {
            get;
        }

        /// <summary>
        /// Gets the number of spans.
        /// </summary>
        uint NumberOfSpans
        {
            get;
        }

        /// <summary>
        /// Gets the span.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        ISpan<BaseType> GetSpan<BaseType>(int index);
    }
}
