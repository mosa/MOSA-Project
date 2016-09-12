// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Author:
//       Benito Palacios Sánchez <benito356@gmail.com>
//
// Copyright (c) 2016 Benito Palacios Sánchez
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;

namespace Mosa.Utility.RSP
{
	internal static class Rle
	{
		private const string RunLengthStart = "*";

		public static string Decode(string data)
		{
			bool done = false;
			int startIdx;
			do
			{
				// Find next RLE start point
				startIdx = data.IndexOf(Rle.RunLengthStart);

				// No more RLE
				if (startIdx == -1)
				{
					done = true;
					continue;
				}

				// The first char must be the repeated char, not the start point
				if (startIdx == 0)
					throw new FormatException("[RLE] Invalid message");

				data = DecodeAt(data, startIdx);
			} while (!done);

			return data;
		}

		private static string DecodeAt(string data, int start)
		{
			char repeatedChar = data[start - 1];
			int length = data[start + 1] - 28;

			// Validate length
			if (!CheckLength(length))
				throw new FormatException("[RLE] Invalid length");

			// Remove RunLengthStart and length and insert the chars (keep the repeated char)
			data = data.Remove(start, 2);
			data = data.Insert(start, new string(repeatedChar, length - 1));
			return data;
		}

		private static bool CheckLength(int length)
		{
			return length >= 4 && length <= 97 && length != 6 && length != 7;
		}
	}
}
