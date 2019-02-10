// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
using System.IO;

namespace Mosa.Compiler.Framework.CompilerStages
{
	/// <summary>
	/// Finalizes the linking
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public sealed class LinkerEmitStage : BaseCompilerStage
	{
		protected override void RunPostCompile()
		{
			if (!CompilerOptions.EmitBinary)
				return;

			if (string.IsNullOrEmpty(CompilerOptions.OutputFile))
				return;

			File.Delete(CompilerOptions.OutputFile);

			using (var file = new FileStream(CompilerOptions.OutputFile, FileMode.Create))
			{
				Linker.Emit(file);
			}
		}
	}
}
