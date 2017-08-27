// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.RegisterAllocator;
using Mosa.Compiler.Trace;
using System.Collections;
using System.Collections.Generic;
using System;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Analysis
{
	/// <summary>
	/// BaseLiveAnalysisEnvironment
	/// </summary>
	public abstract class BaseLiveAnalysisEnvironment
	{
		public BasicBlocks BasicBlocks { get; protected set; }

		public abstract IEnumerable<int> GetInput(InstructionNode node);

		public abstract IEnumerable<int> GetOutput(InstructionNode node);

		public abstract IEnumerable<int> GetKill(InstructionNode node);

		public int IndexCount { get; protected set; }
	}
}
