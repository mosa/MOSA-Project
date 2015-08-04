// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;

namespace Mosa.Compiler.Common
{
	public static class BitArrayExtension
	{
		public static bool AreSame(this BitArray array1, BitArray array2)
		{
			if (array1.Length != array2.Length)
				return false;

			for (int i = 0; i < array1.Length; i++)
				if (array1[i] != array2[i])
					return false;

			return true;
		}
	}
}
