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
using System.Text;
using System.IO;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;
using Mosa.Compiler.Linker;

namespace Mosa.Tools.Compiler.Linker
{
	/// <summary>
	/// An assembly compilation stage, which generates a map file of the built binary file.
	/// </summary>
	public sealed class MapFileGenerationStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, IPipelineStage
	{
		#region Data members

		private IAssemblyLinker linker;

		/// <summary>
		/// Holds the text writer used to emit the map file.
		/// </summary>
		private TextWriter writer;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MapFileGenerationStage"/> class.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public MapFileGenerationStage()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MapFileGenerationStage"/> class.
		/// </summary>
		public MapFileGenerationStage(TextWriter writer)
		{
			if (writer == null)
				throw new ArgumentNullException(@"writer");

			this.writer = writer;
		}

		#endregion // Construction

		#region IPipelineStage members

		string IPipelineStage.Name { get { return @"MapFileGenerationStage"; } }

		#endregion // IPipelineStage members

		#region IAssemblyCompilerStage Members

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);

			linker = RetrieveAssemblyLinkerFromCompiler();
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IAssemblyCompilerStage.Run()
		{
			// Emit map file _header
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
		}

		#endregion // IAssemblyCompilerStage Members

		#region Internals

		/// <summary>
		/// Emits all the section created in the binary file.
		/// </summary>
		/// <param name="linker">The assembly linker.</param>
		private void EmitSections(IAssemblyLinker linker)
		{
			writer.WriteLine("Offset           Virtual          Length           Name                             Class");
			foreach (LinkerSection section in linker.Sections)
			{
				writer.WriteLine("{0:x16} {1:x16} {2:x16} {3} {4}", section.Offset, section.VirtualAddress.ToInt64(), section.Length, section.Name.PadRight(32), section.SectionKind);
			}
		}

		/// <summary>
		/// Emits all symbols emitted in the binary file.
		/// </summary>
		/// <param name="linker">The assembly linker.</param>
		private void EmitSymbols(IAssemblyLinker linker)
		{
			writer.WriteLine("Offset           Virtual          Length           Symbol");
			foreach (LinkerSymbol symbol in linker.Symbols)
				writer.WriteLine("{0:x16} {1:x16} {2:x16} {3}", symbol.Offset, symbol.VirtualAddress.ToInt64(), symbol.Length, symbol.Name);

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
