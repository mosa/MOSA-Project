// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.UnitTestBisector;

internal static class Constant
{
	public const int WorkerContinueExitCode = 2;
	public const int BaselineIterationNumber = 0;
	public const string ExitKindUnknown = "Unknown";
	public const string ExitKindContinue = "Continue";
	public const string ExitKindCompleted = "Completed";
	public const string ExitKindFailure = "Failure";

	public const string OptionBisectStage = "-bisect-stage";
	public const string OptionBisectPlan = "-bisect-plan";
	public const string OptionBisectOrder = "-bisect-order";
	public const string OptionFilter = "-filter";
}
