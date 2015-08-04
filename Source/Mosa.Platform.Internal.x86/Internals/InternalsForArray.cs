// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
