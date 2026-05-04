// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.UnitTestBisector;

internal sealed class PlanResult
{
	public string Transform { get; set; }

	public bool Passed { get; set; }

	public List<string> DisabledTransforms { get; set; } = [];
}
