// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.IO;
using System.Collections.Generic;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	///
	/// </summary>
	public sealed class LinkerSymbol
	{
		public string Name { get; private set; }

		public SectionKind SectionKind { get; private set; }

		public Stream Stream { get; internal set; }

		public uint Alignment { get; internal set; }

		public bool IsDataAvailable { get { return Stream != null; } }

		public uint Size { get { return Stream != null ? (uint)Stream.Length : 0; } }

		public bool IsResolved { get { return VirtualAddress != 0; } }

		public uint SectionOffset { get; internal set; }

		public ulong VirtualAddress { get; internal set; }

		public List<LinkRequest> LinkRequests { get; private set; }

		internal LinkerSymbol(string name, SectionKind kind, uint alignment)
		{
			Name = name;
			Alignment = alignment;
			SectionKind = kind;
			LinkRequests = new List<LinkRequest>();
        }

		public void SetData(MemoryStream stream)
		{
			Stream = stream;
		}

		public void SetData(byte[] data)
		{
			Stream = Stream.Synchronized(new MemoryStream(data));
		}

		public void AddPatch(LinkRequest linkRequest)
		{
			lock(this)
			{
				LinkRequests.Add(linkRequest);
			}
		}

		public void RemovePatches()
		{
			lock (this)
			{
				LinkRequests.Clear();
			}
		}

		public void ApplyPatch(long offset, ulong value, ulong mask, byte patchSize, Endianness endianness)
		{
			try
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
			catch(Exception ex)
			{
				return;
			}
		}

		public override string ToString()
		{
			return SectionKind.ToString() + ": " + Name;
		}
	}
}
