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

namespace Mosa.Compiler.NewLinker
{
	/// <summary>
	///
	/// </summary>
	public class BaseLinker
	{
		public LinkerSection[] LinkerSections { get; private set; }

		public List<LinkRequest> LinkRequests { get; private set; }

		public LinkerObject EntryPoint { get; set; }

		public Endianness Endianness { get; private set; }

		public uint MachineID { get; private set; }

		public ulong BaseAddress { get; private set; }

		public uint FileAlignment { get; protected set; }

		public uint SectionAlignment { get; protected set; }

		private object mylock = new object();

		protected BaseLinker(Endianness endianness, ushort machineType)
		{
			LinkerSections = new LinkerSection[4];
			LinkRequests = new List<LinkRequest>();

			Endianness = endianness;
			MachineID = machineType;

			BaseAddress = 0x00400000; // Use the Win32 default for now, FIXME
			SectionAlignment = 0x1000; // default 1K
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

		public LinkerObject CreateObject(string name, SectionKind kind)
		{
			lock (mylock)
			{
				var section = LinkerSections[(int)kind];

				Debug.Assert(section != null);

				var linkerObject = section.GetLinkerObject(name);

				if (linkerObject == null)
				{
					linkerObject = new LinkerObject(name, kind);

					section.AddLinkerObject(linkerObject);
				}

				return linkerObject;
			}
		}

		private void LayoutObjects()
		{
			foreach (var section in LinkerSections)
			{
				section.ResolveLayout();
			}
		}

		//private LayoutSections();
		//private EmitObjects();

		public bool Emit(Stream stream)
		{
			return false;
		}

		public void ApplyPatches()
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