/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;

namespace Mosa.Platform.Internal.x86
{
	// TODO: Implement properly for SZ arrays and multi dimensional arrays
	public unsafe static class InternalsForArray
	{
		public static int GetLength(void* o, int dimension)
		{
			return *(((int*)o) + 2);
		}

		public static int GetLowerBound(void* o, int dimension)
		{
			return 0;
		}
	}
}