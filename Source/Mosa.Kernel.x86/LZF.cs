// Copyright (c) MOSA Project. Licensed under the New BSD License.

// LibLZF used under the following license:

/*
 * Improved version to C# LibLZF Port:
 * Copyright (c) 2010 Roman Atachiants <kelindar@gmail.com>
 *
 * Original CLZF Port:
 * Copyright (c) 2005 Oren J. Maurice <oymaurice@hazorea.org.il>
 *
 * Original LibLZF Library & Algorithm:
 * Copyright (c) 2000-2008 Marc Alexander Lehmann <schmorp@schmorp.de>
 *
 * Redistribution and use in source and binary forms, with or without modifica-
 * tion, are permitted provided that the following conditions are met:
 *
 *   1.  Redistributions of source code must retain the above copyright notice,
 *       this list of conditions and the following disclaimer.
 *
 *   2.  Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *
 *   3.  The name of the author may not be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MER-
 * CHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO
 * EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPE-
 * CIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTH-
 * ERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * Alternatively, the contents of this file may be used under the terms of
 * the GNU General Public License version 2 (the "GPL"), in which case the
 * provisions of the GPL are applicable instead of the above. If you wish to
 * allow the use of your version of this file only under the terms of the
 * GPL and not to allow others to use your version of this file under the
 * BSD license, indicate your decision by deleting the provisions above and
 * replace them with the notice and other provisions required by the GPL. If
 * you do not delete the provisions above, a recipient may use your version
 * of this file under either the BSD or the GPL.
 */

using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Improved C# LZF Compressor, a very small data compression library. The compression algorithm is extremely fast.
	/// </summary>
	public static class LZF
	{
		/// <summary>
		/// Decompresses the data using LibLZF algorithm
		/// </summary>
		/// <param name="input">Reference to the data to decompress</param>
		/// <param name="inputLength">Length of the data to decompress</param>
		/// <param name="output">Reference to a buffer which will contain the decompressed data</param>
		/// <param name="outputLength">The size of the decompressed archive in the output buffer</param>
		public static bool Decompress(uint input, uint inputLength, uint output, uint outputLength)
		{
			uint iidx = 0;
			uint oidx = 0;

			do
			{
				//uint ctrl = input[iidx++];
				uint ctrl = Native.Get8(input + iidx);
				iidx++;

				if (ctrl < (1 << 5)) /* literal run */
				{
					ctrl++;

					if (oidx + ctrl > outputLength)
					{
						//SET_ERRNO (E2BIG);
						return false;
					}

					do
					{
						//output[oidx++] = input[iidx++];
						Native.Set8(output + oidx, Native.Get8(input + iidx));
						oidx++;
						iidx++;
					}
					while ((--ctrl) != 0);
				}
				else /* back reference */
				{
					uint len = ctrl >> 5;

					uint reference = (uint)(oidx - ((ctrl & 0x1f) << 8) - 1);

					if (len == 7)
					{
						//len += input[iidx++];
						len += Native.Get8(input + iidx);
						iidx++;
					}

					//reference -= input[iidx++];
					reference -= Native.Get8(input + iidx);
					iidx++;

					if (oidx + len + 2 > outputLength)
					{
						//SET_ERRNO (E2BIG);
						return false;
					}

					if (reference < 0)
					{
						//SET_ERRNO (EINVAL);
						return false;
					}

					//output[oidx++] = output[reference++];
					Native.Set8(output + oidx, Native.Get8(output + reference));
					oidx++;
					reference++;

					//output[oidx++] = output[reference++];
					Native.Set8(output + oidx, Native.Get8(output + reference));
					oidx++;
					reference++;

					do
					{
						//output[oidx++] = output[reference++];
						Native.Set8(output + oidx, Native.Get8(output + reference));
						oidx++;
						reference++;
					}
					while ((--len) != 0);
				}
			}
			while (iidx < inputLength);

			return true;
		}
	}
}
