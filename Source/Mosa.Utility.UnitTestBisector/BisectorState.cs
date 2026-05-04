// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.UnitTestBisector;

internal sealed class BisectorState
{
	public string StageName { get; set; }

	public PlanKind Plan { get; set; }

	public string UnitTestFilter { get; set; }

	public List<string> ObservedTransforms { get; set; } = [];

	public Dictionary<string, int> ObservedTransformCounts { get; set; } = new(StringComparer.Ordinal);

	public bool BaselineCompleted { get; set; }

	public bool BaselinePassed { get; set; }

	public int NextIndex { get; set; }

	public int IterationNumber { get; set; }

	public int PassCount { get; set; }

	public int TotalIterationCount { get; set; }

	public OrderKind Order { get; set; }

	public int RandomSeed { get; set; }

	public List<PlanResult> Results { get; set; } = [];

	public bool Completed { get; set; }

	public string LastExitKind { get; set; } = Constant.ExitKindUnknown;

	public int LastExitCode { get; set; }
}
