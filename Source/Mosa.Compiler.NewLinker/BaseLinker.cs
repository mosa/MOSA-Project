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
		public LinkerSection[] LinkerSections { get; private set; }

		public List<LinkRequest> LinkRequests { get; private set; }

		public LinkerObject EntryPoint { get; set; }

		public Endianness Endianness { get; protected set; }

		public uint MachineID { get; private set; }

		public ulong BaseAddress { get; private set; }

		public uint FileAlignment { get; protected set; }

		public uint SectionAlignment { get; protected set; }

		private object mylock = new object();

		protected BaseLinker()
		{
			LinkerSections = new LinkerSection[4];
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

		protected void AddSection(LinkerSection linkerSection)
		{
			LinkerSections[(int)linkerSection.SectionKind] = linkerSection;
		}

		public void Link(LinkType linkType, PatchType patchType, LinkerObject patchSymbol, int patchOffset, int relativeBase, LinkerObject referenceSymbol, int referenceOffset)
		{
			lock (mylock)
			{
				var linkRequest = new LinkRequest(linkType, patchType, patchSymbol, patchOffset, relativeBase, referenceSymbol, referenceOffset);
				LinkRequests.Add(linkRequest);
			}
		}

		public void Link(LinkType linkType, PatchType patchType, string patchSymbol, SectionKind patchKind, int patchOffset, int relativeBase, string referenceSymbol, SectionKind referenceKind, int referenceOffset)
		{
			var referenceObject = GetLinkerObject(referenceSymbol, referenceKind);
			var patchObject = GetLinkerObject(patchSymbol, patchKind);

			Link(linkType, patchType, patchObject, patchOffset, relativeBase, referenceObject, referenceOffset);
		}

		public void Link(LinkType linkType, PatchType patchType, LinkerObject patchSymbol, int patchOffset, int relativeBase, string referenceSymbol, SectionKind referenceKind, int referenceOffset)
		{
			var referenceObject = GetLinkerObject(referenceSymbol, referenceKind);

			Link(linkType, patchType, patchSymbol, patchOffset, relativeBase, referenceObject, referenceOffset);
		}

		public LinkerObject CreateLinkerObject(string name, SectionKind kind, uint alignment)
		{
			lock (mylock)
			{
				var section = LinkerSections[(int)kind];

				Debug.Assert(section != null);

				var linkerObject = section.GetLinkerObject(name);

				if (linkerObject == null)
				{
					linkerObject = new LinkerObject(name, kind, alignment);

					section.AddLinkerObject(linkerObject);
				}

				linkerObject.Alignment = alignment != 0 ? alignment : 0;

				return linkerObject;
			}
		}

		public LinkerObject AllocateLinkerObject(string name, SectionKind kind, int size, int alignment)
		{
			var linkObject = CreateLinkerObject(name, kind, (uint)alignment);

			MemoryStream stream = (size == 0) ? new MemoryStream() : new MemoryStream(size);
			linkObject.SetData(stream);

			if (size != 0)
			{
				stream.SetLength(size);
			}

			return linkObject;
		}

		public Stream Allocate(string name, SectionKind kind, int size, int alignment)
		{
			var linkObject = AllocateLinkerObject(name, kind, size, alignment);

			return linkObject.Stream;
		}

		public LinkerObject GetLinkerObject(string name, SectionKind kind)
		{
			return CreateLinkerObject(name, kind, 0);
		}

		public void Finalize()
		{
			LayoutObjectsAndSections();
			ApplyPatches();
		}

		private void LayoutObjectsAndSections()
		{
			// layout objects & sections
			ulong sectionOffset = 0;
			ulong virtualAddress = BaseAddress;

			foreach (var section in LinkerSections)
			{
				section.ResolveLayout(sectionOffset, virtualAddress);

				sectionOffset = section.ResolvedSectionOffset + section.Size;
				virtualAddress = section.ResolvedVirtualAddress + section.Size;

				Debug.Assert(Alignment.Align(sectionOffset, SectionAlignment) == sectionOffset);
			}

		}

		public abstract void CreateFile(Stream stream);

		private void ApplyPatches()
		{
			foreach (var linkRequest in LinkRequests)
			{
				ApplyPatch(linkRequest);
			}
		}

		private LinkerSection GetSection(SectionKind kind)
		{
			return LinkerSections[(int)kind];
		}

		private void ApplyPatch(LinkRequest linkRequest)
		{
			//var patchSection = GetSection(linkRequest.PatchObject.SectionKind);
			//var referenceSection = GetSection(linkRequest.ReferenceObject.SectionKind);

			ulong targetAddress = linkRequest.ReferenceObject.ResolvedVirtualAddress + (ulong)linkRequest.ReferenceOffset;

			if (linkRequest.LinkType == LinkType.AbsoluteAddress)
			{
				// FIXME: Need a .reloc section with a relocation entry if the module is moved in virtual memory
				// the runtime loader must patch this link request, we'll fail it until we can do relocations.
				//throw new NotSupportedException(@".reloc section not supported.");
			}
			else
			{
				// Change the absolute into a relative offset
				targetAddress = targetAddress - linkRequest.PatchObject.ResolvedVirtualAddress - (ulong)linkRequest.PatchOffset;
			}

			ulong value = Patch.GetResult(linkRequest.PatchType.Patches, (ulong)targetAddress);
			ulong mask = Patch.GetFinalMask(linkRequest.PatchType.Patches);

			linkRequest.PatchObject.ApplyPatch(
				linkRequest.PatchOffset,
				value,
				mask,
				linkRequest.PatchType.Size,
				Endianness
			);
		}
	}
}