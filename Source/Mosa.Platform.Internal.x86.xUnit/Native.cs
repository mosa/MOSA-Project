/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Platform.Internal.x86
{
	public static class Native
	{
		public static uint Div(ulong a, uint b)
		{
			return (uint)(a / b);
		}
	}
}