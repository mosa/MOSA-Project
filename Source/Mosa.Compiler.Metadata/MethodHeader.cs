/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Metadata
{
	/// <summary>
	///
	/// </summary>
	public class MethodHeader
	{
		/// <summary>
		/// Header flags
		/// </summary>
		public MethodFlags Flags { get; private set; }

		/// <summary>
		/// Maximum stack size
		/// </summary>
		public ushort MaxStack { get; private set; }

		/// <summary>
		/// Size of the code in bytes
		/// </summary>
		public uint CodeSize { get; private set; }

		/// <summary>
		/// Local variable signature token
		/// </summary>
		public Token LocalVarSigTok { get; private set; }

		/// <summary>
		/// Gets the clauses.
		/// </summary>
		/// <value>The clauses.</value>
		public List<ExceptionHandlingClause> Clauses { get; private set; }

		/// <summary>
		/// Reads the method header from the instruction stream.
		/// </summary>
		/// <param name="reader">The reader used to decode the instruction stream.</param>
		/// <returns></returns>
		public MethodHeader(EndianAwareBinaryReader reader)
		{
			Clauses = new List<ExceptionHandlingClause>();

			// Read first byte
			Flags = (MethodFlags)reader.ReadByte();

			// Check least significant 2 bits
			switch (Flags & MethodFlags.HeaderMask)
			{
				case MethodFlags.TinyFormat:
					CodeSize = ((uint)(Flags & MethodFlags.TinyCodeSizeMask) >> 2);
					Flags &= MethodFlags.HeaderMask;
					break;

				case MethodFlags.FatFormat:
					// Read second byte of flags
					Flags = (MethodFlags)(reader.ReadByte() << 8 | (byte)Flags);

					if (MethodFlags.ValidHeader != (Flags & MethodFlags.HeaderSizeMask))
						throw new CompilerException("Invalid method ");

					MaxStack = reader.ReadUInt16();
					CodeSize = reader.ReadUInt32();
					LocalVarSigTok = new Token(reader.ReadUInt32()); // ReadStandAloneSigRow
					break;

				default:
					throw new CompilerException("Invalid method header");
			}

			// Are there sections following the code?
			if (MethodFlags.MoreSections != (Flags & MethodFlags.MoreSections))
				return;

			// Yes, seek to them and process those sections
			long codepos = reader.BaseStream.Position;

			// Seek to the end of the code...
			long dataSectPos = codepos + CodeSize;
			if (0 != (dataSectPos & 3))
				dataSectPos += (4 - (dataSectPos % 4));
			reader.BaseStream.Position = dataSectPos;

			// Read all headers, so the IL decoder knows how to handle these...
			byte flags;

			do
			{
				flags = reader.ReadByte();
				bool isFat = (0x40 == (flags & 0x40));
				int length;
				int blocks;
				if (isFat)
				{
					byte a = reader.ReadByte();
					byte b = reader.ReadByte();
					byte c = reader.ReadByte();

					length = (c << 24) | (b << 16) | a;
					blocks = (length - 4) / 24;
				}
				else
				{
					length = reader.ReadByte();
					blocks = (length - 4) / 12;

					/* Read & skip the padding. */
					reader.ReadInt16();
				}

				Debug.Assert(0x01 == (flags & 0x3F), "Unsupported method data section.");

				// Read the clause
				for (int i = 0; i < blocks; i++)
				{
					ExceptionHandlingClause clause = new ExceptionHandlingClause();
					clause.Read(reader, isFat);
					Clauses.Add(clause);
				}
			}
			while (0x80 == (flags & 0x80));

			reader.BaseStream.Position = codepos;
		}
	}
}