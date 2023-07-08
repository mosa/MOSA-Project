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

		Compiler.GlobalCounters.Update("Linker.Text", (int)Linker.Sections[(int)SectionKind.Text].Size);
		Compiler.GlobalCounters.Update("Linker.Data", (int)Linker.Sections[(int)SectionKind.Data].Size);
		Compiler.GlobalCounters.Update("Linker.ROData", (int)Linker.Sections[(int)SectionKind.ROData].Size);
		Compiler.GlobalCounters.Update("Linker.BSS", (int)Linker.Sections[(int)SectionKind.BSS].Size);
	}
}
