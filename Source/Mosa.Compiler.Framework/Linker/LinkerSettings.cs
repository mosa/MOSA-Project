// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;

namespace Mosa.Compiler.Framework.Linker;

/// <summary>
/// Compiler Options
/// </summary>
public class LinkerSettings
{
	#region Properties

	public Settings Settings { get; } = new Settings();

	public ulong BaseAddress => (ulong)Settings.GetValue("Compiler.BaseAddress", 0x00400000);

	public string OutputFile => Settings.GetValue("Compiler.OutputFile", "_kernel.bin");

	public string LinkerFormat => Settings.GetValue("Linker.Format", "elf32");

	public bool Binary => Settings.GetValue("Compiler.Binary", true);

	public bool Symbols => Settings.GetValue("Linker.Symbols", false);

	public bool StaticRelocations => Settings.GetValue("Linker.StaticRelocations", false);

	public bool ShortSymbolNames => Settings.GetValue("Linker.ShortSymbolNames", false);

	public bool Dwarf => Settings.GetValue("Linker.Dwarf", false);

	public bool Statistics => Settings.GetValue("CompilerDebug.Statistics", true);

	public int TraceLevel => Settings.GetValue("Compiler.TraceLevel", 0);

	public bool EmitInline => Settings.GetValue("Compiler.EmitInline", false);

	#endregion Properties

	public LinkerSettings(Settings settings)
	{
		Settings.Merge(settings);
	}
}
