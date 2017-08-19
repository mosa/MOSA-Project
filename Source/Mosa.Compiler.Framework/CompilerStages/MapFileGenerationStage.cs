// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;

namespace Mosa.Compiler.Framework.CompilerStages
{
	/// <summary>
	/// An compilation stage, which generates a map file of the built binary file.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public sealed class MapFileGenerationStage : BaseCompilerStage
	{
		#region Data members

		public string MapFile { get; set; }

		/// <summary>
		/// Holds the text writer used to emit the map file.
		/// </summary>
		private TextWriter writer;

		#endregion Data members

		protected override void Setup()
		{
			MapFile = CompilerOptions.MapFile;
		}

		protected override void RunPostCompile()
		{
			if (string.IsNullOrEmpty(MapFile))
				return;

			using (writer = new StreamWriter(MapFile))
			{
				// Emit map file header
				writer.WriteLine(CompilerOptions.OutputFile);
				writer.WriteLine();
				writer.WriteLine("Timestamp is {0}", DateTime.Now);
				writer.WriteLine();
				writer.WriteLine("Preferred load address is {0:x16}", (object)Linker.BaseAddress);
				writer.WriteLine();

				// Emit the sections
				EmitSections();
				writer.WriteLine();

				// Emit all symbols
				EmitSymbols();

				writer.Close();
			}
		}

		#region Internals

		/// <summary>
		/// Emits all the section created in the binary file.
		/// </summary>
		private void EmitSections()
		{
			writer.WriteLine("Offset           Virtual          Length           Name                             Class");
			foreach (var section in Linker.LinkerSections)
			{
				writer.WriteLine("{0:x16} {1:x16} {2:x16} {3} {4}", section.FileOffset, section.VirtualAddress, section.Size, section.Name.PadRight(32), section.SectionKind);
			}
		}

		/// <summary>
		/// Emits all symbols emitted in the binary file.
		/// </summary>
		private void EmitSymbols()
		{
			writer.WriteLine("Virtual          Offset           Length           Symbol");

			foreach (var section in Linker.LinkerSections)
			{
				foreach (var symbol in section.Symbols)
				{
					writer.WriteLine("{0:x16} {1:x16} {2:x16} {3} {4}", symbol.VirtualAddress, symbol.SectionOffset, symbol.Size, symbol.SectionKind.ToString().PadRight(7), symbol.Name);
				}
			}

			var entryPoint = Linker.EntryPoint;
			if (entryPoint != null)
			{
				writer.WriteLine();
				writer.WriteLine("Entry point is {0}", entryPoint.Name);
				writer.WriteLine("\tat Offset {0:x16}", entryPoint.SectionOffset); // TODO! add section offset too?
				writer.WriteLine("\tat virtual address {0:x16}", entryPoint.VirtualAddress);
			}
		}

		#endregion Internals
	}
}
