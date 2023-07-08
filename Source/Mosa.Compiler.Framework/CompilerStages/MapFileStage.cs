// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;
using Mosa.Compiler.Framework.Linker;

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// An compilation stage, which generates a map file of the built binary file.
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public sealed class MapFileStage : BaseCompilerStage
{
	protected override void Initialization()
	{
	}

	protected override void Finalization()
	{
		if (string.IsNullOrEmpty(MosaSettings.MapFile))
			return;

		using (var writer = new StreamWriter(MosaSettings.MapFile))
		{
			// Emit map file header
			writer.WriteLine(MosaSettings.OutputFile);
			writer.WriteLine();
			writer.WriteLine($"Timestamp is {DateTime.Now}");
			writer.WriteLine();
			writer.WriteLine($"Preferred load address is {Linker.MosaSettings.BaseAddress:x16}");
			writer.WriteLine();

			// Emit the sections
			EmitSections(writer);
			writer.WriteLine();

			// Emit all symbols
			EmitSymbols(writer);

			writer.Close();
		}
	}

	#region Internals

	/// <summary>
	/// Emits all the section created in the binary file.
	/// </summary>
	private void EmitSections(TextWriter writer)
	{
		writer.WriteLine("Virtual          Length           Name");

		foreach (var linkerSection in Linker.Sections)
		{
			writer.WriteLine("{0:x16} {1:x16} {2}", linkerSection.VirtualAddress, linkerSection.Size, linkerSection.Name);
		}
	}

	/// <summary>
	/// Emits all symbols emitted in the binary file.
	/// </summary>
	private void EmitSymbols(TextWriter writer)
	{
		writer.WriteLine("Virtual          Offset           Length           Section Symbol");

		foreach (var kind in MosaLinker.SectionKinds)
		{
			foreach (var symbol in Linker.Symbols)
			{
				if (symbol.SectionKind != kind)
					continue;

				writer.WriteLine("{0:x16} {1:x16} {2:x16} {3} {4}", symbol.VirtualAddress, symbol.SectionOffset, symbol.Size, symbol.SectionKind.ToString().PadRight(8), symbol.Name);
			}
		}

		var entryPoint = Linker.EntryPoint;
		if (entryPoint != null)
		{
			writer.WriteLine();
			writer.WriteLine("Entry point is {0}", entryPoint.Name);

			//writer.WriteLine("\tat Offset {0:x16}", entryPoint.SectionOffset); // TODO! add section offset too?
			writer.WriteLine("\tat virtual address {0:x16}", entryPoint.VirtualAddress);
		}
	}

	#endregion Internals
}
