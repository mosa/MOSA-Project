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
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;

namespace Mosa.Platform.AVR32
{
	/// <summary>
	/// An AVR32 machine code emitter.
	/// </summary>
	public sealed class MachineCodeEmitter : BaseCodeEmitter, ICodeEmitter, IDisposable
	{
		private static readonly DataConverter LittleEndianBitConverter = DataConverter.LittleEndian;

		#region ICodeEmitter Members

		void ICodeEmitter.ResolvePatches()
		{
			// Save the current position
			long currentPosition = codeStream.Position;

			foreach (Patch p in patches)
			{
				long labelPosition;
				if (!labels.TryGetValue(p.Label, out labelPosition))
				{
					throw new ArgumentException(@"Missing label while resolving patches.", @"label");
				}

				codeStream.Position = p.Position;

				// Compute relative branch offset
				int relOffset = (int)labelPosition - ((int)p.Position + 4);

				// Write relative offset to stream
				byte[] bytes = LittleEndianBitConverter.GetBytes(relOffset);
				codeStream.Write(bytes, 0, bytes.Length);
			}

			patches.Clear();

			// Reset the position
			codeStream.Position = currentPosition;
		}

		#endregion // ICodeEmitter Members

		#region Code Generation Members

		/// <summary>
		/// Writes the unsigned short.
		/// </summary>
		/// <param name="data">The data.</param>
		public void WriteUShort(ushort data)
		{
			codeStream.WriteByte((byte)((data >> 8) & 0xFF));
			codeStream.WriteByte((byte)(data & 0xFF));
		}

		/// <summary>
		/// Writes the unsigned int.
		/// </summary>
		/// <param name="data">The data.</param>
		public void WriteUShort(uint data)
		{
			codeStream.WriteByte((byte)((data >> 24) & 0xFF));
			codeStream.WriteByte((byte)((data >> 16) & 0xFF));
			codeStream.WriteByte((byte)((data >> 8) & 0xFF));
			codeStream.WriteByte((byte)(data & 0xFF));
		}

		#endregion

	}
}
