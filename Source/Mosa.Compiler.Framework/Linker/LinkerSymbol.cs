// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Framework.Linker
{
	/// <summary>
	/// Linker Symbol
	/// </summary>
	public sealed class LinkerSymbol
	{
		public string Name { get; }

		public SectionKind SectionKind { get; internal set; }

		public Stream Stream { get; internal set; }

		public uint Alignment { get; internal set; }

		public bool IsDataAvailable { get { return Stream != null; } }

		public uint Size { get { return Stream != null ? (uint)Stream.Length : 0; } }

		public bool IsResolved { get { return VirtualAddress != 0; } }

		public string ExternalSymbolName { get; internal set; }

		public bool IsExternalSymbol { get; set; }

		public uint SectionOffset { get; internal set; }

		public ulong VirtualAddress { get; internal set; }

		public bool IsReplaced { get; internal set; }

		public List<LinkRequest> LinkRequests { get; }

		private readonly object _lock = new object();

		internal LinkerSymbol(string name, uint alignment = 0, SectionKind kind = SectionKind.Unknown)
		{
			Name = name;
			Alignment = alignment;
			SectionKind = kind;
			LinkRequests = new List<LinkRequest>();
			IsExternalSymbol = false;
			IsReplaced = false;
		}

		public void SetData(MemoryStream stream)
		{
			Stream = stream;
		}

		public void SetData(byte[] data)
		{
			SetData(new MemoryStream(data));
		}

		public void SetReplacementStatus(bool replaced)
		{
			IsReplaced = replaced;
		}

		public void AddPatch(LinkRequest linkRequest)
		{
			lock (_lock)
			{
				LinkRequests.Add(linkRequest);
			}
		}

		public void RemovePatches()
		{
			lock (_lock)
			{
				LinkRequests.Clear();
			}
		}

		public LinkRequest[] GetLinkRequests()
		{
			lock (_lock)
			{
				return LinkRequests.ToArray();
			}
		}

		public void ApplyPatch(long offset, ulong value, byte patchSize, Endianness endianness)
		{
			Stream.Position = offset;

			// Apply the patch
			switch (patchSize)
			{
				case 8: Stream.WriteByte((byte)value); break;
				case 16: Stream.Write((ushort)value, endianness); break;
				case 32: Stream.Write((uint)value, endianness); break;
				case 64: Stream.Write(value, endianness); break;
			}
		}

		public override string ToString()
		{
			return SectionKind.ToString() + ": " + Name;
		}
	}
}
