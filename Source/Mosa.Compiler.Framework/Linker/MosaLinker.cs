// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Linker.Elf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mosa.Compiler.Framework.Linker
{
	/// <summary>
	/// Base Linker
	/// </summary>
	public sealed class MosaLinker
	{
		public LinkerSettings LinkerSettings;

		public List<LinkerSymbol> Symbols { get; } = new List<LinkerSymbol>();

		private readonly Dictionary<string, LinkerSymbol> symbolLookup = new Dictionary<string, LinkerSymbol>();

		public LinkerSymbol EntryPoint { get; set; }

		private readonly ElfLinker ElfLinker;

		public uint SectionAlignment { get { return ElfLinker.SectionAlignment; } }

		public uint BaseFileOffset { get { return ElfLinker.BaseFileOffset; } }

		public LinkerFormatType LinkerFormatType { get; }

		public LinkerSection[] Sections { get; } = new LinkerSection[4];

		public static readonly SectionKind[] SectionKinds = new[] { SectionKind.Text, SectionKind.Data, SectionKind.ROData, SectionKind.BSS };

		private readonly object _lock = new object();
		private readonly object _cacheLock = new object();

		public Compiler Compiler { get; set; }

		public MosaLinker(Compiler compiler)
		{
			Compiler = compiler;

			LinkerSettings = new LinkerSettings(compiler.CompilerSettings.Settings);

			LinkerFormatType = LinkerSettings.LinkerFormat.ToLower() == "elf64" ? LinkerFormatType.Elf64 : LinkerFormatType.Elf32;

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

		public LinkerSymbol DefineSymbol(string name, SectionKind kind, int alignment, int size)
		{
			uint aligned = alignment != 0 ? (uint)alignment : 1;

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

				symbol.Stream = (size == 0) ? new MemoryStream() : new MemoryStream(size);

				if (size != 0)
				{
					symbol.Stream.SetLength(size);
				}

				return symbol;
			}
		}

		public void SetFirst(LinkerSymbol symbol)
		{
			Symbols.Remove(symbol);
			Symbols.Insert(0, symbol);
		}

		public void FinalizeLayout()
		{
			LayoutObjectsAndSections();

			ApplyPatches();
		}

		private void LayoutObjectsAndSections()
		{
			var virtualAddress = LinkerSettings.BaseAddress;

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
				Debug.Assert(linkRequest.ReferenceSymbol.VirtualAddress != 0);
				Debug.Assert(linkRequest.PatchSymbol.VirtualAddress != 0);

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
				GetPatchTypeSize(linkRequest.PatchType)
			);
		}

		private static byte GetPatchTypeSize(PatchType patchType)
		{
			switch (patchType)
			{
				case PatchType.I32: return 32;
				case PatchType.I64: return 64;
			}

			throw new CompilerException($"unknown patch type: {patchType}");
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
			var data = BitConverter.GetBytes(value);

			string name = "$double$";

			foreach (byte b in data)
			{
				name += b.ToString("x");
			}

			lock (_cacheLock)
			{
				var symbol = DefineSymbol(name, SectionKind.ROData, 1, 8);

				symbol.SetData(data);

				return symbol;
			}
		}

		public LinkerSymbol GetConstantSymbol(float value)
		{
			var data = BitConverter.GetBytes(value);

			string name = "$float$";

			foreach (byte b in data)
			{
				name += b.ToString("x");
			}

			lock (_cacheLock)
			{
				var symbol = DefineSymbol(name, SectionKind.ROData, 1, 4);

				symbol.SetData(data);

				return symbol;
			}
		}

		public LinkerSymbol GetConstantSymbol(uint value)
		{
			string name = "$integer$" + value.ToString("x");

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
			string name = "$long$" + value.ToString("x");

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
}
