// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Platform.x86.CompilerStages
{
	/// <summary>
	/// Start Up Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public sealed class StartUpStage : BaseCompilerStage
	{
		protected override void RunPreCompile()
		{
			var startUpType = TypeSystem.GetTypeByName("Mosa.Runtime", "StartUp");
			var startUpMethod = startUpType.FindMethodByName("StartApplication");

			Compiler.PlugSystem.CreatePlug(TypeSystem.EntryPoint, startUpMethod);

			if (Linker.EntryPoint == null)
			{
				var startUpInitializeMethod = startUpType.FindMethodByName("Initialize");

				Linker.EntryPoint = Linker.GetSymbol(startUpInitializeMethod.FullName, SectionKind.Text);
			}
		}
	}
}
