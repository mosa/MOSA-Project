// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Linker.Elf;
using Mosa.Utility.Configuration;

namespace Mosa.Compiler.Framework.Linker;

/// <summary>
/// Base Linker
/// </summary>
public sealed class MosaLinker
{
	public MosaSettings MosaSettings;

	public List<LinkerSymbol> Symbols { get; } = new List<LinkerSymbol>();

	private readonly Dictionary<string, LinkerSymbol> symbolLookup = new Dictionary<string, LinkerSymbol>();

	public LinkerSymbol EntryPoint { get; set; }

	private readonly ElfLinker ElfLinker;

	private LinkerSymbol FirstSymbol { get; set; }

	public uint SectionAlignment => ElfLinker.SectionAlignment;

	public uint BaseFileOffset => ElfLinker.BaseFileOffset;

	public LinkerFormatType LinkerFormatType { get; }

	public LinkerSection[] Sections { get; } = new LinkerSection[4];

	public static readonly SectionKind[] SectionKinds = new[] { SectionKind.Text, SectionKind.Data, SectionKind.ROData, SectionKind.BSS };

	private readonly object _lock = new object();
	private readonly object _cacheLock = new object();

	public Compiler Compiler { get; set; }

	public MosaLinker(Compiler compiler)
	{
		Compiler = compiler;

		MosaSettings = compiler.MosaSettings;

		LinkerFormatType = MosaSettings.LinkerFormat == "elf64" ? LinkerFormatType.Elf64 : LinkerFormatType.Elf32;

		ElfLinker = new ElfLinker(this, LinkerFormatType, compiler.Architecture.ElfMachineType);

		AddSection(new LinkerSection(SectionKind.Text));
		AddSection(new LinkerSection(SectionKind.Data));
		AddSection(new LinkerSection(SectionKind.ROData));
		AddSection(new LinkerSection(SectionKind.BSS));
	}

	public void Emit(Stream stream)
	{
		ElfLinker.Emit(stream);
	}

	private void AddSection(LinkerSection linkerSection)
	{
		Sections[(int)linkerSection.SectionKind] = linkerSection;
	}

	public void Link(LinkType linkType, PatchType patchType, LinkerSymbol patchSymbol, long patchOffset, LinkerSymbol referenceSymbol, int referenceOffset)
	{
		var linkRequest = new LinkRequest(linkType, patchType, patchSymbol, (int)patchOffset, referenceSymbol, referenceOffset);

		lock (_lock)
		{
			patchSymbol.AddPatch(linkRequest);
		}
	}

	public void Link(LinkType linkType, PatchType patchType, string patchSymbolName, long patchOffset, string referenceSymbolName, int referenceOffset)
	{
		var referenceSymbol = GetSymbol(referenceSymbolName);
		var patchObject = GetSymbol(patchSymbolName);

		Link(linkType, patchType, patchObject, patchOffset, referenceSymbol, referenceOffset);
	}

	public void Link(LinkType linkType, PatchType patchType, LinkerSymbol patchSymbol, long patchOffset, string referenceSymbolName, int referenceOffset)
	{
		var referenceObject = GetSymbol(referenceSymbolName);

		Link(linkType, patchType, patchSymbol, patchOffset, referenceObject, referenceOffset);
	}

