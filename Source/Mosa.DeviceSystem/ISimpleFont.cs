// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public interface ISimpleFont
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the size.
		/// </summary>
		int Size { get; }

		/// <summary>
		/// Draws the string.
		/// </summary>
		void DrawString(FrameBuffer32 frameBuffer, uint color, uint x, uint y, string text);
	}
}
