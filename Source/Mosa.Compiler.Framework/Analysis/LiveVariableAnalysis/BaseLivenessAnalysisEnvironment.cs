// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis.LiveVariableAnalysis
{
	/// <summary>
	/// BaseLiveAnalysisEnvironment
	/// </summary>
	public abstract class BaseLivenessAnalysisEnvironment
	{
		public BasicBlocks BasicBlocks { get; protected set; }

		public abstract IEnumerable<int> GetInputs(InstructionNode node);

		public abstract IEnumerable<int> GetOutputs(InstructionNode node);

		public abstract IEnumerable<int> GetKills(InstructionNode node);

		public int IndexCount { get; protected set; }
	}
}
