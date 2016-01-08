// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	///
	/// </summary>
	public sealed class LinkerSection
	{
		public List<LinkerSymbol> Symbols { get; private set; }

		private Dictionary<string, LinkerSymbol> symbolLookup;

		public string Name { get { return SectionKind.ToString(); } }

		public SectionKind SectionKind { get; private set; }

		public uint SectionAlignment { get; private set; }

		public bool IsResolved { get; private set; }

		public ulong VirtualAddress { get; private set; }

		public uint FileOffset { get; private set; }

		public uint Size { get; private set; }

		public uint AlignedSize { get { return Alignment.AlignUp(Size, SectionAlignment); } }

		private object mylock = new object();

		public LinkerSection(SectionKind sectionKind, uint alignment)
		{
			SectionKind = sectionKind;
			IsResolved = false;
			symbolLookup = new Dictionary<string, LinkerSymbol>();
			Symbols = new List<LinkerSymbol>();
			SectionAlignment = alignment;
			Size = 0;
		}

		internal void AddLinkerObject(LinkerSymbol symbol)
		{
			Symbols.Add(symbol);
			symbolLookup.Add(symbol.Name, symbol);
		}

		internal LinkerSymbol GetSymbol(string name)
		{
			LinkerSymbol symbol = null;

			symbolLookup.TryGetValue(name, out symbol);

			return symbol;
		}

		internal void ResolveLayout(uint fileOffset, ulong virtualAddress)
		{
			VirtualAddress = virtualAddress;
			FileOffset = fileOffset;

			foreach (var symbol in Symbols)
			{
				if (symbol.IsResolved)
					continue;

				Size = Alignment.AlignUp(Size, symbol.Alignment);

				symbol.SectionOffset = Size;
				symbol.VirtualAddress = VirtualAddress + Size;

				Size = Size + symbol.Size;
			}

			IsResolved = true;
		}

		internal void WriteTo(Stream stream)
		{
			foreach (var symbol in Symbols)
			{
				stream.Seek(FileOffset + symbol.SectionOffset, SeekOrigin.Begin);
				if (symbol.IsDataAvailable)
				{
					symbol.Stream.Position = 0;
					symbol.Stream.WriteTo(stream);
				}
			}

			stream.WriteZeroBytes((int)(FileOffset + AlignedSize - stream.Position));
		}
	}
}
