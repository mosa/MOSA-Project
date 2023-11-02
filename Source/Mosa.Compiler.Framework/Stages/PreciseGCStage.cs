// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// This stage determines were object references are located in code.
/// </summary>
public class PreciseGCStage : BaseMethodCompilerStage
{
	private TraceLog trace;

	protected override void Run()
	{
		if (MethodCompiler.IsMethodPlugged)
			return;

		trace = CreateTraceLog();
	}

	protected override void Finish()
	{
		trace = null;
	}
}
