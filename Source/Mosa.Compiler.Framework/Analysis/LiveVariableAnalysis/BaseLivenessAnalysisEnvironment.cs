// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Analysis.LiveVariableAnalysis;

/// <summary>
/// BaseLiveAnalysisEnvironment
/// </summary>
public abstract class BaseLivenessAnalysisEnvironment
{
	public BasicBlocks BasicBlocks { get; protected set; }

	public abstract IEnumerable<int> GetInputs(Node node);

	public abstract IEnumerable<int> GetOutputs(Node node);

	public abstract IEnumerable<int> GetKills(Node node);

	public int IndexCount { get; protected set; }
}
