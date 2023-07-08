// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public sealed class PostLinkHashFileStage : PreLinkHashFileStage
{
	protected override void Finalization()
	{
		Generate(MosaSettings.PostLinkHashFile);
	}
}
