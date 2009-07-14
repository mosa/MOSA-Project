#if MOSAPROJECT

using System.Text;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Mono.Globalization.Unicode;

namespace System
{
	public partial class String
	{
		unsafe public String (char *value)
		{
			throw new System.NotImplementedException();
		}
		unsafe public String (char *value, int startIndex, int length)
		{
			throw new System.NotImplementedException();
		}
		unsafe public String (sbyte *value)
		{
			throw new System.NotImplementedException();
		}
		unsafe public String (sbyte *value, int startIndex, int length)
		{
			throw new System.NotImplementedException();
		}
		unsafe public String (sbyte *value, int startIndex, int length, Encoding enc)
		{
			throw new System.NotImplementedException();
		}
		public String (char [] value, int startIndex, int length)
		{
			throw new System.NotImplementedException();
		}
		public String (char [] value)
		{
			throw new System.NotImplementedException();
		}
		public String (char c, int count)
		{
			throw new System.NotImplementedException();
		}

		internal unsafe String[] InternalSplit(char[] separator, int count, int options)
		{
			int srcsize = this.Length;
			int arrsize = separator.Length;
			int remempty = options & 1;
			int splitsize = 1;

			string[] retarr;
			int lastpos;
			int arrpos;
			int flag;

			fixed (char* src = &this.start_char) {

				/* Count the number of elements we will return. Note that this operation
				 * guarantees that we will return exactly splitsize elements, and we will
				 * have enough data to fill each. This allows us to skip some checks later on.
				 */
				if (remempty == 0) {
					for (int i = 0; i != srcsize && splitsize < count; i++) {
						if (InternalIsSeperator(separator, arrsize, src[i]))
							splitsize++;
					}
				}
				else if (count > 1) {
					/* Require pattern "Nondelim + Delim + Nondelim" to increment counter.
					 * Lastpos != 0 means first nondelim found.
					 * Flag = 0 means last char was delim.
					 * Efficient, though perhaps confusing.
					 */
					lastpos = 0;
					flag = 0;
					for (int i = 0; i != srcsize && splitsize < count; i++) {
						if (InternalIsSeperator(separator, arrsize, src[i])) {
							flag = 0;
						}
						else if (flag == 0) {
							if (lastpos == 1)
								splitsize++;
							flag = 1;
							lastpos = 1;
						}
					}

					/* Nothing but separators */
					if (lastpos == 0) {
						retarr = new string[0];
						return retarr;
					}
				}

				/* if no split chars found return the string */
				if (splitsize == 1) {
					if (remempty == 0 || count == 1) {
						/* Copy the whole string */
						retarr = new string[1];
						retarr[0] = this;
					}
					else {
						/* otherwise we have to filter out leading & trailing delims */
						int start = 0;
						/* find first non-delim char */
						for (; srcsize != 0; srcsize--, start++) {
							if (!InternalIsSeperator(separator, arrsize, src[start]))
								break;
						}
						/* find last non-delim char */
						for (; srcsize != 0; srcsize--) {
							if (!InternalIsSeperator(separator, arrsize, src[start + srcsize - 1]))
								break;
						}

						retarr = new string[1];
						retarr[0] = SubstringUnchecked(start, srcsize);
					}
					return retarr;
				}

				lastpos = 0;
				arrpos = 0;

				retarr = new string[splitsize];

				for (int i = 0; i != srcsize && arrpos != splitsize; i++) {
					if (InternalIsSeperator(separator, arrsize, src[i])) {

						if (lastpos != i || remempty == 0) {
							retarr[arrpos] = SubstringUnchecked(lastpos, i - lastpos); ;
							arrpos++;

							if (arrpos == splitsize - 1) {
								/* Shortcut the last array element */
								lastpos = i + 1;

								if (remempty != 0) {
									/* Search for non-delim starting char (guaranteed to find one) Note that loop
									 * condition is only there for safety. It will never actually terminate the loop. */
									for (; lastpos != srcsize; lastpos++) {
										if (!InternalIsSeperator(separator, arrsize, src[lastpos]))
											break;
									}
									if (count > splitsize) {
										/* Since we have fewer results than our limit, we must remove
										 * trailing delimiters as well. 
										 */
										for (; srcsize != lastpos + 1; srcsize--) {
											if (!InternalIsSeperator(separator, arrsize, src[srcsize - 1]))
												break;
										}
									}
								}

								retarr[arrpos] = SubstringUnchecked(lastpos, srcsize - lastpos);

								/* Loop will ALWAYS end here. Test criteria in the FOR loop is technically unnecessary. */
								break;
							}
						}
						lastpos = i + 1;
					}
				}
			}
			return retarr;
		}

		internal static bool InternalIsSeperator(char[] chars, int arraylength, char chr)
		{
			for (int arrpos = 0; arrpos != arraylength; arrpos++)
				if (chars[arrpos] == chr)
					return true;

			return false;
		}

		internal static String InternalAllocateStr (int length)
		{
			throw new System.NotImplementedException();
		}
		internal static void InternalStrcpy (String dest, int destPos, String src)
		{
			throw new System.NotImplementedException();
		}
		internal static void InternalStrcpy (String dest, int destPos, char[] chars)
		{
			throw new System.NotImplementedException();
		}
		internal static void InternalStrcpy (String dest, int destPos, String src, int sPos, int count)
		{
			throw new System.NotImplementedException();
		}
		internal static void InternalStrcpy (String dest, int destPos, char[] chars, int sPos, int count)
		{
			throw new System.NotImplementedException();
		}
		private static string InternalIntern (string str)
		{
			throw new System.NotImplementedException();
		}
		private static string InternalIsInterned (string str)
		{
			throw new System.NotImplementedException();
		}

	}
}

#endif
