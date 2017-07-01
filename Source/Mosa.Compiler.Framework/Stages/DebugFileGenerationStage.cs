// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// An compilation stage, which generates a map file of the built binary file.
	/// </summary>
	public sealed class DebugFileGenerationStage : BaseCompilerStage
	{
		#region Data members

		public string DebugFile { get; set; }

		/// <summary>
		/// Holds the text writer used to emit the map file.
		/// </summary>
		private TextWriter writer;

		#endregion Data members

		protected override void Setup()
		{
			DebugFile = CompilerOptions.DebugFile;
		}

		protected override void RunPostCompile()
		{
			if (string.IsNullOrEmpty(DebugFile))
				return;

			using (writer = new StreamWriter(DebugFile))
			{
				EmitSections();
				EmitSymbols();
				EmitEntryPoint();
			}
		}

		#region Internals

		private void EmitSections()
		{
			writer.WriteLine("[Sections]");
			writer.WriteLine("Address\tOffset\tSize\tKind\tName");
			foreach (var section in Linker.LinkerSections)
			{
				writer.WriteLine("{0:x16}\t{1:x16}\t{2:x16}\t{3}\t{4}", section.VirtualAddress, section.FileOffset, section.Size, section.SectionKind.ToString(), section.Name);
			}
		}

		private void EmitSymbols()
		{
			writer.WriteLine("[Symbols]");
			writer.WriteLine("Address\tOffset\tSize\tType\tName");

			foreach (var symbol in Linker.Symbols)
			{
				writer.WriteLine("{0:x16}\t{1:x16}\t{2:x16}\t{3}\t{4}", symbol.VirtualAddress, symbol.SectionOffset, symbol.Size, symbol.SectionKind.ToString(), symbol.Name);
			}
		}

		private void EmitEntryPoint()
		{
			if (Linker.EntryPoint != null)
			{
				writer.WriteLine("[EntryPoint]");
				writer.WriteLine("Address\tName");
				writer.WriteLine("{0}\t{1}", Linker.EntryPoint.VirtualAddress, Linker.EntryPoint.Name);
			}
		}

		#endregion Internals
	}
}
