/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

namespace Pictor.Renderer.Scanline
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="PixelFormat">The type of the ixel format.</typeparam>
    /// <typeparam name="ColorType">The type of the olor type.</typeparam>
    public class ScanlineRenderer<PixelFormat, ColorType> : BaseRenderer<PixelFormat, ColorType> where ColorType : ColorTypes.IColorType, new()
                                                                                                 where PixelFormat : PixelFormats.BasePixelFormat<ColorType>
    {
        /// <summary>
        /// 
        /// </summary>
        private ColorType color = new ColorType();

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public ColorType Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        /// <summary>
        /// Renders the specified scanline.
        /// </summary>
        /// <param name="scanline">The scanline.</param>
        public void Render(Scanlines.IScanline scanline)
        {
            RenderScanlineAASolid(scanline, color);
        }

        /// <summary>
        /// Renders the scanline AA solid.
        /// </summary>
        /// <param name="scanline">The scanline.</param>
        /// <param name="color">The color.</param>
        private void RenderScanlineAASolid(Scanlines.IScanline scanline, ColorType color)
        {
            int y = scanline.Y;
            uint num_spans = scanline.NumberOfSpans;
            for (int i = 0; i < num_spans; ++i)
            {
                Scanlines.ISpan<uint> span = scanline.GetSpan<uint>(i);
                int x = (int)span.x;
                /*if (span.length > 0)
                {
                    /*BlendSolidHSpan(x, y, (uint)span.length,
                                        color,
                                        span.cover);*
                }
                else
                {*/
                    BlendHLine(x, y, (int)(x - span.length - 1), color, span.cover);
                //}
                if (--num_spans == 0) 
                    break;
            }
        }
    }
}