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
			using (var file = new FileStream(CompilerOptions.OutputFile, FileMode.Create))
			{
				Linker.Emit(file);
			}
		}
	}
}
