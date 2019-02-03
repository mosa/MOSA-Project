// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
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

			using (var file = new FileStream(CompilerOptions.OutputFile, FileMode.Create))
			{
				Linker.Emit(file);
			}

			Compiler.GlobalCounters.Update("Linker.Text", (int)Linker.LinkerSections[(int)SectionKind.Text].Size);
			Compiler.GlobalCounters.Update("Linker.Data", (int)Linker.LinkerSections[(int)SectionKind.Data].Size);
			Compiler.GlobalCounters.Update("Linker.ROData", (int)Linker.LinkerSections[(int)SectionKind.ROData].Size);
			Compiler.GlobalCounters.Update("Linker.BSS", (int)Linker.LinkerSections[(int)SectionKind.BSS].Size);
		}
	}
}
