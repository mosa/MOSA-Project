// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// Linker Layout Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public sealed class LinkerLayoutStage : BaseCompilerStage
{
	protected override void Finalization()
	{
		if (string.IsNullOrEmpty(MosaSettings.OutputFile))
			return;

		Linker.FinalizeLayout();

		Compiler.LinkerTime.Reset();

		Compiler.GlobalCounters.Set("Linker.Text", (int)Linker.Sections[(int)SectionKind.Text].Size);
		Compiler.GlobalCounters.Set("Linker.Data", (int)Linker.Sections[(int)SectionKind.Data].Size);
		Compiler.GlobalCounters.Set("Linker.ROData", (int)Linker.Sections[(int)SectionKind.ROData].Size);
		Compiler.GlobalCounters.Set("Linker.BSS", (int)Linker.Sections[(int)SectionKind.BSS].Size);
	}
}
