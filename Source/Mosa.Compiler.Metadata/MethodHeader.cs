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
using Mosa.Compiler.Metadata;

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
		/// Reads the method header from the instruction stream.
		/// </summary>
		/// <param name="reader">The reader used to decode the instruction stream.</param>
		/// <returns></returns>
		public MethodHeader(EndianAwareBinaryReader reader)
		{
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

		}

	}
}