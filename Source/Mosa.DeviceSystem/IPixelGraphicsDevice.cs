// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a pixel graphics device
	/// </summary>
	public interface IPixelGraphicsDevice
	{
		/// <summary>Writes the pixel.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public void WritePixel(Color color, ushort x, ushort y);

		/// <summary>Reads the pixel.</summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public Color ReadPixel(ushort x, ushort y);

		/// <summary>Clears the specified color.</summary>
		/// <param name="color">The color.</param>
		public void Clear(Color color);

		/// <summary>Gets the width.</summary>
		public ushort Width { get; }

		/// <summary>Gets the height.</summary>
		public ushort Height { get; }
	}
}
