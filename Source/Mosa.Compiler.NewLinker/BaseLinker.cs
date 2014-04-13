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

		public string OutputFile { get; private set; }

		public Endianness Endianness { get; private set; }

		public uint MachineID { get; private set; }

		public long BaseAddress { get; private set; }

		public uint ObjectAlignment { get; private set; }	// previousily called SectionAlignment

		public uint SectionAlignment { get; private set; }	// previousily called LoadSectionAlignment

		public BaseLinker(Endianness endianness, ushort machineType)
		{
			LinkerSections = new LinkerSection[4];
			LinkRequests = new List<LinkRequest>();

			Endianness = endianness;
			MachineID = machineType;

			BaseAddress = 0x00400000; // Use the Win32 default for now, FIXME
			SectionAlignment = 0x1000; // default 1K
		}

		public void AddSection(LinkerSection linkerSection)
		{
			LinkerSections[(int)linkerSection.SectionKind] = linkerSection;
		}

		public void Initialize(string outputFile)
		{
			OutputFile = outputFile;
		}

		private void Link(LinkType linkType, PatchType patchType, LinkerObject patchSymbol, int patchOffset, int relativeBase, LinkerObject referenceSymbol, int referenceOffset)
		{
			var linkRequest = new LinkRequest(linkType, patchType, patchSymbol, patchOffset, relativeBase, referenceSymbol, referenceOffset);
			LinkRequests.Add(linkRequest);
		}

		public LinkerObject CreateObject(string name, SectionKind kind)
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

		//LayoutSections();
		//LayoutObjects();
		//EmitObjects();
	}
}