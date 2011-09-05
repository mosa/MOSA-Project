/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

namespace Pictor
{
	public interface IColorType
	{
		RGBA_Doubles GetAsRGBA_Doubles();
		RGBA_Bytes GetAsRGBA_Bytes();

		RGBA_Bytes Gradient(RGBA_Bytes c, double k);

		uint R_Byte { get; set; }
		uint G_Byte { get; set; }
		uint B_Byte { get; set; }
		uint A_Byte { get; set; }
	};
}
