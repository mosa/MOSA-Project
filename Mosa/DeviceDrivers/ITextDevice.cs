/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers
{
    public enum TextColor : byte
    {
        Black,
        Blue,
        Green,
        Cyan,
        Red,
        Magenta,
        Brown,
        White,
        DarkGray,
        LightBlue,
        LightGreen,
        LightCyan,
        LightRed,
        LightMagenta,
        Yellow,
        BrightWhite
    }

    public interface ITextDevice
    {
        void WriteChar(ushort x, ushort y, char c, TextColor foreground, TextColor background);
        void SetCursor(ushort x, ushort y);
    }
}
