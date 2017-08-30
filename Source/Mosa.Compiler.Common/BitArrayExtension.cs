// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Text;

namespace Mosa.Compiler.Common
{
	public static class BitArrayExtension
	{
		public static string ToString2(this BitArray bitArray)
		{
			var sb = new StringBuilder();

			foreach (bool bit in bitArray)
			{
				sb.Append(bit ? "X" : ".");
			}

			return sb.ToString();
		}

		/// <summary>
		/// Ares the same.
		/// </summary>
		/// <param name="array1">The array1.</param>
		/// <param name="array2">The array2.</param>
		/// <returns></returns>
		public static bool AreSame(this BitArray array1, BitArray array2)
		{
			if (array1.Length != array2.Length)
				return false;

			for (int i = 0; i < array1.Length; i++)
			{
				if (array1[i] != array2[i])
					return false;
			}

			return true;
		}
	}
}
