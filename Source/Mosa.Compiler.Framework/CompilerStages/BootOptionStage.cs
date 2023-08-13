// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Linker;

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// Emit boot options
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public class BootOptionStage : BaseCompilerStage
{
	protected override void Finalization()
	{
		var bootOptions = Linker.DefineSymbol(Metadata.BootOptions, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
		var writer = new BinaryWriter(bootOptions.Stream);

		if (MosaSettings.OSBootOptions == null)
		{
			writer.WriteByte(0);
		}
		else
		{
			writer.WriteNullTerminatedString(MosaSettings.OSBootOptions);
		}
	}
}
