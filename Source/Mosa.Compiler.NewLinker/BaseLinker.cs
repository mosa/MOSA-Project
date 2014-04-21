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

		public List<LinkRequest> LinkRequests { get; private set; }

		public LinkerSymbol EntryPoint { get; set; }

		public Endianness Endianness { get; protected set; }

		public uint MachineID { get; private set; }

		public ulong BaseAddress { get; private set; }

		public uint FileAlignment { get; protected set; }

		public uint SectionAlignment { get; protected set; }

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
			LinkRequests = new List<LinkRequest>();

			Endianness = Common.Endianness.Little;
			MachineID = 0;

			BaseAddress = 0x00400000; // Use the Win32 default for now
			SectionAlignment = 0x1000; // default 1K
		}

		public virtual void Initialize(ulong baseAddress, Endianness endianness, ushort machineID)
		{
			BaseAddress = baseAddress;
			Endianness = endianness;
			MachineID = machineID;
		}

		protected void AddSection(LinkerSection section)
		{
			Sections[(int)section.SectionKind] = section;
		}

		public void Link(LinkType linkType, PatchType patchType, LinkerSymbol patchSymbol, int patchOffset, int relativeBase, LinkerSymbol referenceSymbol, int referenceOffset)
		{
			lock (mylock)
			{
				var linkRequest = new LinkRequest(linkType, patchType, patchSymbol, patchOffset, relativeBase, referenceSymbol, referenceOffset);
				LinkRequests.Add(linkRequest);
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

			MemoryStream stream = (size == 0) ? new MemoryStream() : new MemoryStream(size);
			symbol.Stream = stream;

			if (size != 0)
			{
				stream.SetLength(size);
			}

			// TODO! for debugging
			//GetSection(kind).Ordered.Add(symbol);

			return symbol;
		}

		public void FinalizeLayout()
		{
			LayoutObjectsAndSections();
			ApplyPatches();
		}

		private void LayoutObjectsAndSections()
		{
			// layout objects & sections
			ulong sectionOffset = 0;
			ulong virtualAddress = BaseAddress;

			foreach (var section in Sections)
			{
				section.ResolveLayout(sectionOffset, virtualAddress);

				ulong size = Alignment.Align(section.Size, SectionAlignment);

				sectionOffset = section.ResolvedSectionOffset + size;
				virtualAddress = section.ResolvedVirtualAddress + size;
			}
		}

		public abstract void Emit(Stream stream);

		private void ApplyPatches()
		{
			foreach (var linkRequest in LinkRequests)
			{
				ApplyPatch(linkRequest);
			}
		}

		private LinkerSection GetSection(SectionKind kind)
		{
			return Sections[(int)kind];
		}

		private void ApplyPatch(LinkRequest linkRequest)
		{
			ulong targetAddress = linkRequest.ReferenceSymbol.ResolvedVirtualAddress + (ulong)linkRequest.ReferenceOffset;

			if (linkRequest.LinkType == LinkType.AbsoluteAddress)
			{
				// FIXME: Need a .reloc section with a relocation entry if the module is moved in virtual memory
				// the runtime loader must patch this link request, we'll fail it until we can do relocations.
				//throw new NotSupportedException(@".reloc section not supported.");
			}
			else
			{
				// Change the absolute into a relative offset
				targetAddress = targetAddress - (linkRequest.PatchSymbol.ResolvedVirtualAddress + (ulong)linkRequest.PatchOffset);
			}


			targetAddress = targetAddress + (ulong)linkRequest.RelativeBase;

			ulong value = Patch.GetResult(linkRequest.PatchType.Patches, (ulong)targetAddress);
			ulong mask = Patch.GetFinalMask(linkRequest.PatchType.Patches);

			linkRequest.PatchSymbol.ApplyPatch(
				linkRequest.PatchOffset,
				value,
				mask,
				linkRequest.PatchType.Size,
				Endianness
			);
		}
	}
}