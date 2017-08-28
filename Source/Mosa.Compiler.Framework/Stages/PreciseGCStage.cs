// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.RegisterAllocator;
using Mosa.Compiler.Trace;
using System.Collections;
using System.Collections.Generic;
using System;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis.LiveVariableAnalysis;

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

			return;
		}

		protected override void Finish()
		{
		}
	}
}