	public bool IsSymbolDefined(string name)
	{
		lock (_lock)
		{
			return symbolLookup.ContainsKey(name);
		}
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

	public LinkerSymbol DefineSymbol(string name, SectionKind kind, uint alignment, uint size)
	{
		var aligned = alignment != 0 ? alignment : 1;

		lock (_lock)
		{
			if (!symbolLookup.TryGetValue(name, out LinkerSymbol symbol))
			{
				symbol = new LinkerSymbol(name, aligned, kind);

				Symbols.Add(symbol);
				symbolLookup.Add(name, symbol);
				symbol.IsExternalSymbol = false;
			}

			symbol.Alignment = aligned;
			symbol.SectionKind = kind;

			symbol.Stream = size == 0 ? new MemoryStream() : new MemoryStream((int)size);

			if (size != 0)
			{
				symbol.Stream.SetLength(size);
			}

			return symbol;
		}
	}

	public void SetFirst(LinkerSymbol symbol)
	{
		FirstSymbol = symbol;
	}

	public void FinalizeLayout()
	{
		LayoutObjectsAndSections();

		ApplyPatches();
	}

	private void LayoutObjectsAndSections()
	{
		var virtualAddress = MosaSettings.BaseAddress;

		// Sort the list --- helpful for debugging
		Symbols.Sort((y, x) => x.Name.CompareTo(y.Name));

		if (FirstSymbol != null)
		{
			Symbols.Remove(FirstSymbol);
			Symbols.Insert(0, FirstSymbol);
		}

		foreach (var section in SectionKinds)
		{
			var size = ResolveSymbolLocation(section, virtualAddress);

			var linkerSection = Sections[(int)section];

			linkerSection.VirtualAddress = virtualAddress;
			linkerSection.Size = size;

			size = Alignment.AlignUp(size, SectionAlignment);

			virtualAddress += size;
		}
	}

	private void ApplyPatches()
	{
		foreach (var symbol in Symbols)
		{
			if (symbol.IsReplaced)
				continue;

			foreach (var linkRequest in symbol.LinkRequests)
			{
				ApplyPatch(linkRequest);
			}
		}
	}

	private void ApplyPatch(LinkRequest linkRequest)
	{
		ulong value;

		if (linkRequest.LinkType == LinkType.Size)
		{
			value = linkRequest.ReferenceSymbol.Size;
		}
		else
		{
			if (linkRequest.ReferenceSymbol.VirtualAddress == 0)
				throw new CompilerException($"Linker: ReferenceSymbol not resolved: {linkRequest.ReferenceSymbol}");

			if (linkRequest.PatchSymbol.VirtualAddress == 0)
				throw new CompilerException($"Linker: PatchSymbol not resolved: {linkRequest.PatchSymbol}");

			Debug.Assert(linkRequest.ReferenceSymbol.VirtualAddress != 0);
			Debug.Assert(linkRequest.PatchSymbol.VirtualAddress != 0);

			value = linkRequest.ReferenceSymbol.VirtualAddress + (ulong)linkRequest.ReferenceOffset;

			if (linkRequest.LinkType == LinkType.RelativeOffset)
			{
				// Change the absolute into a relative offset
				value -= linkRequest.PatchSymbol.VirtualAddress + (ulong)linkRequest.PatchOffset;
			}
		}

		linkRequest.PatchSymbol.ApplyPatch(
			linkRequest.PatchOffset,
			value,
			GetPatchTypeSize(linkRequest.PatchType),
			GetPatchTypeShift(linkRequest.PatchType)
		);
	}

	private static byte GetPatchTypeSize(PatchType patchType)
	{
		return patchType switch
		{
			PatchType.I32 => 32,
			PatchType.I64 => 64,
			PatchType.I24o8 => 24,
			_ => throw new CompilerException($"unknown patch type: {patchType}")
		};
	}

	private static byte GetPatchTypeShift(PatchType patchType)
	{
		return patchType switch
		{
			PatchType.I32 => 0,
			PatchType.I64 => 0,
			PatchType.I24o8 => 8,
			_ => throw new CompilerException($"unknown patch type: {patchType}")
		};
	}

	private uint ResolveSymbolLocation(SectionKind section, ulong VirtualAddress)
	{
		uint position = 0;

		foreach (var symbol in Symbols)
		{
			if (symbol.IsReplaced)
				continue;

			if (symbol.SectionKind != section)
				continue;

			if (symbol.IsResolved)
				continue;

			symbol.SectionOffset = position;
			symbol.VirtualAddress = VirtualAddress + position;

			position += symbol.Size;
		}

		return position;
	}

	#region Cache Methods

	public LinkerSymbol GetConstantSymbol(double value)
	{
		return GetConstantSymbol(BitConverter.DoubleToInt64Bits(value));
	}

	public LinkerSymbol GetConstantSymbol(float value)
	{
		return GetConstantSymbol(BitConverter.SingleToInt32Bits(value));
	}

	public LinkerSymbol GetConstantSymbol(uint value)
	{
		var name = $"$const32${value:x}";

		var data = BitConverter.GetBytes(value);

		lock (_cacheLock)
		{
			var symbol = DefineSymbol(name, SectionKind.ROData, 1, 4);

			symbol.SetData(data);

			return symbol;
		}
	}

	public LinkerSymbol GetConstantSymbol(ulong value)
	{
		var name = $"$const64${value:x}";

		var data = BitConverter.GetBytes(value);

		lock (_cacheLock)
		{
			var symbol = DefineSymbol(name, SectionKind.ROData, 1, 8);

			symbol.SetData(data);

			return symbol;
		}
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
