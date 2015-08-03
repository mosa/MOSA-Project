// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Pdb
{
	/// <summary>
	/// Contains utility functions to parse CodeView/PDB files.
	/// </summary>
	internal static class CvUtil
	{
		/// <summary>
		/// Reads a variable length C-style zero-terminated string.
		/// </summary>
		/// <param name="reader">The reader to read the string From.</param>
		/// <returns>The read string.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="reader"/> is null.</exception>
		public static string ReadString(BinaryReader reader)
		{
			return ReadString(reader, Encoding.ASCII);
		}

		/// <summary>
		/// Reads a variable length C-style zero-terminated string.
		/// </summary>
		/// <param name="reader">The reader to read the string From.</param>
		/// <param name="encoding">Specifies the encoding of the string to read.</param>
		/// <returns>The read string.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="reader"/> or <paramref name="encoding"/> is null.</exception>
		public static string ReadString(BinaryReader reader, Encoding encoding)
		{
			if (reader == null)
				throw new ArgumentNullException(@"reader");
			if (encoding == null)
				throw new ArgumentNullException(@"encoding");

			// We start with 32 and double the size until we
			// have found the \0 in the name.
			int size = 32, oldSize = 0, offset = 0, term;
			byte[] chars = new byte[size];
			long pos = reader.BaseStream.Position;

			do
			{
				// Read additional bytes from the buffer
				reader.Read(chars, offset, size - oldSize);

				// Determine the index of the \0
				term = Array.IndexOf(chars, (byte)0);
				if (term == -1)
				{
					// No terminator, extend the buffer
					oldSize = size;
					offset = size;
					size *= 2;
					Array.Resize(ref chars, size);
				}
			}
			while (term == -1);

			// We've found the terminating index, reposition the
			// stream after the \0
			reader.BaseStream.Position = pos + term + 1;

			// Return the name
			return encoding.GetString(chars, 0, term);
		}

		/// <summary>
		/// Skips bytes to get to the requested byte boundary.
		/// </summary>
		/// <param name="reader">The reader to skip bytes in.</param>
		/// <param name="byteBoundary">The byte boundary to skip to.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="reader"/> is null.</exception>
		public static void PadToBoundary(BinaryReader reader, int byteBoundary)
		{
			if (reader == null)
				throw new ArgumentNullException(@"reader");

			int mod = (int)(reader.BaseStream.Position % byteBoundary);
			if (mod > 0)
				reader.BaseStream.Position += (byteBoundary - mod);
		}
	}
}