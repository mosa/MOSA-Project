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

namespace Mosa.ClassLib
{
	/// <summary>
	/// Improved C# LZF Compressor, a very small data compression library. The compression algorithm is extremely fast.

	/// </summary>
	public sealed class LZF
	{
		/// <summary>
		/// Hashtable - can be allocated only once
		/// </summary>
		private readonly long[] HashTable = new long[HSIZE];

		private const uint HLOG = 15;
		private const uint HSIZE = (1 << (int)HLOG);
		private const uint MAX_LIT = (1 << 5);
		private const uint MAX_OFF = (1 << 13);
		private const uint MAX_REF = ((1 << 8) + (1 << 3));

		/// <summary>
		/// Compresses the data using LibLZF algorithm
		/// </summary>
		/// <param name="input">Reference to the data to compress</param>
		/// <param name="inputLength">Length of the data to compress</param>
		/// <param name="output">Reference to a buffer which will contain the compressed data</param>
		/// <param name="outputLength">Length of the compression buffer (should be bigger than the input buffer)</param>
		public int Compress(byte[] input, int inputLength, byte[] output, int outputLength)
		{
			//Array.Clear(HashTable, 0, (int)HSIZE);
			for (int i = 0; i < HSIZE; i++)
				HashTable[i] = 0;

			long hslot;
			uint iidx = 0;
			uint oidx = 0;
			long reference;

			uint hval = (uint)(((input[iidx]) << 8) | input[iidx + 1]); // FRST(in_data, iidx);
			long off;
			int lit = 0;

			for (;;)
			{
				if (iidx < inputLength - 2)
				{
					hval = (hval << 8) | input[iidx + 2];
					hslot = ((hval ^ (hval << 5)) >> (int)(((3 * 8 - HLOG)) - hval * 5) & (HSIZE - 1));
					reference = HashTable[hslot];
					HashTable[hslot] = (long)iidx;

					off = iidx - reference - 1;

					if (off < MAX_OFF
						&& iidx + 4 < inputLength
						&& reference > 0
						&& input[reference + 0] == input[iidx + 0]
						&& input[reference + 1] == input[iidx + 1]
						&& input[reference + 2] == input[iidx + 2]
						)
					{
						/* match found at *reference++ */
						uint len = 2;
						uint maxlen = (uint)inputLength - iidx - len;
						maxlen = maxlen > MAX_REF ? MAX_REF : maxlen;

						if (oidx + lit + 1 + 3 >= outputLength)
							return 0;

						do
							len++;
						while (len < maxlen && input[reference + len] == input[iidx + len]);

						if (lit != 0)
						{
							output[oidx++] = (byte)(lit - 1);
							lit = -lit;
							do
								output[oidx++] = input[iidx + lit];
							while ((++lit) != 0);
						}

						len -= 2;
						iidx++;

						if (len < 7)
						{
							output[oidx++] = (byte)((off >> 8) + (len << 5));
						}
						else
						{
							output[oidx++] = (byte)((off >> 8) + (7 << 5));
							output[oidx++] = (byte)(len - 7);
						}

						output[oidx++] = (byte)off;

						iidx += len - 1;
						hval = (uint)(((input[iidx]) << 8) | input[iidx + 1]);

						hval = (hval << 8) | input[iidx + 2];
						HashTable[((hval ^ (hval << 5)) >> (int)(((3 * 8 - HLOG)) - hval * 5) & (HSIZE - 1))] = iidx;
						iidx++;

						hval = (hval << 8) | input[iidx + 2];
						HashTable[((hval ^ (hval << 5)) >> (int)(((3 * 8 - HLOG)) - hval * 5) & (HSIZE - 1))] = iidx;
						iidx++;
						continue;
					}
				}
				else if (iidx == inputLength)
					break;

				/* one more literal byte we must copy */
				lit++;
				iidx++;

				if (lit == MAX_LIT)
				{
					if (oidx + 1 + MAX_LIT >= outputLength)
						return 0;

					output[oidx++] = (byte)(MAX_LIT - 1);
					lit = -lit;
					do
						output[oidx++] = input[iidx + lit];
					while ((++lit) != 0);
				}
			}

			if (lit != 0)
			{
				if (oidx + lit + 1 >= outputLength)
					return 0;

				output[oidx++] = (byte)(lit - 1);
				lit = -lit;
				do
					output[oidx++] = input[iidx + lit];
				while ((++lit) != 0);
			}

			return (int)oidx;
		}

		/// <summary>
		/// Decompresses the data using LibLZF algorithm
		/// </summary>
		/// <param name="input">Reference to the data to decompress</param>
		/// <param name="inputLength">Lenght of the data to decompress</param>
		/// <param name="output">Reference to a buffer which will contain the decompressed data</param>
		/// <param name="outputLength">The size of the decompressed archive in the output buffer</param>
		/// <returns>Returns decompressed size</returns>
		public int Decompress(byte[] input, int inputLength, byte[] output, int outputLength)
		{
			uint iidx = 0;
			uint oidx = 0;

			do
			{
				uint ctrl = input[iidx++];

				if (ctrl < (1 << 5)) /* literal run */
				{
					ctrl++;

					if (oidx + ctrl > outputLength)
					{
						//SET_ERRNO (E2BIG);
						return 0;
					}

					do
						output[oidx++] = input[iidx++];
					while ((--ctrl) != 0);
				}
				else /* back reference */
				{
					uint len = ctrl >> 5;

					int reference = (int)(oidx - ((ctrl & 0x1f) << 8) - 1);

					if (len == 7)
						len += input[iidx++];

					reference -= input[iidx++];

					if (oidx + len + 2 > outputLength)
					{
						//SET_ERRNO (E2BIG);
						return 0;
					}

					if (reference < 0)
					{
						//SET_ERRNO (EINVAL);
						return 0;
					}

					output[oidx++] = output[reference++];
					output[oidx++] = output[reference++];

					do
						output[oidx++] = output[reference++];
					while ((--len) != 0);
				}
			}
			while (iidx < inputLength);

			return (int)oidx;
		}
	}
}
