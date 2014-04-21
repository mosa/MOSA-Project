/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	///
	/// </summary>
	public class LinkerSection
	{
		public List<LinkerSymbol> Symbols { get; private set; }

		// TODO! for debugging
		//public List<LinkerSymbol> Ordered { get; private set; }

		private Dictionary<string, LinkerSymbol> symbolLookup;

		public string Name { get; private set; }

		public SectionKind SectionKind { get; private set; }

		public uint SectionAlignment { get; private set; }

		public bool IsResolved { get; private set; }

		public ulong ResolvedSectionOffset { get; private set; }

		public ulong ResolvedVirtualAddress { get; private set; }

		public ulong Size { get; private set; }

		private object mylock = new object();

		public LinkerSection(SectionKind sectionKind, string name, uint alignment)
		{
			Name = name;
			SectionKind = sectionKind;
			IsResolved = false;
			symbolLookup = new Dictionary<string, LinkerSymbol>();
			Symbols = new List<LinkerSymbol>();
			//Ordered = new List<LinkerSymbol>();
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

		internal void ResolveLayout(ulong sectionOffset, ulong virtualAddress)
		{
			ResolvedSectionOffset = sectionOffset;
			ResolvedVirtualAddress = virtualAddress;

			foreach (var symbol in Symbols)
			//foreach (var symbol in Ordered)
			{
				if (symbol.IsResolved)
					continue;

				Size = Alignment.Align(Size, symbol.Alignment);

				symbol.ResolvedSectionOffset = Size;
				symbol.ResolvedVirtualAddress = ResolvedVirtualAddress + Size;

				Size = Size + symbol.Size;
			}

			//Size = Alignment.Align(Size, SectionAlignment);

			IsResolved = true;
		}

		internal void WriteTo(Stream stream)
		{
			long start = stream.Position;

			foreach (var symbol in Symbols)
			//foreach (var symbol in Ordered)
			{
				long at = (long)symbol.ResolvedSectionOffset + start;
				stream.Seek(at, SeekOrigin.Begin);
				symbol.Stream.Position = 0;
				symbol.Stream.WriteTo(stream);
			}
		}
	}
}