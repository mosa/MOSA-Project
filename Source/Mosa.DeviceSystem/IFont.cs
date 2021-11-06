// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public interface IFont
	{
		/// <summary>Gets the name.</summary>
		/// <value>The name.</value>
		string Name { get; }

		/// <summary>Gets the size.</summary>
		/// <value>The size.</value>
		int Size { get; }

		/// <summary>Draws the string.</summary>
		void DrawString(IFrameBuffer frameBuffer, uint color, uint x, uint y, string text);
	}
}
