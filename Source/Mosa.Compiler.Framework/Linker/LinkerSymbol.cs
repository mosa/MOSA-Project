﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Mosa.Compiler.Framework.Linker
{
	/// <summary>
	/// Linker Symbol
	/// </summary>
	public sealed class LinkerSymbol
	{
		private readonly object _lock = new object();

		public string Name { get; }

		public SectionKind SectionKind { get; }

		public Stream Stream { get; internal set; }

		public uint Alignment { get; internal set; }

		public bool IsDataAvailable { get { return Stream != null; } }

		public uint Size { get { return Stream != null ? (uint)Stream.Length : 0; } }

		public bool IsResolved { get { return VirtualAddress != 0; } }

		public uint SectionOffset { get; internal set; }

		public ulong VirtualAddress { get; internal set; }

		public List<LinkRequest> LinkRequests { get; }

		public string PreHash { get; internal set; }

		public string PostHash { get; internal set; }

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

		public string ComputeMD5Hash()
		{
			var md5 = MD5.Create();

			if (Stream == null)
				return string.Empty;

			Stream.Position = 0;

			var hash = md5.ComputeHash(Stream);

			var s = new StringBuilder();

			for (int i = 0; i < hash.Length; i++)
			{
				s.Append(hash[i].ToString("X2"));
			}

			return s.ToString();
		}
	}
}
