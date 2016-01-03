// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	///
	/// </summary>
	public abstract class BaseLinker
	{
		public LinkerSection[] Sections { get; private set; }

		public LinkerSymbol EntryPoint { get; set; }

		public Endianness Endianness { get; protected set; }

		public uint MachineID { get; private set; }

		public ulong BaseAddress { get; private set; }

		public uint SectionAlignment { get; protected set; }

		public uint BaseFileOffset { get; set; }

		public bool EmitSymbols { get; set; }

		private object mylock = new object();

		public IEnumerable<LinkerSymbol> Symbols
		{
			get
			{
				foreach (var section in Sections)
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

		protected BaseLinker()
		{
			Sections = new LinkerSection[4];
			BaseFileOffset = 0;
			SectionAlignment = 0x1000; // default 1K

			// defaults
			BaseAddress = 0x00400000;
			Endianness = Common.Endianness.Little;
			MachineID = 0;
			EmitSymbols = true;
		}

		public virtual void Initialize(ulong baseAddress, Endianness endianness, ushort machineID, bool emitSymbols)
		{
			BaseAddress = baseAddress;
			Endianness = endianness;
			MachineID = machineID;
			EmitSymbols = emitSymbols;

			AddSection(new LinkerSection(SectionKind.Text, SectionAlignment));
			AddSection(new LinkerSection(SectionKind.Data, SectionAlignment));
			AddSection(new LinkerSection(SectionKind.ROData, SectionAlignment));
			AddSection(new LinkerSection(SectionKind.BSS, SectionAlignment));
		}

		private void AddSection(LinkerSection section)
		{
			Sections[(int)section.SectionKind] = section;
		}

		public void Link(LinkType linkType, PatchType patchType, LinkerSymbol patchSymbol, int patchOffset, int relativeBase, LinkerSymbol referenceSymbol, int referenceOffset)
		{
			lock (mylock)
			{
				var linkRequest = new LinkRequest(linkType, patchType, patchSymbol, patchOffset, relativeBase, referenceSymbol, referenceOffset);

				patchSymbol.AddPatch(linkRequest);
			}
		}

		public void Link(LinkType linkType, PatchType patchType, string patchSymbol, SectionKind patchKind, int patchOffset, int relativeBase, string referenceSymbol, SectionKind referenceKind, int referenceOffset)
		{
			var referenceObject = GetSymbol(referenceSymbol, referenceKind);
			var patchObject = GetSymbol(patchSymbol, patchKind);

			Link(linkType, patchType, patchObject, patchOffset, relativeBase, referenceObject, referenceOffset);
		}

		public void Link(LinkType linkType, PatchType patchType, LinkerSymbol patchSymbol, int patchOffset, int relativeBase, string referenceSymbol, SectionKind patchRelativeBase, int referenceOffset)
		{
			var referenceObject = GetSymbol(referenceSymbol, patchRelativeBase);

			Link(linkType, patchType, patchSymbol, patchOffset, relativeBase, referenceObject, referenceOffset);
		}

		public LinkerSymbol GetSymbol(string name, SectionKind kind)
		{
			return CreateSymbol(name, kind, 0);
		}

		private static SectionKind[] SectionList = new[] { SectionKind.BSS, SectionKind.Data, SectionKind.ROData, SectionKind.Text };

		public LinkerSymbol FindSymbol(string name)
		{
			foreach (var kind in SectionList)
			{
				var section = Sections[(int)kind];

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
				var section = Sections[(int)kind];

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

			foreach (var section in Sections)
			{
				section.ResolveLayout(fileOffset, virtualAddress);

				uint size = section.AlignedSize;

				virtualAddress = section.VirtualAddress + size;
				fileOffset = fileOffset + size;
			}
		}

		protected abstract void EmitImplementation(Stream stream);

		public void Emit(Stream stream)
		{
			FinalizeLayout();

			EmitImplementation(stream);
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

		protected LinkerSection GetSection(SectionKind kind)
		{
			return Sections[(int)kind];
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

				value = Patch.GetResult(linkRequest.PatchType.Patches, (ulong)targetAddress);
			}

			ulong mask = Patch.GetFinalMask(linkRequest.PatchType.Patches);

			linkRequest.PatchSymbol.ApplyPatch(
				linkRequest.PatchOffset,
				value,
				mask,
				linkRequest.PatchType.Size,
				Endianness
			);
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
