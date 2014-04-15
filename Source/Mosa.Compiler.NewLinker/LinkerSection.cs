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

namespace Mosa.Compiler.NewLinker
{
	/// <summary>
	///
	/// </summary>
	public class LinkerSection
	{
		private List<LinkerObject> linkerObjects;

		private Dictionary<string, LinkerObject> linkerObjectLookup;

		public string Name { get; private set; }

		public SectionKind SectionKind { get; private set; }

		public uint SectionAlignment { get; private set; }

		public ulong BaseVirtualAddress { get; private set; }

		public bool IsResolved { get; private set; }

		public ulong ResolvedSectionOffset { get; private set; }

		public ulong ResolvedVirtualAddress { get; private set; }

		public long Size { get; private set; }

		private object mylock = new object();

		public LinkerSection(SectionKind sectionKind, string name, uint alignment, ulong baseVirtualAddress)
		{
			Name = name;
			SectionKind = sectionKind;
			BaseVirtualAddress = baseVirtualAddress;
			IsResolved = false;
			linkerObjectLookup = new Dictionary<string, LinkerObject>();
			linkerObjects = new List<LinkerObject>();
			SectionAlignment = alignment;
			Size = 0;
		}

		internal void AddLinkerObject(LinkerObject linkerObject)
		{
			linkerObjects.Add(linkerObject);
			linkerObjectLookup.Add(linkerObject.Name, linkerObject);
		}

		internal LinkerObject GetLinkerObject(string name)
		{
			LinkerObject linkerObject = null;

			linkerObjectLookup.TryGetValue(name, out linkerObject);

			return null;
		}

		internal void ResolveLayout()
		{
			foreach (var linkerObject in linkerObjects)
			{
				if (linkerObject.IsResolved)
					continue;

				Size = Alignment.Align(Size, linkerObject.Alignment);

				linkerObject.ResolvedSectionOffset = Size;

				Size = Size + linkerObject.Size;
			}

			Size = Alignment.Align(Size, SectionAlignment);

			IsResolved = true;
		}

	}
}