/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mosa.Compiler.NewLinker
{
	/// <summary>
	///
	/// </summary>
	public class LinkerObject
	{
		public string Name { get; private set; }
		public SectionKind SectionKind { get; private set; }

		public MemoryStream Stream { get; private set; }

		public bool IsAvailable { get { return Stream != null; } }
		public int Size { get { return Stream != null ? (int)Stream.Length : 0; } }

		public bool IsResolved { get; private set; }
		public int ResolvedSectionOffset { get; private set; }
		public ulong ResolvedVirtualAddress { get; private set; }

		public LinkerObject(string name, SectionKind sectionKind)
		{
			Name = name;
			SectionKind = sectionKind;
			IsResolved = false;
		}

		public void SetData(MemoryStream stream)
		{
			Stream = stream;
		}

		public void SetData(byte[] data)
		{
			Stream = new MemoryStream(data);
		}

		public void SetSectionOffset(int offset)
		{
			ResolvedSectionOffset = offset;
		}
	}
}