// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis.LiveVariableAnalysis;
using Mosa.Compiler.Trace;

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
			if (IsPlugged)
				return;

			trace = CreateTraceLog();

			var liveAnalysisGCEnvironment = new GCEnvironment(BasicBlocks, Architecture, MethodCompiler.LocalStack);

			LiveAnalysis = new LivenessAnalysis(liveAnalysisGCEnvironment, this, true);

			if (trace.Active)
			{
				for (int i = 0; i < LiveAnalysis.LiveRanges.Length; i++)
				{
					var range = LiveAnalysis.LiveRanges[i];

					trace.Log(i.ToString() + ": " + range);
				}
			}
		}

		protected override void Finish()
		{
			base.Finish();

			trace = null;
			LiveAnalysis = null;
		}
	}
}
