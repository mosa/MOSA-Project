// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Linker.Elf;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Framework.Linker
{
	/// <summary>
	/// Base Linker
	/// </summary>
	public sealed class MosaLinker
	{

		public delegate List<Section> CreateExtraSectionsDelegate();
		public delegate List<ProgramHeader> CreateExtraProgramHeaderDelegate();

		public List<LinkerSymbol> Symbols { get; } = new List<LinkerSymbol>();

		public LinkerSection[] LinkerSections { get; } = new LinkerSection[4];

		public LinkerSymbol EntryPoint { get; set; }

		public Endianness Endianness { get; }

		public MachineType MachineType { get; }

		public ulong BaseAddress { get; }

		public uint SectionAlignment { get; set; }

		public uint BaseFileOffset { get; set; }

		public bool EmitAllSymbols { get; set; }

		public bool EmitStaticRelocations { get; set; }

		public LinkerFormatType LinkerFormatType { get; }

		private readonly ElfLinker elfLinker;

		public static readonly SectionKind[] SectionKinds = new[] { SectionKind.Text, SectionKind.Data, SectionKind.ROData, SectionKind.BSS };

		private readonly Dictionary<string, LinkerSymbol> symbolLookup = new Dictionary<string, LinkerSymbol>();

		private readonly object _lock = new object();

		public CreateExtraSectionsDelegate CreateExtraSections { get; set; }
		public CreateExtraProgramHeaderDelegate CreateExtraProgramHeaders { get; set; }

		public MosaLinker(ulong baseAddress, Endianness endianness, MachineType machineType, bool emitAllSymbols, bool emitStaticRelocations, LinkerFormatType linkerFormatType, CreateExtraSectionsDelegate createExtraSections, CreateExtraProgramHeaderDelegate createExtraProgramHeaders)
		{
			BaseAddress = baseAddress;
			Endianness = endianness;
			MachineType = machineType;
			EmitAllSymbols = emitAllSymbols;
			EmitStaticRelocations = emitStaticRelocations;
			LinkerFormatType = linkerFormatType;
			CreateExtraSections = createExtraSections;
			CreateExtraProgramHeaders = createExtraProgramHeaders;

			elfLinker = new ElfLinker(this, LinkerFormatType);

			BaseFileOffset = elfLinker.BaseFileOffset;
			SectionAlignment = elfLinker.SectionAlignment;

			AddSection(new LinkerSection(SectionKind.Text, SectionAlignment));
			AddSection(new LinkerSection(SectionKind.Data, SectionAlignment));
			AddSection(new LinkerSection(SectionKind.ROData, SectionAlignment));
			AddSection(new LinkerSection(SectionKind.BSS, SectionAlignment));
		}

		public void Emit(Stream stream)
		{
			FinalizeLayout();

			elfLinker.Emit(stream);
		}

		private void AddSection(LinkerSection linkerSection)
		{
			LinkerSections[(int)linkerSection.SectionKind] = linkerSection;
		}

		public void Link(LinkType linkType, PatchType patchType, LinkerSymbol patchSymbol, int patchOffset, LinkerSymbol referenceSymbol, int referenceOffset)
		{
			lock (_lock)
			{
				var linkRequest = new LinkRequest(linkType, patchType, patchSymbol, patchOffset, referenceSymbol, referenceOffset);

				patchSymbol.AddPatch(linkRequest);
			}
		}

		public void Link(LinkType linkType, PatchType patchType, string patchSymbolName, int patchOffset, string referenceSymbolName, int referenceOffset)
		{
			var referenceSymbol = GetSymbol(referenceSymbolName);
			var patchObject = GetSymbol(patchSymbolName);

			Link(linkType, patchType, patchObject, patchOffset, referenceSymbol, referenceOffset);
		}

		public void Link(LinkType linkType, PatchType patchType, LinkerSymbol patchSymbol, int patchOffset, string referenceSymbolName, int referenceOffset)
		{
			var referenceObject = GetSymbol(referenceSymbolName);

			Link(linkType, patchType, patchSymbol, patchOffset, referenceObject, referenceOffset);
		}

		public LinkerSymbol GetSymbol(string name)
		{
			lock (_lock)
			{
				if (!symbolLookup.TryGetValue(name, out LinkerSymbol symbol))
				{
					symbol = new LinkerSymbol(name);

					Symbols.Add(symbol);
					symbolLookup.Add(name, symbol);
				}

				return symbol;
			}
		}

		public LinkerSymbol DefineSymbol(string name, SectionKind kind, int alignment, int size)
		{
			lock (_lock)
			{
				uint aligned = alignment != 0 ? (uint)alignment : 1;

				if (!symbolLookup.TryGetValue(name, out LinkerSymbol symbol))
				{
					symbol = new LinkerSymbol(name, aligned, kind);

					Symbols.Add(symbol);
					symbolLookup.Add(name, symbol);
					symbol.IsExport = false;
				}

				symbol.Alignment = aligned;
				symbol.SectionKind = kind;

				var stream = (size == 0) ? new MemoryStream() : new MemoryStream(size);
				symbol.Stream = Stream.Synchronized(stream);

				if (size != 0)
				{
					stream.SetLength(size);
				}

				return symbol;
			}
		}

		public LinkerSymbol DefineExternalSymbol(string name, string externalName, SectionKind kind)
		{
			lock (_lock)
			{
				if (!symbolLookup.TryGetValue(name, out LinkerSymbol symbol))
				{
					symbol = new LinkerSymbol(name, 0, kind);

					Symbols.Add(symbol);
					symbolLookup.Add(name, symbol);
				}

				symbol.SectionKind = kind;
				symbol.IsExport = true;
				symbol.ExportName = externalName;

				return symbol;
			}
		}

		public void SetFirst(LinkerSymbol symbol)
		{
			Symbols.Remove(symbol);
			Symbols.Insert(0, symbol);
		}

		private void FinalizeLayout()
		{
			LayoutObjectsAndSections();
			ApplyPatches();
		}

		private void LayoutObjectsAndSections()
		{
			ulong virtualAddress = BaseAddress;
			uint fileOffset = BaseFileOffset;

			foreach (var linkerSection in LinkerSections)
			{
				ResolveLayout(linkerSection, fileOffset, virtualAddress);

				uint size = linkerSection.AlignedSize;

				virtualAddress = linkerSection.VirtualAddress + size;
				fileOffset += size;
			}
		}

		private void ApplyPatches()
		{
			foreach (var symbol in Symbols)
			{
				foreach (var linkRequest in symbol.LinkRequests)
				{
					ApplyPatch(linkRequest);
				}
			}
		}

		private void ApplyPatch(LinkRequest linkRequest)
		{
			ulong value = 0;

			if (linkRequest.LinkType == LinkType.Size)
			{
				value = linkRequest.ReferenceSymbol.Size;
			}
			else
			{
				value = linkRequest.ReferenceSymbol.VirtualAddress + (ulong)linkRequest.ReferenceOffset;

				if (linkRequest.LinkType == LinkType.RelativeOffset)
				{
					// Change the absolute into a relative offset
					value -= (linkRequest.PatchSymbol.VirtualAddress + (ulong)linkRequest.PatchOffset);
				}
			}

			linkRequest.PatchSymbol.ApplyPatch(
				linkRequest.PatchOffset,
				value,
				GetPatchTypeSize(linkRequest.PatchType),
				Endianness
			);
		}

		private static byte GetPatchTypeSize(PatchType patchType)
		{
			switch (patchType)
			{
				case PatchType.I4: return 32;
				case PatchType.I8: return 64;
			}

			throw new CompilerException($"unknown patch type: {patchType}");
		}

		private void ResolveLayout(LinkerSection section, uint fileOffset, ulong virtualAddress)
		{
			section.VirtualAddress = virtualAddress;
			section.FileOffset = fileOffset;

			foreach (var symbol in Symbols)
			{
				if (symbol.SectionKind != section.SectionKind)
					continue;

				if (symbol.IsResolved)
					continue;

				if (symbol.IsExport)
					continue;

				section.Size = Alignment.AlignUp(section.Size, symbol.Alignment);

				symbol.SectionOffset = section.Size;
				symbol.VirtualAddress = section.VirtualAddress + section.Size;

				section.Size += symbol.Size;
			}

			section.IsResolved = true;
		}

		internal void WriteTo(Stream stream, LinkerSection section)
		{
			foreach (var symbol in Symbols)
			{
				if (symbol.SectionKind != section.SectionKind)
					continue;

				if (!symbol.IsResolved)
					continue;

				stream.Seek(section.FileOffset + symbol.SectionOffset, SeekOrigin.Begin);

				if (symbol.IsDataAvailable)
				{
					symbol.Stream.Position = 0;
					symbol.Stream.WriteTo(stream);
				}
			}

			stream.WriteZeroBytes((int)(section.FileOffset + section.AlignedSize - stream.Position));
		}

		#region Cache Methods

		public LinkerSymbol GetConstantSymbol(double value)
		{
			var data = BitConverter.GetBytes(value);

			string name = "$double$";

			foreach (byte b in data)
			{
				name += b.ToString("x");
			}

			var symbol = DefineSymbol(name, SectionKind.ROData, 1, 8);

			symbol.SetData(data);

			return symbol;
		}

		public LinkerSymbol GetConstantSymbol(float value)
		{
			var data = BitConverter.GetBytes(value);

			string name = "$float$";

			foreach (byte b in data)
			{
				name += b.ToString("x");
			}

			var symbol = DefineSymbol(name, SectionKind.ROData, 1, 4);

			symbol.SetData(data);

			return symbol;
		}

		public LinkerSymbol GetConstantSymbol(uint value)
		{
			string name = "$integer$" + value.ToString("x");

			var symbol = DefineSymbol(name, SectionKind.ROData, 1, 4);

			symbol.SetData(BitConverter.GetBytes(value));

			return symbol;
		}

		public LinkerSymbol GetConstantSymbol(ulong value)
		{
			string name = "$long$" + value.ToString("x");

			var symbol = DefineSymbol(name, SectionKind.ROData, 1, 8);

			symbol.SetData(BitConverter.GetBytes(value));

			return symbol;
		}

		public LinkerSymbol GetConstantSymbol(int value)
		{
			return GetConstantSymbol((uint)value);
		}

		public LinkerSymbol GetConstantSymbol(long value)
		{
			return GetConstantSymbol((ulong)value);
		}

		#endregion Cache Methods
	}
}
