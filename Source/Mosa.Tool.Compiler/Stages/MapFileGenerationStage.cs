/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.IO;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;

namespace Mosa.Tool.Compiler.Stages
{
	/// <summary>
	/// An compilation stage, which generates a map file of the built binary file.
	/// </summary>
	public sealed class MapFileGenerationStage : BaseCompilerStage, ICompilerStage, IPipelineStage
	{
		#region Data members

		private ILinker linker;

		public string MapFile { get; set; }

		/// <summary>
		/// Holds the text writer used to emit the map file.
		/// </summary>
		private TextWriter writer;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MapFileGenerationStage"/> class.
		/// </summary>
		public MapFileGenerationStage()
		{
		}

		#endregion // Construction

		#region ICompilerStage Members

		void ICompilerStage.Setup(BaseCompiler compiler)
		{
			base.Setup(compiler);
			this.linker = RetrieveLinkerFromCompiler();
			this.MapFile = compiler.CompilerOptions.MapFile;
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void ICompilerStage.Run()
		{
			if (string.IsNullOrEmpty(MapFile))
				return;

			using (writer = new StreamWriter(MapFile))
			{
				// Emit map file header
				writer.WriteLine(linker.OutputFile);
				writer.WriteLine();
				writer.WriteLine("Timestamp is {0}", DateTime.Now);
				writer.WriteLine();
				writer.WriteLine("Preferred load address is {0:x16}", linker.BaseAddress);
				writer.WriteLine();

				// Emit the sections
				EmitSections(linker);
				writer.WriteLine();

				// Emit all symbols
				EmitSymbols(linker);

				writer.Close();
			}
		}

		#endregion // ICompilerStage Members

		#region Internals

		/// <summary>
		/// Emits all the section created in the binary file.
		/// </summary>
		/// <param name="linker">The linker.</param>
		private void EmitSections(ILinker linker)
		{
			writer.WriteLine("Offset           Virtual          Length           Name                             Class");
			foreach (LinkerSection section in linker.Sections)
			{
				writer.WriteLine("{0:x16} {1:x16} {2:x16} {3} {4}", section.Offset, section.VirtualAddress.ToInt64(), section.Length, section.Name.PadRight(32), section.SectionKind);
			}
		}

		private class LinkerSymbolComparerByVirtualAddress : IComparer<LinkerSymbol>
		{
			public int Compare(LinkerSymbol x, LinkerSymbol y)
			{
				return (int)(x.VirtualAddress.ToInt64() - y.VirtualAddress.ToInt64());
			}
		}

		/// <summary>
		/// Emits all symbols emitted in the binary file.
		/// </summary>
		/// <param name="linker">The linker.</param>
		private void EmitSymbols(ILinker linker)
		{
			List<LinkerSymbol> sorted = new List<LinkerSymbol>();

			foreach (LinkerSymbol symbol in linker.Symbols)
				sorted.Add(symbol);

			var comparer = new LinkerSymbolComparerByVirtualAddress();
			sorted.Sort(comparer);

			writer.WriteLine("Offset           Virtual          Length           Section Symbol");
			foreach (var symbol in sorted)
			{
				writer.WriteLine("{0:x16} {1:x16} {2:x16} {3} {4}", symbol.Offset, symbol.VirtualAddress.ToInt64(), symbol.Length, symbol.Section.ToString().PadRight(7), symbol.Name);
			}

			LinkerSymbol entryPoint = linker.EntryPoint;
			if (entryPoint != null)
			{
				writer.WriteLine();
				writer.WriteLine("Entry point is {0}", entryPoint.Name);
				writer.WriteLine("\tat offset {0:x16}", entryPoint.Offset);
				writer.WriteLine("\tat virtual address {0:x16}", entryPoint.VirtualAddress.ToInt64());
			}
		}

		#endregion // Internals
	}
}
