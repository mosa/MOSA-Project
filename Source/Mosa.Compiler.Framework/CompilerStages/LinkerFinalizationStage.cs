// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Framework.CompilerStages
{
	/// <summary>
	/// Finalizes the linking
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public sealed class LinkerFinalizationStage : BaseCompilerStage
	{
		protected override void RunPostCompile()
		{
			if (string.IsNullOrEmpty(CompilerOptions.OutputFile))
				return;

			File.Delete(CompilerOptions.OutputFile);

			if (Compiler.IsStopped)
			{
				return;
			}

			using (var file = new FileStream(CompilerOptions.OutputFile, FileMode.Create))
			{
				Linker.Emit(file);
			}
		}
	}
}
