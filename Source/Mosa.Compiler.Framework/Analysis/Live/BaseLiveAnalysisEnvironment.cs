// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis.Live
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

		public int SlotCount { get; protected set; }
	}
}
