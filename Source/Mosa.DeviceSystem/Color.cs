// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Color
	/// </summary>
	public struct Color
	{
		/// <summary>
		/// Red
		/// </summary>
		public byte Red;

		/// <summary>
		/// Green
		/// </summary>
		public byte Green;

		/// <summary>
		/// Blue
		/// </summary>
		public byte Blue;

		/// <summary>
		/// Alpha
		/// </summary>
		public byte Alpha;

		/// <summary>
		///
		/// </summary>
		public static readonly Color Transparent = new Color(0, 0, 0, 0);

		/// <summary>
		/// Initializes a new instance of the <see cref="Color"/> struct.
		/// </summary>
		/// <param name="red">The red.</param>
		/// <param name="green">The green.</param>
		/// <param name="blue">The blue.</param>
		public Color(byte red, byte green, byte blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = 255;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Color"/> struct.
		/// </summary>
		/// <param name="red">The red.</param>
		/// <param name="green">The green.</param>
		/// <param name="blue">The blue.</param>
		/// <param name="alpha">The alpha.</param>
		public Color(byte red, byte green, byte blue, byte alpha)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}

		/// <summary>
		/// Determines whether the specified color is equal.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>
		/// 	<c>true</c> if the specified color is equal; otherwise, <c>false</c>.
		/// </returns>
		public bool IsEqual(Color color)
		{
			return ((color.Red == this.Red) && (color.Green == this.Green) && (color.Blue == this.Blue) && (color.Alpha == this.Alpha));
		}
	}
}
