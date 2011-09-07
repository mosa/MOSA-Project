/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.IO;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Framework;

namespace Mosa.Tools.Compiler.Linker
{
	public class LinkerProxy : IAssemblyLinker, IAssemblyCompilerStage
	{
		private readonly IAssemblyLinker linker;

		public LinkerProxy(IAssemblyLinker linker)
		{
			this.linker = linker;
		}

		public long BaseAddress
		{
			get { return this.linker.BaseAddress; }
		}

		public LinkerSymbol EntryPoint
		{
			get { return this.linker.EntryPoint; }
			set { this.linker.EntryPoint = value; }
		}

		public long LoadSectionAlignment
		{
			get { return this.linker.LoadSectionAlignment; }
		}

		public ICollection<LinkerSection> Sections
		{
			get { return this.linker.Sections; }
		}

		public ICollection<LinkerSymbol> Symbols
		{
			get { return this.linker.Symbols; }
		}

		public string OutputFile
		{
			get { return this.linker.OutputFile; }
			set { this.linker.OutputFile = value; }
		}

		public long VirtualSectionAlignment
		{
			get { return this.linker.VirtualSectionAlignment; }
		}

		public Stream Allocate(string name, SectionKind section, int size, int alignment)
		{
			return this.linker.Allocate(name, section, size, alignment);
		}

		public LinkerSection GetSection(SectionKind sectionKind)
		{
			return this.linker.GetSection(sectionKind);
		}

		public LinkerSymbol GetSymbol(string symbolName)
		{
			return this.linker.GetSymbol(symbolName);
		}

		public bool HasSymbol(string symbolName)
		{
			return this.linker.HasSymbol(symbolName);
		}

		public long Link(LinkType linkType, string symbolName, int methodOffset, int methodRelativeBase, string targetSymbol, IntPtr offset)
		{
			return this.linker.Link(linkType, symbolName, methodOffset, methodRelativeBase, targetSymbol, offset);
		}

		public string Name
		{
			get
			{
				return @"LinkerProxy";
			}
		}

		public void Run()
		{
			// Nothing
		}

		public void Setup(AssemblyCompiler compiler)
		{
			// Nothing
		}
	}
}
