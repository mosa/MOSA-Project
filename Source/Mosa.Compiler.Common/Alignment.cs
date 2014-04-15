/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Common
{
	public static class Alignment
	{

		public static long Align(long position, uint alignment)
		{
			long off = position % alignment;

			if (off != 0)
			{
				position += (alignment - off);
			}

			return position;
		}

		public static ulong Align(ulong position, uint alignment)
		{
			ulong off = position % alignment;

			if (off != 0)
			{
				position += (alignment - off);
			}

			return position;
		}
	}
}