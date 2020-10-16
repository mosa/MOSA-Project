// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis.LiveVariableAnalysis;
using Mosa.Compiler.Framework.Trace;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stage determine were object references are located in code.
	/// </summary>
	public class PreciseGCStage : BaseMethodCompilerStage
	{
		private TraceLog trace;
		protected LivenessAnalysis LiveAnalysis;

		protected override void Run()
		{
			if (MethodCompiler.IsMethodPlugged)
				return;

			trace = CreateTraceLog();

			var liveAnalysisGCEnvironment = new GCEnvironment(BasicBlocks, Architecture, MethodCompiler.LocalStack);

			LiveAnalysis = new LivenessAnalysis(liveAnalysisGCEnvironment, CreateTraceLog, true);

			if (trace != null)
			{
				for (int i = 0; i < LiveAnalysis.LiveRanges.Length; i++)
				{
					var range = LiveAnalysis.LiveRanges[i];

					trace.Log($"{i}: {range}");
				}
			}
		}

		protected override void Finish()
		{
			trace = null;
			LiveAnalysis = null;
		}
	}
}
