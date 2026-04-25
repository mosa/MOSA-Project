// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.Configuration;
using Mosa.Utility.UnitTests;

namespace Mosa.Utility.UnitTestBisector;

public sealed class UnitTestBisectorSystem
{
	private readonly Stopwatch Stopwatch = new();
	private readonly MosaSettings MosaSettings = new();
	private readonly object transformDiscoveryLock = new();
	private List<UnitTestInfo> discoveredUnitTests = [];
	private Type selectedStageType;
	private string selectedStageName;
	private HashSet<string> observedTransformNames = [];
	private HashSet<string> disabledTransformNames = [];

	private Bisector<string> bisector;

	public int Start(string[] args)
	{
		try
		{
			MosaSettings.LoadArguments(args);

			MosaSettings.UnitTestFailFast = true;

			Stopwatch.Start();

			OutputStatus("Resolving stage type...");
			selectedStageType = ResolveStageType(MosaSettings.UnitTestBisectorStage);
			selectedStageName = selectedStageType.Name;
			OutputStatus($"Stage: {selectedStageType.FullName} ({selectedStageName})");

			OutputStatus("Discovering Unit Tests...");
			discoveredUnitTests = Discovery.DiscoverUnitTests(MosaSettings.UnitTestFilter);
			OutputStatus($"Found Tests: {discoveredUnitTests.Count} in {Stopwatch.ElapsedMilliseconds / 1000.0:F2} secs");

			if (discoveredUnitTests.Count == 0)
			{
				OutputStatus("ERROR: No tests matched the filter.");
				return 1;
			}

			OutputStatus("Starting discovery iteration...");
			var discoveryResult = ExecuteIteration();

			if (!discoveryResult.CompileSucceeded)
			{
				OutputStatus("ERROR: Discovery compilation failed.");
				return 1;
			}

			if (observedTransformNames.Count == 0)
			{
				OutputStatus("ERROR: No observed transforms were captured for the selected stage.");
				return 1;
			}

			OutputStatus($"Observed Transforms: {observedTransformNames.Count}");

			if (discoveryResult.Passed)
			{
				OutputStatus("All selected unit tests passed. Nothing to bisect.");
				return 0;
			}

			bisector = new Bisector<string>(observedTransformNames);

			while (!bisector.IsComplete)
			{
				disabledTransformNames = [.. bisector.GetNextDisabledItems()];
				PrintIterationHeader(bisector.GetStatus());
				PrintDisabledTransforms();

				var iterationResult = ExecuteIteration();

				if (!iterationResult.CompileSucceeded)
				{
					OutputStatus("ERROR: Iteration compilation failed.");
					return 1;
				}

				bisector.AcceptResult(iterationResult.Passed);
				OutputStatus($"Iteration Result: {(iterationResult.Passed ? "PASS" : "FAIL")}");
				PrintStatus(bisector.GetStatus());
			}

			OutputStatus("Bisector complete.");
			PrintFinalReport(bisector);
			return 0;
		}
		catch (Exception ex)
		{
			OutputStatus($"Exception: {ex.Message}");
			OutputStatus($"Exception: {ex.StackTrace}");
			return 1;
		}
	}

	private IterationResult ExecuteIteration()
	{
		using var unitTestEngine = new UnitTestEngine(MosaSettings, OutputStatus, CreateCompilerHooks);
		if (unitTestEngine.IsAborted)
		{
			return new IterationResult(false, false);
		}

		var unitTests = PrepareUnitTests(discoveredUnitTests, unitTestEngine.TypeSystem, unitTestEngine.Linker);

		unitTestEngine.QueueUnitTests(unitTests);
		unitTestEngine.WaitUntilComplete();
		unitTestEngine.Terminate();

		var passed = true;
		foreach (var unitTest in unitTests)
		{
			if (unitTest.Status is UnitTestStatus.Failed or UnitTestStatus.FailedByCrash or UnitTestStatus.Pending)
			{
				passed = false;
				break;
			}
		}

		return new IterationResult(true, passed);
	}

	private CompilerHooks CreateCompilerHooks()
	{
		return new CompilerHooks
		{
			NotifyTransformObserved = NotifyTransformObserved,
			IsTransformDisabled = IsTransformDisabled,
		};
	}

	private void NotifyTransformObserved(string stageName, string transformName)
	{
		if (!string.Equals(stageName, selectedStageName, StringComparison.Ordinal))
			return;

		lock (transformDiscoveryLock)
		{
			if (observedTransformNames.Add(transformName) && bisector != null)
			{
				bisector.ObserveItem(transformName);
			}
		}
	}

