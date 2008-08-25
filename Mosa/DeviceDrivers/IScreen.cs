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
	public interface IScreen
	{
		void Clear();
		void SetCursor(ushort cursorX, ushort cursorY);
		void Write(string text);
		void WriteLine();
		void WriteLine(string text);
	}
}
