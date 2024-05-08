// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CompilerStages;

namespace Mosa.Tool.Explorer.Avalonia.CompilerStage;

/// <summary>
/// A compilation stage which generates a map file of the built binary file.
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public sealed class ExplorerMethodCompileTimeStage : MethodCompileTimeStage
{
	protected override void Finalization()
	{
		var methods = GetAndSortMethodData();
		var log = new TraceLog(TraceType.GlobalDebug, null, null, "Compiler Time");

		log.Log("Ticks\tMilliseconds\tCompiler Count\tMethod");
		foreach (var data in methods)
			log.Log($"{data.ElapsedTicks}{'\t'}{data.ElapsedTicks / TimeSpan.TicksPerMillisecond}{'\t'}{data.Version}{'\t'}{data.Method.FullName}");

		PostTraceLog(log);
	}
}