	private bool IsTransformDisabled(string stageName, string transformName)
	{
		if (!string.Equals(stageName, selectedStageName, StringComparison.Ordinal))
			return false;

		return disabledTransformNames.Contains(transformName);
	}

	private Type ResolveStageType(string stageName)
	{
		if (string.IsNullOrWhiteSpace(stageName))
			throw new InvalidOperationException("A stage type name is required. Use -bisect-stage.");

		var stageTypes = typeof(OptimizationStage).Assembly.GetTypes()
			.Where(t => !t.IsAbstract && typeof(BaseTransformStage).IsAssignableFrom(t))
			.ToList();

		var fullNameMatches = stageTypes.Where(t => string.Equals(t.FullName, stageName, StringComparison.Ordinal)).ToList();
		if (fullNameMatches.Count == 1)
			return fullNameMatches[0];
		if (fullNameMatches.Count > 1)
			throw new InvalidOperationException($"Stage name '{stageName}' is ambiguous.");

		var shortNameMatches = stageTypes.Where(t => string.Equals(t.Name, stageName, StringComparison.Ordinal)).ToList();
		if (shortNameMatches.Count == 1)
			return shortNameMatches[0];
		if (shortNameMatches.Count > 1)
			throw new InvalidOperationException($"Stage name '{stageName}' is ambiguous. Use the full type name.");

		throw new InvalidOperationException($"Unable to resolve stage '{stageName}'.");
	}

	private List<UnitTest> PrepareUnitTests(List<UnitTestInfo> tests, TypeSystem typeSystem, MosaLinker linker)
	{
		var unitTests = new List<UnitTest>(tests.Count);
		var id = 0;

		foreach (var unitTestInfo in tests)
		{
			var linkerMethodInfo = Linker.GetMethodInfo(typeSystem, linker, unitTestInfo);
			var unitTest = new UnitTest(unitTestInfo, linkerMethodInfo)
			{
				SerializedUnitTest = UnitTestSystem.SerializeUnitTestMessage(new UnitTest(unitTestInfo, linkerMethodInfo)),
				UnitTestID = ++id,
			};

			unitTest.SerializedUnitTest = UnitTestSystem.SerializeUnitTestMessage(unitTest);
			unitTests.Add(unitTest);
		}

		return unitTests;
	}

	private void PrintIterationHeader(Bisector<string>.BisectorStatus status)
	{
		OutputStatus($"Iteration: {status.Iteration + 1}");
		OutputStatus($"Level: {status.Level}");
		OutputStatus($"Phase: {status.Phase}");
		OutputStatus($"Stage: {selectedStageType.FullName} ({selectedStageName})");
	}

	private void PrintDisabledTransforms()
	{
		OutputStatus($"Disabled Transforms: {disabledTransformNames.Count}");
		foreach (var transform in disabledTransformNames.OrderBy(t => t))
		{
			OutputStatus($"  DISABLED: {transform}");
		}
	}

	private void PrintStatus(Bisector<string>.BisectorStatus status)
	{
		OutputStatus($"Status.Iteration: {status.Iteration}");
		OutputStatus($"Status.TotalItems: {status.TotalItemCount}");
		OutputStatus($"Status.Suspects: {status.SuspectItemCount}");
		OutputStatus($"Status.BadItems: {status.ConfirmedBadItemCount}");
		OutputStatus($"Status.BadPairs: {status.ConfirmedBadPairCount}");
		OutputStatus($"Status.PairwiseCompleted: {status.PairwiseTestsCompleted}");
		OutputStatus($"Status.PairwiseRemaining: {status.PairwiseTestsRemaining}");
	}

	private void PrintFinalReport(Bisector<string> bisector)
	{
		OutputStatus($"Final Stage: {selectedStageType.FullName} ({selectedStageName})");
		OutputStatus("Confirmed Bad Items:");
		foreach (var transform in bisector.ConfirmedBadItems.OrderBy(t => t))
		{
			OutputStatus($"  {transform}");
		}

		OutputStatus("Confirmed Bad Pairs:");
		foreach (var pair in bisector.ConfirmedBadPairs.OrderBy(p => p.Item1).ThenBy(p => p.Item2))
		{
			OutputStatus($"  {pair.Item1} + {pair.Item2}");
		}

		OutputStatus("Remaining Suspects:");
		foreach (var transform in bisector.RemainingSuspectItems.OrderBy(t => t))
		{
			OutputStatus($"  {transform}");
		}
	}

	private void OutputStatus(string status)
	{
		Console.WriteLine($"{Stopwatch.Elapsed.TotalSeconds:00.00} | {status}");
	}

	private readonly record struct IterationResult(bool CompileSucceeded, bool Passed);
}
