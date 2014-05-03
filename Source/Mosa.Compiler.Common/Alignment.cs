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
		public static uint AlignUp(uint position, uint alignment)
		{
			if (alignment == 0)
				return position;

			uint off = position % alignment;

			if (off != 0)
			{
				position += (alignment - off);
			}

			return position;
		}

		public static int AlignUp(int position, int alignment)
		{
			if (alignment == 0)
				return position;

			int off = position % alignment;

			if (off != 0)
			{
				position += (alignment - off);
			}

			return position;
		}

		public static long AlignUp(long position, uint alignment)
		{
			if (alignment == 0)
				return position;

			long off = position % alignment;

			if (off != 0)
			{
				position += (alignment - off);
			}

			return position;
		}

		public static int AlignDown(int position, int alignment)
		{
			if (alignment == 0)
				return position;

			position -= position % alignment;

			return position;
		}
	}
}