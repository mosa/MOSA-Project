// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using Mosa.Compiler.Framework.Trace;

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

		File.Delete(MosaSettings.OutputFile);

		using (var file = new FileStream(MosaSettings.OutputFile, FileMode.Create))
		{
			Linker.Emit(file);
		}

		Compiler.PostEvent(CompilerEvent.LinkingEnd);
	}
}
