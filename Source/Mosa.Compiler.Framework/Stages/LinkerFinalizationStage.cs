// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Finalizes the linking
	/// </summary>
	public sealed class LinkerFinalizationStage : BaseCompilerStage
	{
		protected override void Run()
		{
			using (var file = new FileStream(CompilerOptions.OutputFile, FileMode.Create))
			{
				Linker.Emit(file);
			}
		}
	}
}