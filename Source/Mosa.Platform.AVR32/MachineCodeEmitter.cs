/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Platform;

namespace Mosa.Platform.AVR32
{
	/// <summary>
	/// An AVR32 machine code emitter.
	/// </summary>
	public sealed class MachineCodeEmitter : BaseCodeEmitter, ICodeEmitter, IDisposable
	{

		public MachineCodeEmitter()
		{
			bitConverter = DataConverter.BigEndian;
		}

		#region Code Generation Members

		/// <summary>
		/// Writes the unsigned short.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Write(ushort data)
		{
			codeStream.WriteByte((byte)((data >> 8) & 0xFF));
			codeStream.WriteByte((byte)(data & 0xFF));
		}

		/// <summary>
		/// Writes the unsigned int.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Write(uint data)
		{
			codeStream.WriteByte((byte)((data >> 24) & 0xFF));
			codeStream.WriteByte((byte)((data >> 16) & 0xFF));
			codeStream.WriteByte((byte)((data >> 8) & 0xFF));
			codeStream.WriteByte((byte)(data & 0xFF));
		}

		private bool Is32Opcode(uint opcode)
		{
			return ((opcode & 0xE0000000) == 0xE000000);
		}

		public void WriteOpcode(uint opcode)
		{
			if (Is32Opcode(opcode))
				Write(opcode);
			else
				Write((ushort)opcode);
		}

		#endregion

	}
}
