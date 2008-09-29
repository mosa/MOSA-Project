/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

namespace Pictor.Renderer.Scanline
{
    /// <summary>
    /// 
    /// </summary>
    public class ScanlineRenderer<PixelFormat, ColorType> : BaseRenderer<PixelFormat, ColorType> where ColorType : ColorTypes.IColorType
                                                                                                 where PixelFormat : PixelFormats.BasePixelFormat<ColorType>
    {

    }
}