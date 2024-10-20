// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// This stage inserts the GC safe points.
/// </summary>
public class SafePointStage : BaseMethodCompilerStage
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
