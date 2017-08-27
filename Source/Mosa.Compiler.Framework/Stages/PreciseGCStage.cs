// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.RegisterAllocator;
using Mosa.Compiler.Trace;
using System.Collections;
using System.Collections.Generic;
using System;
using Mosa.Compiler.Common;

// INCOMPLETE

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stage determine were object references are located in code.
	/// </summary>
	public class PreciseGCStage : BaseMethodCompilerStage
	{
		private TraceLog trace;

		protected override void Run()
		{
			if (IsPlugged)
				return;

			trace = CreateTraceLog();
		}

		protected override void Finish()
		{
		}
	}
}
