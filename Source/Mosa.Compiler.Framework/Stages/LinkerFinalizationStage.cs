/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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
				file.Close();
			}
		}
	}
}