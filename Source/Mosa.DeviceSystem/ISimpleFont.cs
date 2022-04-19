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
		/// Gets the width.
		/// </summary>
		int Width { get; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		int Height { get; }

		/// <summary>
		/// Draws the string.
		/// </summary>
		void DrawString(FrameBuffer32 frameBuffer, uint color, uint x, uint y, string text);

		/// <summary>
		/// Calculates the width of a character.
		/// </summary>
		int CalculateWidth(char c);

		/// <summary>
		/// Calculates the width of a string.
		/// </summary>
		int CalculateWidth(string s);
	}
}
