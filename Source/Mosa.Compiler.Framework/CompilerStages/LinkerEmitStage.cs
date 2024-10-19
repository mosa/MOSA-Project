// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// Finalizes the linking
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public sealed class LinkerEmitStage : BaseCompilerStage
{
	protected override void Finalization()
	{
		if (!MosaSettings.EmitBinary)
			return;

		if (string.IsNullOrEmpty(MosaSettings.OutputFile))
			return;

		Compiler.PostEvent(CompilerEvent.LinkingStart);

		using (var file = new FileStream(MosaSettings.OutputFile, FileMode.Create))
		{
			Linker.Emit(file);
		}

		Compiler.LinkerTime.Stop();

		Compiler.PostEvent(CompilerEvent.LinkingEnd);
	}
}
