// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Linker.Elf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	///
	/// </summary>
	public class BaseLinker
	{
		public LinkerSection[] LinkerSections { get; private set; }

		public LinkerSymbol EntryPoint { get; set; }

		public Endianness Endianness { get; protected set; }

		public MachineType MachineType { get; private set; }

		public ulong BaseAddress { get; private set; }

		public uint SectionAlignment { get; set; }

		public uint BaseFileOffset { get; set; }

		public bool EmitSymbols { get; set; }

		public LinkerFormatType LinkerFormatType { get; private set; }

		private readonly ElfLinker elfLinker;

		private object mylock = new object();

		private static SectionKind[] SectionList = new[] { SectionKind.BSS, SectionKind.Data, SectionKind.ROData, SectionKind.Text };

		public IEnumerable<LinkerSymbol> Symbols
		{
			get
			{
				foreach (var section in LinkerSections)
				{
					if (section == null)
						continue;

					foreach (var symbol in section.Symbols)
					{
						yield return symbol;
					}
				}
			}
		}

		public BaseLinker(ulong baseAddress, Endianness endianness, MachineType machineType, bool emitSymbols, LinkerFormatType linkerFormatType)
		{
			LinkerSections = new LinkerSection[4];

			BaseAddress = baseAddress;
			Endianness = endianness;
			MachineType = machineType;
			EmitSymbols = emitSymbols;

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

		private void AddSection(LinkerSection section)
		{
			LinkerSections[(int)section.SectionKind] = section;
		}

		public void Link(LinkType linkType, PatchType patchType, LinkerSymbol patchSymbol, int patchOffset, int relativeBase, LinkerSymbol referenceSymbol, int referenceOffset)
		{
			lock (mylock)
			{
				var linkRequest = new LinkRequest(linkType, patchType, patchSymbol, patchOffset, relativeBase, referenceSymbol, referenceOffset);

				patchSymbol.AddPatch(linkRequest);
			}
		}

		public void Link(LinkType linkType, PatchType patchType, string patchSymbolName, SectionKind patchKind, int patchOffset, int relativeBase, string referenceSymbolName, SectionKind referenceKind, int referenceOffset)
		{
			var referenceSymbol = GetSymbol(referenceSymbolName, referenceKind);
			var patchObject = GetSymbol(patchSymbolName, patchKind);

			Link(linkType, patchType, patchObject, patchOffset, relativeBase, referenceSymbol, referenceOffset);
		}

		public void Link(LinkType linkType, PatchType patchType, string patchSymbolName, SectionKind patchKind, int patchOffset, int relativeBase, LinkerSymbol referenceSymbol, int referenceOffset)
		{
			var patchObject = GetSymbol(patchSymbolName, patchKind);

			Link(linkType, patchType, patchObject, patchOffset, relativeBase, referenceSymbol, referenceOffset);
		}

		public void Link(LinkType linkType, PatchType patchType, LinkerSymbol patchSymbol, int patchOffset, int relativeBase, string referenceSymbolName, SectionKind referenceKind, int referenceOffset)
		{
			var referenceObject = GetSymbol(referenceSymbolName, referenceKind);

			Link(linkType, patchType, patchSymbol, patchOffset, relativeBase, referenceObject, referenceOffset);
		}

		public LinkerSymbol GetSymbol(string name, SectionKind kind)
		{
			return CreateSymbol(name, kind, 0);
		}

		public LinkerSymbol FindSymbol(string name, SectionKind kind)
		{
			var section = LinkerSections[(int)kind];

			Debug.Assert(section != null);

			return section.GetSymbol(name);
		}

		public LinkerSymbol FindSymbol(string name)
		{
			foreach (var kind in SectionList)
			{
				var section = LinkerSections[(int)kind];

				Debug.Assert(section != null);

				var symbol = section.GetSymbol(name);

				if (symbol != null)
					return symbol;
			}

			return null;
		}

		protected LinkerSymbol CreateSymbol(string name, SectionKind kind, uint alignment)
		{
			lock (mylock)
			{
				var section = LinkerSections[(int)kind];

				Debug.Assert(section != null);

				var symbol = section.GetSymbol(name);

				if (symbol == null)
				{
					symbol = new LinkerSymbol(name, kind, alignment);

					section.AddLinkerObject(symbol);
				}

				symbol.Alignment = alignment != 0 ? alignment : 0;

				return symbol;
			}
		}

		public LinkerSymbol CreateSymbol(string name, SectionKind kind, int alignment, int size)
		{
			var symbol = CreateSymbol(name, kind, (uint)alignment);

			var stream = (size == 0) ? new MemoryStream() : new MemoryStream(size);

			symbol.Stream = Stream.Synchronized(stream);

			if (size != 0)
			{
				stream.SetLength(size);
			}

			return symbol;
		}

		private void FinalizeLayout()
		{
			LayoutObjectsAndSections();
			ApplyPatches();
		}

		private void LayoutObjectsAndSections()
		{
			// layout objects & sections
			ulong virtualAddress = BaseAddress;
			uint fileOffset = BaseFileOffset;

			foreach (var section in LinkerSections)
			{
				section.ResolveLayout(fileOffset, virtualAddress);

				uint size = section.AlignedSize;

				virtualAddress = section.VirtualAddress + size;
				fileOffset = fileOffset + size;
			}
		}

		private void ApplyPatches()
		{
			foreach (var symbol in Symbols)
			{
				symbol.PreHash = symbol.ComputeMD5Hash();

				foreach (var linkRequest in symbol.LinkRequests)
				{
					ApplyPatch(linkRequest);
				}

				symbol.PostHash = symbol.ComputeMD5Hash();
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
				ulong targetAddress = linkRequest.ReferenceSymbol.VirtualAddress + (ulong)linkRequest.ReferenceOffset;

				if (linkRequest.LinkType == LinkType.AbsoluteAddress)
				{
					// FIXME: Need a .reloc section with a relocation entry if the module is moved in virtual memory
					// the runtime loader must patch this link request, we'll fail it until we can do relocations.
					//throw new NotSupportedException(@".reloc section not supported.");
				}
				else
				{
					// Change the absolute into a relative offset
					targetAddress = targetAddress - (linkRequest.PatchSymbol.VirtualAddress + (ulong)linkRequest.PatchOffset);
				}

				targetAddress = targetAddress + (ulong)linkRequest.RelativeBase;

				//value = Patch.GetResult(linkRequest.PatchType.Patches, targetAddress);
				value = targetAddress;
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
				default:
					throw new CompilerException("unknown patch type: " + patchType.ToString());
			}
		}

		#region Cache Methods

		public LinkerSymbol GetConstantSymbol(double value)
		{
			var data = BitConverter.GetBytes(value);

			string name = "$double$";

			foreach (byte b in data)
			{
				name = name + b.ToString("x");
			}

			var symbol = GetSymbol(name, SectionKind.ROData);

			if (!symbol.IsDataAvailable)
			{
				symbol.SetData(data);
			}

			return symbol;
		}

		public LinkerSymbol GetConstantSymbol(float value)
		{
			var data = BitConverter.GetBytes(value);

			string name = "$float$";

			foreach (byte b in data)
			{
				name = name + b.ToString("x");
			}

			var symbol = GetSymbol(name, SectionKind.ROData);

			if (!symbol.IsDataAvailable)
			{
				symbol.SetData(data);
			}

			return symbol;
		}

		public LinkerSymbol GetConstantSymbol(uint value)
		{
			string name = "$integer$" + value.ToString("x");

			var symbol = GetSymbol(name, SectionKind.ROData);

			if (!symbol.IsDataAvailable)
			{
				symbol.SetData(BitConverter.GetBytes(value));
			}

			return symbol;
		}

		public LinkerSymbol GetConstantSymbol(ulong value)
		{
			string name = "$long$" + value.ToString("x");

			var symbol = GetSymbol(name, SectionKind.ROData);

			if (!symbol.IsDataAvailable)
			{
				symbol.SetData(BitConverter.GetBytes(value));
			}

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
