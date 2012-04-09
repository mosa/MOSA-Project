/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.AVR32.Intrinsic;

namespace Mosa.Kernel.AVR32
{
	/// <summary>
	/// 
	/// </summary>
	public static class Kernel
	{

		public static void Setup()
		{
			Native.Nop();
		}
	}
}
