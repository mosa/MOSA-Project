/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using System.IO;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	///
	/// </summary>
	public class LinkerSymbol
	{
		public string Name { get; private set; }

		public SectionKind SectionKind { get; private set; }

		public MemoryStream Stream { get; internal set; }

		public uint Alignment { get; internal set; }

		public bool IsAvailable { get { return Stream != null; } }

		public ulong Size { get { return Stream != null ? (ulong)Stream.Length : 0; } }

		public bool IsResolved { get { return ResolvedVirtualAddress != 0; } }

		public ulong ResolvedSectionOffset { get; internal set; }

		public ulong ResolvedVirtualAddress { get; internal set; }

		internal LinkerSymbol(string name, SectionKind kind, uint alignment)
		{
			Name = name;
			Alignment = alignment;
			SectionKind = kind;
		}

		public void SetData(MemoryStream stream)
		{
			Stream = stream;
		}

		public void SetData(byte[] data)
		{
			Stream = new MemoryStream(data);
		}

		public void ApplyPatch(long offset, ulong value, ulong mask, byte patchSize, Endianness endianness)
		{
			Stream.Position = offset;

			ulong current = 0;

			switch (patchSize)
			{
				case 8: current = (ulong)Stream.ReadByte(); break;
				case 16: current = (ulong)Stream.ReadUInt16(endianness); break;
				case 32: current = (ulong)Stream.ReadUInt32(endianness); break;
				case 64: current = (ulong)Stream.ReadUInt64(endianness); break;
			}

			Stream.Position = offset;
			current = (current & ~mask) | value;

			// Apply the patch
			switch (patchSize)
			{
				case 8: Stream.WriteByte((byte)current); break;
				case 16: Stream.Write((ushort)current, endianness); break;
				case 32: Stream.Write((uint)current, endianness); break;
				case 64: Stream.Write((ulong)current, endianness); break;
			}
		}

		public override string ToString()
		{
			return SectionKind.ToString() + ": " + Name;
		}
	}
}