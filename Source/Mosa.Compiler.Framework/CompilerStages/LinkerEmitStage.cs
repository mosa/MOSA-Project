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
			if (!CompilerSettings.EmitBinary)
				return;

			if (string.IsNullOrEmpty(CompilerSettings.OutputFile))
				return;

			Compiler.PostEvent(CompilerEvent.LinkingStart);

			File.Delete(CompilerSettings.OutputFile);

			using (var file = new FileStream(CompilerSettings.OutputFile, FileMode.Create))
			{
				Linker.Emit(file);
			}

			Compiler.PostEvent(CompilerEvent.LinkingEnd);
		}
	}
}
