// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using System.IO;

namespace Mosa.Compiler.Framework.CompilerStages
{
	/// <summary>
	/// Finalizes the linking
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public sealed class LinkerEmitStage : BaseCompilerStage
	{
		protected override void Finalization()
		{
			if (!CompilerOptions.EmitBinary)
				return;

			if (string.IsNullOrEmpty(CompilerOptions.OutputFile))
				return;

			Compiler.PostCompilerTraceEvent(CompilerEvent.LinkingStart);

			File.Delete(CompilerOptions.OutputFile);

			using (var file = new FileStream(CompilerOptions.OutputFile, FileMode.Create))
			{
				Linker.Emit(file);
			}

			Compiler.PostCompilerTraceEvent(CompilerEvent.LinkingEnd);
		}
	}
}
