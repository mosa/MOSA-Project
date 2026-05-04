// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Text.Json;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.Configuration;
using Mosa.Utility.UnitTests;

namespace Mosa.Utility.UnitTestBisector;

public sealed partial class UnitTestBisectorSystem
{
	private readonly record struct IterationResult(bool Passed);

	private enum PlanKind
	{
		DisableOne,
		EnableOne,
		RandomCombo,
		FailureInducing,
		Masking,
	}

	private enum OrderKind
	{
		Unspecified = 0,
		Original = 1,
		CountAscending = 2,
		Random = 3,
	}

	private readonly Stopwatch stopwatch = new();
	private readonly MosaSettings mosaSettings = new();
	private readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };
	private readonly object transformDiscoveryLock = new();

	private bool hasCompilationFailure;
	private string lastCompilationFailure;
	private string unitTestFilter;

	private List<UnitTestInfo> discoveredUnitTests = [];
	private bool observeFilterOnly;
	private HashSet<string> observedTransformNames = [];
	private Dictionary<string, int> observedTransformCounts = new(StringComparer.Ordinal);
	private HashSet<string> bisectorDisabledTransformNames = [];
	private HashSet<string> effectiveDisabledTransformNames = [];
	private HashSet<string> forcedDisabledTransformNames = [];
	private Bisector<string> bisector;

	public int Start(string[] args)
	{
		try
		{
			OutputStatusBisector($"Bisector started");

			mosaSettings.LoadArguments(args);
			mosaSettings.UnitTestFailFast = true;
			unitTestFilter = mosaSettings.UnitTestFilter;
			hasCompilationFailure = false;
			lastCompilationFailure = null;

			stopwatch.Start();

			var plan = ParsePlan(mosaSettings.BisectorPlan);
			var isBisectorPlan = IsBisectorPlan(plan);
			var order = isBisectorPlan ? OrderKind.Unspecified : ParseOrder(mosaSettings.BisectorOrder);
			var stateFile = GetFullStateFilePath();
			if (mosaSettings.BisectorResetState && File.Exists(stateFile))
			{
				File.Delete(stateFile);
				OutputStatusBisector($"Deleted state file: {stateFile}");
			}

			LoadForcedDisabledTransforms();

			if (string.IsNullOrWhiteSpace(mosaSettings.BisectorStage))
				throw new InvalidOperationException($"A stage type name is required. Use {Constant.OptionBisectStage}.");

			OutputStatusBisector($"Stage: {mosaSettings.BisectorStage}");

			OutputStatusBisector($"Plan: {plan}");
			if (!isBisectorPlan)
			{
				OutputStatusBisector($"Order: {order}");
			}
			OutputStatusBisector($"State File: {stateFile}");

			OutputStatus("Discovering Unit Tests...");
			discoveredUnitTests = Discovery.DiscoverUnitTests(unitTestFilter);
			observeFilterOnly = !string.IsNullOrWhiteSpace(unitTestFilter) && discoveredUnitTests.Count != 0;
			OutputStatus($"Found Tests: {discoveredUnitTests.Count} in {stopwatch.ElapsedMilliseconds / 1000.0:F1} secs");
			if (observeFilterOnly)
				OutputStatusBisector($"Observed Methods In Scope: {discoveredUnitTests.Count}");

			if (discoveredUnitTests.Count == 0)
			{
				OutputStatus("ERROR: No tests matched the filter.");
				return 1;
			}

			if (isBisectorPlan)
			{
				var bisectorState = LoadOrCreateState(stateFile, plan, out _);
				EnsureStateCompatibility(bisectorState, plan, OrderKind.Unspecified);
				OutputStatusBisector($"Saved Iteration: {bisectorState.IterationNumber}");
				return ExecuteBisectorPlan(plan, stateFile, bisectorState);
			}

			var state = LoadOrCreateState(stateFile, plan, out var stateFileUsed);
			EnsureStateCompatibility(state, plan, order);

			OutputStatusBisector($"Saved Iteration: {state.IterationNumber}");

			if (state.ObservedTransforms.Count == 0)
			{
				OutputStatusBisector("Running transform discovery iteration...");
				observedTransformCounts.Clear();
				bisectorDisabledTransformNames = [];
				RebuildEffectiveDisabledSet();

				var discoveryResult = ExecuteIteration(state.IterationNumber);
				OutputStatusBisector($"Discovery Iteration: {(discoveryResult.Passed ? "PASS" : "FAIL")}");

				var observed = observedTransformNames
					.Where(name => !forcedDisabledTransformNames.Contains(name))
					.OrderBy(name => name)
					.ToList();

				if (observed.Count == 0)
				{
					OutputStatusBisector("No transforms observed for selected stage/plan/options. Nothing to bisect; marking session complete.");
					state.Completed = true;
					SetLastExit(state, Constant.ExitKindCompleted, 0);
					SaveState(stateFile, state);
					WriteFailureReviewFile(stateFile, plan, state);
					return 0;
				}

				var filteredCounts = new Dictionary<string, int>(StringComparer.Ordinal);
				foreach (var name in observed)
				{
					filteredCounts[name] = observedTransformCounts.TryGetValue(name, out var count) ? count : 0;
				}

				state.Order = order;
				state.ObservedTransformCounts = filteredCounts;
				state.RandomSeed = ResolveRandomSeed(state.RandomSeed);
				state.ObservedTransforms = BuildIterationSequence(observed, filteredCounts, order, state.RandomSeed);
				state.IterationNumber = Constant.BaselineIterationNumber;
				SaveState(stateFile, state);

				OutputStatusBisector($"Discovered transforms for plan: {state.ObservedTransforms.Count}");
				ReportForcedDisabledNotObserved();
			}
			else
			{
				foreach (var transform in state.ObservedTransforms)
				{
					observedTransformNames.Add(transform);
				}

				state.ObservedTransformCounts ??= new Dictionary<string, int>(StringComparer.Ordinal);
				state.Order = state.Order == OrderKind.Unspecified ? order : state.Order;
				state.RandomSeed = ResolveRandomSeed(state.RandomSeed);

				OutputStatusBisector($"Loaded transforms from state: {state.ObservedTransforms.Count}");
			}

			return plan == PlanKind.RandomCombo
				? ExecuteRandomComboPlan(stateFile, state)
				: ExecuteDeterministicPlan(stateFile, state, plan);
		}
		catch (Exception ex)
		{
			OutputStatus($"Exception: {ex.Message}");
			OutputStatus($"Exception: {ex.StackTrace}");
			return 1;
		}
	}

	private static bool IsBisectorPlan(PlanKind plan)
	{
		return plan is PlanKind.FailureInducing or PlanKind.Masking;
	}

	private int ExecuteBisectorPlan(PlanKind plan, string stateFile, BisectorState state)
	{
		if (!IsBisectorPlan(plan))
			throw new InvalidOperationException($"Plan '{plan}' is not a bisector plan.");

		if (state.Completed)
		{
			OutputStatusBisector("Bisector state already completed.");
			return 0;
		}

		if (state.ObservedTransforms.Count != 0)
		{
			foreach (var transform in state.ObservedTransforms)
			{
				observedTransformNames.Add(transform);
			}
		}

		if (!state.BaselineCompleted || state.ObservedTransforms.Count == 0)
		{
			OutputStatusBisector("Running transform discovery iteration...");
			OutputStatusBisector($"Iteration: {state.IterationNumber}");
			observedTransformCounts.Clear();
			bisectorDisabledTransformNames = [];
			RebuildEffectiveDisabledSet();

			var discoveryResult = ExecuteIteration(state.IterationNumber);
			state.BaselineCompleted = true;
			state.BaselinePassed = discoveryResult.Passed;
			UpdateCounters(state, discoveryResult.Passed);
			OutputStatusBisector($"Discovery Iteration: {(discoveryResult.Passed ? "PASS" : "FAIL")}");
			PrintPassAndIterationCounts(state);

			if (hasCompilationFailure)
			{
				SaveState(stateFile, state);
				WriteFailureReviewFile(stateFile, plan, state);
				return 1;
			}

			var observed = observedTransformNames
				.Where(name => !forcedDisabledTransformNames.Contains(name))
				.OrderBy(name => name)
				.ToList();

			if (observed.Count == 0)
			{
				OutputStatusBisector("No transforms observed for selected stage/plan/options. Nothing to bisect; marking session complete.");
				state.Completed = true;
				SetLastExit(state, Constant.ExitKindCompleted, 0);
				SaveState(stateFile, state);
				WriteFailureReviewFile(stateFile, plan, state);
				return 0;
			}

			OutputStatusBisector($"Observed Transforms: {observed.Count}");
			ReportForcedDisabledNotObserved();

			state.ObservedTransforms = observed;
			state.ObservedTransformCounts = observed.ToDictionary(
				name => name,
				name => observedTransformCounts.TryGetValue(name, out var count) ? count : 0,
				StringComparer.Ordinal);

			state.Results = [];
			state.NextIndex = 0;
			state.IterationNumber = Constant.BaselineIterationNumber;

			SaveState(stateFile, state);
		}
		else
		{
			state.ObservedTransformCounts ??= new Dictionary<string, int>(StringComparer.Ordinal);
			OutputStatusBisector($"Loaded transforms from state: {state.ObservedTransforms.Count}");
		}

		var discoveryResultForSession = new IterationResult(state.BaselinePassed);

		if (plan == PlanKind.FailureInducing)
		{
			state.Results = RunBisectorSession(stateFile, state, "Failure-Inducing", invertOutcome: false, discoveryResultForSession);
			state.NextIndex = state.Results.Count;
			state.Completed = !hasCompilationFailure;
			SaveState(stateFile, state);
			WriteFailureReviewFile(stateFile, plan, state);
			return hasCompilationFailure ? 1 : 0;
		}

		if (!discoveryResultForSession.Passed)
		{
			OutputStatusBisector("Masking baseline is FAIL. Falling back to failure-inducing bisector to narrow failing transforms.");
			state.Results = RunBisectorSession(stateFile, state, "Failure-Inducing", invertOutcome: false, discoveryResultForSession);
			state.NextIndex = state.Results.Count;
			state.Completed = !hasCompilationFailure;
			SaveState(stateFile, state);
			WriteFailureReviewFile(stateFile, plan, state);
			return hasCompilationFailure ? 1 : 0;
		}

		OutputStatusBisector("Running masking pre-check (all transforms disabled)...");
		bisectorDisabledTransformNames = [.. state.ObservedTransforms];
		RebuildEffectiveDisabledSet();
		var maskingPreCheckResult = ExecuteIteration(state.IterationNumber);
		bisectorDisabledTransformNames = [];
		RebuildEffectiveDisabledSet();

		if (hasCompilationFailure)
		{
			SaveState(stateFile, state);
			WriteFailureReviewFile(stateFile, plan, state);
			return 1;
		}

		OutputStatusBisector($"Masking Pre-Check -> Actual: {(maskingPreCheckResult.Passed ? "PASS" : "FAIL")}");

		if (maskingPreCheckResult.Passed)
		{
			state.Completed = true;
			SaveState(stateFile, state);
			WriteFailureReviewFile(stateFile, plan, state);
			OutputStatusBisector("Masking pre-check passed (all-disabled still passes). No masking transforms identified.");
			return 0;
		}

		state.Results = RunBisectorSession(stateFile, state, "Masking", invertOutcome: true, discoveryResultForSession);
		state.NextIndex = state.Results.Count;
		state.Completed = !hasCompilationFailure;
		SaveState(stateFile, state);
		WriteFailureReviewFile(stateFile, plan, state);
		return hasCompilationFailure ? 1 : 0;
	}

	private List<PlanResult> RunBisectorSession(string stateFile, BisectorState state, string sessionName, bool invertOutcome, IterationResult discoveryResult)
	{
		var sessionResults = new List<PlanResult>();
		bisectorDisabledTransformNames = [];
		RebuildEffectiveDisabledSet();
		bisector = new Bisector<string>(observedTransformNames.Where(name => !forcedDisabledTransformNames.Contains(name)), enablePairwise: mosaSettings.BisectorPairwise);
		var reportedBadItems = new HashSet<string>(StringComparer.Ordinal);

		bisectorDisabledTransformNames = [.. bisector.GetNextDisabledItems()];
		RebuildEffectiveDisabledSet();
		var mappedBaseline = MapOutcome(discoveryResult.Passed, invertOutcome);
		bisector.AcceptResult(mappedBaseline);
		sessionResults.Add(new PlanResult
		{
			Transform = "baseline",
			Passed = discoveryResult.Passed,
			DisabledTransforms = effectiveDisabledTransformNames.OrderBy(x => x).ToList(),
		});
		state.IterationNumber = Constant.BaselineIterationNumber;
		state.Results = [.. sessionResults];
		state.NextIndex = state.Results.Count;
		SaveState(stateFile, state);
		OutputStatusBisector($"{sessionName} Baseline -> Actual: {(discoveryResult.Passed ? "PASS" : "FAIL")}, Mapped: {(mappedBaseline ? "PASS" : "FAIL")}");
		PrintNewlyConfirmedBadItems(sessionName, bisector, reportedBadItems);

		while (!bisector.IsComplete)
		{
			bisectorDisabledTransformNames = [.. bisector.GetNextDisabledItems()];
			RebuildEffectiveDisabledSet();
			var status = bisector.GetStatus();
			state.IterationNumber = status.Iteration + 1;
			PrintIterationHeader(sessionName, status);
			PrintDisabledTransforms();

			var iterationResult = ExecuteIteration(state.IterationNumber);
			var mappedResult = MapOutcome(iterationResult.Passed, invertOutcome);

			sessionResults.Add(new PlanResult
			{
				Transform = $"iter-{status.Iteration + 1}-{status.Level}-{status.Phase}",
				Passed = iterationResult.Passed,
				DisabledTransforms = effectiveDisabledTransformNames.OrderBy(x => x).ToList(),
			});

			state.Results = [.. sessionResults];
			state.NextIndex = state.Results.Count;
			SaveState(stateFile, state);
			WriteFailureReviewFile(stateFile, state.Plan, state);

			bisector.AcceptResult(mappedResult);
			OutputStatusBisector($"Iteration Result -> Actual: {(iterationResult.Passed ? "PASS" : "FAIL")}, Mapped: {(mappedResult ? "PASS" : "FAIL")}");
			PrintNewlyConfirmedBadItems(sessionName, bisector, reportedBadItems);
			PrintStatus(bisector.GetStatus());

			if (hasCompilationFailure)
				return sessionResults;
		}

		OutputStatusBisector($"{sessionName} bisector complete.");
		PrintFinalReport(sessionName, bisector);

		return sessionResults;
	}

	private void PrintIterationHeader(string sessionName, Bisector<string>.BisectorStatus status)
	{
		OutputStatusBisector($"{sessionName} Iteration: {status.Iteration + 1}");
		OutputStatusBisector($"Level: {status.Level}");
		OutputStatusBisector($"Phase: {status.Phase}");
		OutputStatusBisector($"Stage: {mosaSettings.BisectorStage}");
	}

	private static bool MapOutcome(bool passed, bool invertOutcome)
	{
		return invertOutcome ? !passed : passed;
	}

	private static void RecalculateCounters(BisectorState state)
	{
		if (IsBisectorPlan(state.Plan))
		{
			if (state.Results.Count == 0)
			{
				state.TotalIterationCount = state.BaselineCompleted ? 1 : 0;
				state.PassCount = state.BaselineCompleted && state.BaselinePassed ? 1 : 0;
			}
			else
			{
				state.TotalIterationCount = state.Results.Count;
				state.PassCount = state.Results.Count(r => r.Passed);
			}
		}
		else
		{
			state.TotalIterationCount = state.Results.Count + (state.BaselineCompleted ? 1 : 0);
			state.PassCount = state.Results.Count(r => r.Passed) + (state.BaselineCompleted && state.BaselinePassed ? 1 : 0);
		}
	}

	private static void UpdateCounters(BisectorState state, bool passed)
	{
		state.TotalIterationCount++;
		if (passed)
			state.PassCount++;
	}

	private void PrintPassAndIterationCounts(BisectorState state)
	{
		var failureCount = Math.Max(0, state.TotalIterationCount - state.PassCount);
		OutputStatusBisector($"Iterations: {state.TotalIterationCount} | Passes: {state.PassCount} | Failures: {failureCount}");
	}

	private void PrintNewlyConfirmedBadItems(string sessionName, Bisector<string> sessionBisector, HashSet<string> reportedBadItems)
	{
		foreach (var transform in sessionBisector.ConfirmedBadItems.OrderBy(t => t))
		{
			if (reportedBadItems.Add(transform))
				OutputStatusBisector($"{sessionName} Known Bad Item: {transform}");
		}
	}

	private void PrintStatus(Bisector<string>.BisectorStatus status)
	{
		OutputStatusBisector($"Status.Iteration: {status.Iteration}");
		OutputStatusBisector($"Status.TotalItems: {status.TotalItemCount}");
		OutputStatusBisector($"Status.Suspects: {status.SuspectItemCount}");
		OutputStatusBisector($"Status.BadItems: {status.ConfirmedBadItemCount}");
		OutputStatusBisector($"Status.BadPairs: {status.ConfirmedBadPairCount}");
		OutputStatusBisector($"Status.PairwiseCompleted: {status.PairwiseTestsCompleted}");
		OutputStatusBisector($"Status.PairwiseRemaining: {status.PairwiseTestsRemaining}");
	}

	private void PrintFinalReport(string sessionName, Bisector<string> sessionBisector)
	{
		OutputStatusBisector($"{sessionName} Final Stage: {mosaSettings.BisectorStage}");
		OutputStatusBisector("Forced Disabled Items:");
		foreach (var transform in forcedDisabledTransformNames.OrderBy(t => t))
		{
			OutputStatusBisector($"  {transform}");
		}

		OutputStatusBisector("Confirmed Bad Items:");
		foreach (var transform in sessionBisector.ConfirmedBadItems.OrderBy(t => t))
		{
			OutputStatusBisector($"  {transform}");
		}

		OutputStatusBisector("Confirmed Bad Pairs:");
		foreach (var pair in sessionBisector.ConfirmedBadPairs.OrderBy(p => p.Item1).ThenBy(p => p.Item2))
		{
			OutputStatusBisector($"  {pair.Item1} + {pair.Item2}");
		}

		OutputStatusBisector("Remaining Suspects:");
		foreach (var transform in sessionBisector.RemainingSuspectItems.OrderBy(t => t))
		{
			OutputStatusBisector($"  {transform}");
		}
	}

	private static PlanKind ParsePlan(string plan)
	{
		return plan.ToLowerInvariant() switch
		{
			"enable-one" => PlanKind.EnableOne,
			"disable-one" => PlanKind.DisableOne,
			"random-combo" => PlanKind.RandomCombo,
			"failure-inducing" => PlanKind.FailureInducing,
			"masking" => PlanKind.Masking,
			_ => throw new InvalidOperationException($"Unknown plan '{plan}'. Valid values: disable-one, enable-one, random-combo, failure-inducing, masking."),
		};
	}

	private static OrderKind ParseOrder(string order)
	{
		if (string.IsNullOrWhiteSpace(order))
			return OrderKind.Original;

		return order.ToLowerInvariant() switch
		{
			"original" => OrderKind.Original,
			"count" or "count-ascending" => OrderKind.CountAscending,
			"random" => OrderKind.Random,
			_ => throw new InvalidOperationException($"Invalid order value '{order}'. Valid values: original, count, random."),
		};
	}

	private string GetFullStateFilePath()
	{
		var stateFile = mosaSettings.BisectorStateFile;

		if (!Path.IsPathRooted(stateFile))
			stateFile = Path.GetFullPath(stateFile);

		return stateFile;
	}

	private BisectorState LoadOrCreateState(string stateFile, PlanKind plan, out bool stateFileUsed)
	{
		if (!File.Exists(stateFile))
		{
			stateFileUsed = false;
			return new BisectorState
			{
				Plan = plan,
				StageName = mosaSettings.BisectorStage,
				UnitTestFilter = unitTestFilter,
				DisabledTransformsFile = mosaSettings.BisectorDisabledTransformsFile,
				IterationNumber = Constant.BaselineIterationNumber,
				LastExitKind = Constant.ExitKindUnknown,
				LastExitCode = 0,
			};
		}

		stateFileUsed = true;
		var content = File.ReadAllText(stateFile);
		var state = JsonSerializer.Deserialize<BisectorState>(content);
		if (state == null)
			throw new InvalidOperationException($"Unable to deserialize state file: {stateFile}");

		state.Results ??= [];
		state.ObservedTransforms ??= [];
		if (string.IsNullOrWhiteSpace(state.LastExitKind))
			state.LastExitKind = Constant.ExitKindUnknown;

		if (state.NextIndex < 0)
			state.NextIndex = 0;

		if (state.NextIndex > state.Results.Count)
			state.NextIndex = state.Results.Count;

		if (state.Results.Count > state.NextIndex)
			state.Results = state.Results.Take(state.NextIndex).ToList();

		if (state.IterationNumber < Constant.BaselineIterationNumber)
			state.IterationNumber = Constant.BaselineIterationNumber;

		state.IterationNumber = Math.Max(state.IterationNumber, state.NextIndex);
		RecalculateCounters(state);

		return state;
	}

	private static void SetLastExit(BisectorState state, string exitKind, int exitCode)
	{
		state.LastExitKind = exitKind;
		state.LastExitCode = exitCode;
	}

	private void SaveState(string stateFile, BisectorState state)
	{
		var directory = Path.GetDirectoryName(stateFile);
		if (!string.IsNullOrEmpty(directory))
			Directory.CreateDirectory(directory);

		if (state.IterationNumber < Constant.BaselineIterationNumber)
			state.IterationNumber = Constant.BaselineIterationNumber;

		RecalculateCounters(state);

		var json = JsonSerializer.Serialize(state, jsonSerializerOptions);
		File.WriteAllText(stateFile, json);
	}

	private void EnsureStateCompatibility(BisectorState state, PlanKind plan, OrderKind order)
	{
		if (!string.Equals(state.StageName, mosaSettings.BisectorStage, StringComparison.Ordinal))
			throw new InvalidOperationException($"State file stage does not match current {Constant.OptionBisectStage}.");

		if (state.Plan != plan)
			throw new InvalidOperationException($"State file plan does not match current {Constant.OptionBisectPlan}.");

		if (!string.Equals(state.UnitTestFilter, unitTestFilter, StringComparison.Ordinal))
			throw new InvalidOperationException($"State file UnitTest filter does not match current {Constant.OptionFilter}.");

		if (!string.Equals(state.DisabledTransformsFile, mosaSettings.BisectorDisabledTransformsFile, StringComparison.Ordinal))
			throw new InvalidOperationException($"State file disabled transforms file does not match current {Constant.OptionBisectDisabledFile}.");

		if (state.Order == OrderKind.Unspecified)
			state.Order = order;

		if (state.Order != order)
			throw new InvalidOperationException($"State file order does not match current {Constant.OptionBisectOrder}.");
	}

	private HashSet<string> BuildDisabledSetForBaseline(PlanKind plan, List<string> transforms)
	{
		if (plan == PlanKind.EnableOne)
			return [.. transforms];

		return [];
	}

	private HashSet<string> BuildDisabledSetForTransform(PlanKind plan, List<string> transforms, string transform)
	{
		if (plan == PlanKind.DisableOne)
			return [transform];

		var disabled = new HashSet<string>(transforms, StringComparer.Ordinal);
		disabled.Remove(transform);
		return disabled;
	}

	private IterationResult ExecuteIteration(int iterationNumber)
	{
		using var assertCapture = new AssertCaptureScope();
		OutputIterationExecutionStatus("Before", iterationNumber);

		try
		{
			using var unitTestEngine = new UnitTestEngine(mosaSettings, OutputStatus, CreateCompilerHooks);
			if (unitTestEngine.IsAborted)
			{
				CaptureCompilationFailure(unitTestEngine.CompilationFailure);
				OutputStatusBisector("Iteration compiler run aborted. Treating as FAIL.");
				OutputIterationExecutionStatus("After", iterationNumber);
				return new IterationResult(false);
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

			OutputIterationExecutionStatus("After", iterationNumber);
			return new IterationResult(passed);
		}
		catch (AssertFailureException ex)
		{
			OutputStatusBisector($"Debug.Assert captured and treated as FAIL: {ex.Message}");
			OutputIterationExecutionStatus("After", iterationNumber);
			return new IterationResult(false);
		}
		catch (Exception ex)
		{
			OutputStatusBisector($"Iteration exception treated as FAIL: {ex.Message}");
			OutputStatusBisector($"Iteration exception stack: {ex.StackTrace}");
			OutputIterationExecutionStatus("After", iterationNumber);
			return new IterationResult(false);
		}
	}

	private void OutputIterationExecutionStatus(string phase, int iterationNumber)
	{
		OutputStatusBisector($"{phase} Unit Tests | Iteration: {iterationNumber} | Transforms: {observedTransformNames.Count} | Disabled: {effectiveDisabledTransformNames.Count}");
	}

	private CompilerHooks CreateCompilerHooks()
	{
		return new CompilerHooks
		{
			NotifyTransformObserved = NotifyTransformObserved,
			IsTransformDisabled = IsTransformDisabled,
		};
	}

	private void NotifyTransformObserved(string stageName, string transformName, string methodFullName)
	{
		if (!string.Equals(stageName, mosaSettings.BisectorStage, StringComparison.Ordinal))
			return;

		// When bisector is active, observe all transforms regardless of filter
		// The filter is only for limiting which tests run, not which transforms are observed during bisection
		if (bisector == null && observeFilterOnly && !methodFullName.Contains(unitTestFilter, StringComparison.Ordinal))
			return;

		lock (transformDiscoveryLock)
		{
			var observed = observedTransformNames.Add(transformName);
			observedTransformCounts[transformName] = observedTransformCounts.TryGetValue(transformName, out var count) ? count + 1 : 1;

			if (observed && bisector != null && !forcedDisabledTransformNames.Contains(transformName))
				bisector.ObserveItem(transformName);
		}
	}

	private bool IsTransformDisabled(string stageName, string transformName)
	{
		if (!string.Equals(stageName, mosaSettings.BisectorStage, StringComparison.Ordinal))
			return false;

		return effectiveDisabledTransformNames.Contains(transformName);
	}

	private void LoadForcedDisabledTransforms()
	{
		forcedDisabledTransformNames = [];

		var filename = mosaSettings.BisectorDisabledTransformsFile;
		if (string.IsNullOrWhiteSpace(filename))
			return;

		if (!File.Exists(filename))
			throw new InvalidOperationException($"Disabled transforms file does not exist: {filename}");

		foreach (var line in File.ReadLines(filename))
		{
			var text = line.Trim();

			if (text.Length == 0)
				continue;

			if (text.StartsWith("#", StringComparison.Ordinal) || text.StartsWith("//", StringComparison.Ordinal))
				continue;

			forcedDisabledTransformNames.Add(text);
		}

		OutputStatusBisector($"Forced Disabled Transforms File: {filename}");
		OutputStatusBisector($"Forced Disabled Transforms Loaded: {forcedDisabledTransformNames.Count}");
	}

	private void RebuildEffectiveDisabledSet()
	{
		effectiveDisabledTransformNames = [.. forcedDisabledTransformNames];

		foreach (var transformName in bisectorDisabledTransformNames)
			effectiveDisabledTransformNames.Add(transformName);
	}

	private void ReportForcedDisabledNotObserved()
	{
		if (forcedDisabledTransformNames.Count == 0)
			return;

		var notObserved = forcedDisabledTransformNames.Where(name => !observedTransformNames.Contains(name)).OrderBy(name => name).ToList();
		if (notObserved.Count == 0)
			return;

		OutputStatusBisector($"WARNING: {notObserved.Count} forced-disabled transforms were not observed in selected stage:");
		foreach (var name in notObserved)
		{
			OutputStatusBisector($"  {name}");
		}
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

			unitTests.Add(unitTest);
		}

		return unitTests;
	}

	private void PrintDisabledTransforms()
	{
		OutputStatusBisector($"Forced Disabled: {forcedDisabledTransformNames.Count}");
		OutputStatusBisector($"Session Disabled: {bisectorDisabledTransformNames.Count}");
		OutputStatusBisector($"Effective Disabled: {effectiveDisabledTransformNames.Count}");
	}

	private void PrintFinalReport(PlanKind plan, BisectorState state)
	{
		OutputStatusBisector($"Plan complete: {plan}");
		OutputStatusBisector($"Final Stage: {mosaSettings.BisectorStage}");
		OutputStatusBisector($"Baseline: {(state.BaselinePassed ? "PASS" : "FAIL")}");
		OutputStatusBisector($"Iterations: {state.TotalIterationCount}");

		OutputStatusBisector($"Passed Iterations: {state.PassCount}");
		OutputStatusBisector($"Failed Iterations: {state.TotalIterationCount - state.PassCount}");

		OutputStatusBisector("Failed Transforms:");
		foreach (var result in state.Results.Where(r => !r.Passed).OrderBy(r => r.Transform))
		{
			OutputStatusBisector($"  {result.Transform}");
		}

		PrintPassAndIterationCounts(state);
	}

	private void OutputStatusBisector(string status)
	{
		Console.WriteLine($"{stopwatch.Elapsed.TotalSeconds:00.00} | [Bisector] {status}");
	}

	private void OutputStatus(string status)
	{
		Console.WriteLine($"{stopwatch.Elapsed.TotalSeconds:00.00} | {status}");
	}

	private void CaptureCompilationFailure(string failure)
	{
		if (string.IsNullOrWhiteSpace(failure))
			return;

		hasCompilationFailure = true;
		if (string.Equals(lastCompilationFailure, failure, StringComparison.Ordinal))
			return;

		lastCompilationFailure = failure;
		OutputStatusBisector("Compilation failure details:");
		foreach (var line in failure.Split([Environment.NewLine], StringSplitOptions.None))
		{
			if (!string.IsNullOrWhiteSpace(line))
				OutputStatusBisector($"  {line}");
		}
	}

	private void WriteFailureReviewFile(string stateFile, PlanKind plan, BisectorState state)
	{
		var reviewFile = stateFile + ".failures.txt";
		var lines = new List<string>
		{
			$"Unit Test Bisector Failure Review",
			$"Stage: {mosaSettings.BisectorStage}",
			$"Plan: {plan}",
			$"State File: {stateFile}",
			$"Baseline: {(state.BaselinePassed ? "PASS" : "FAIL")}",
			$"Completed: {state.Completed}",
			$"Iteration: {state.IterationNumber}",
			$"Progress: {state.NextIndex}/{state.ObservedTransforms.Count}",
			$"Total Iterations: {state.TotalIterationCount}",
			$"Total Passes: {state.PassCount}",
			$"Total Failures: {state.TotalIterationCount - state.PassCount}",
			string.Empty,
		};

		var failedResults = state.Results.Where(r => !r.Passed).OrderBy(r => r.Transform).ToList();
		lines.Add($"Failed Iterations: {failedResults.Count}");
		lines.Add(string.Empty);

		if (failedResults.Count == 0)
		{
			lines.Add("No failing iterations recorded.");
		}
		else
		{
			foreach (var result in failedResults)
			{
				lines.Add($"Transform: {result.Transform}");
				lines.Add($"  Result: FAIL");
				lines.Add("  Disabled Transforms:");

				foreach (var disabled in result.DisabledTransforms.OrderBy(x => x))
				{
					lines.Add($"    {disabled}");
				}

				lines.Add(string.Empty);
			}
		}

		File.WriteAllLines(reviewFile, lines);
	}

	private static List<string> BuildIterationSequence(List<string> observed, Dictionary<string, int> counts, OrderKind order, int randomSeed)
	{
		if (order == OrderKind.CountAscending)
			return observed.OrderBy(name => counts.TryGetValue(name, out var count) ? count : 0).ThenBy(name => name).ToList();

		if (order == OrderKind.Random)
			return Shuffle(observed, randomSeed);

		return observed;
	}

	private int ResolveRandomSeed(int existingSeed)
	{
		if (existingSeed != 0)
			return existingSeed;

		if (mosaSettings.BisectorRandomSeed != 0)
			return mosaSettings.BisectorRandomSeed;

		return Random.Shared.Next(1, int.MaxValue);
	}

	private static List<string> Shuffle(List<string> items, int seed)
	{
		var copy = items.ToList();
		var random = new Random(seed);

		for (var i = copy.Count - 1; i > 0; i--)
		{
			var j = random.Next(i + 1);
			(copy[i], copy[j]) = (copy[j], copy[i]);
		}

		return copy;
	}

	private static HashSet<string> BuildRandomDisabledSet(List<string> transforms, int seed, int index)
	{
		var random = new Random(seed + (index * 7919));
		var disabled = new HashSet<string>(StringComparer.Ordinal);

		foreach (var transform in transforms)
		{
			if (random.Next(2) == 0)
				disabled.Add(transform);
		}

		return disabled;
	}

	private int ExecuteDeterministicPlan(string stateFile, BisectorState state, PlanKind plan)
	{
		if (!state.BaselineCompleted)
		{
			bisectorDisabledTransformNames = BuildDisabledSetForBaseline(plan, state.ObservedTransforms);
			RebuildEffectiveDisabledSet();
			state.IterationNumber = Constant.BaselineIterationNumber;

			OutputStatusBisector("Running baseline iteration...");
			OutputStatusBisector($"Iteration: {state.IterationNumber}");
			PrintDisabledTransforms();

			var baselineResult = ExecuteIteration(state.IterationNumber);
			state.BaselineCompleted = true;
			state.BaselinePassed = baselineResult.Passed;
			UpdateCounters(state, baselineResult.Passed);
			SaveState(stateFile, state);

			OutputStatusBisector($"Baseline Result: {(baselineResult.Passed ? "PASS" : "FAIL")}");
			PrintPassAndIterationCounts(state);

			if (hasCompilationFailure)
			{
				SetLastExit(state, Constant.ExitKindFailure, 1);
				SaveState(stateFile, state);
				WriteFailureReviewFile(stateFile, plan, state);
				return 1;
			}
		}
		else
		{
			OutputStatusBisector($"Resuming after baseline. Baseline Result: {(state.BaselinePassed ? "PASS" : "FAIL")}");
			OutputStatusBisector($"Iteration: {state.IterationNumber}");
			PrintPassAndIterationCounts(state);
		}

		while (state.NextIndex < state.ObservedTransforms.Count)
		{
			var transform = state.ObservedTransforms[state.NextIndex];
			var iterationNumber = state.NextIndex + 1;
			state.IterationNumber = iterationNumber;
			bisectorDisabledTransformNames = BuildDisabledSetForTransform(plan, state.ObservedTransforms, transform);
			RebuildEffectiveDisabledSet();
			var disabledSnapshot = effectiveDisabledTransformNames.OrderBy(x => x).ToList();

			OutputStatusBisector($"Iteration {iterationNumber}/{state.ObservedTransforms.Count}");
			OutputStatusBisector($"Iteration: {state.IterationNumber}");
			OutputStatusBisector($"Transform: {transform}");
			PrintDisabledTransforms();

			var iterationResult = ExecuteIteration(state.IterationNumber);
			state.Results.Add(new PlanResult
			{
				Transform = transform,
				Passed = iterationResult.Passed,
				DisabledTransforms = disabledSnapshot,
			});
			UpdateCounters(state, iterationResult.Passed);
			state.NextIndex++;
			SaveState(stateFile, state);
			WriteFailureReviewFile(stateFile, plan, state);

			OutputStatusBisector($"Iteration Result: {(iterationResult.Passed ? "PASS" : "FAIL")}");
			PrintPassAndIterationCounts(state);
			if (!iterationResult.Passed)
			{
				OutputStatusBisector($"Failure state captured for review: transform={transform}, disabled={disabledSnapshot.Count}");
			}

			if (hasCompilationFailure)
			{
				SetLastExit(state, Constant.ExitKindFailure, 1);
				SaveState(stateFile, state);
				return 1;
			}

			if (mosaSettings.BisectorWorkerIteration && state.NextIndex < state.ObservedTransforms.Count)
			{
				SetLastExit(state, Constant.ExitKindContinue, Constant.WorkerContinueExitCode);
				SaveState(stateFile, state);
				return Constant.WorkerContinueExitCode;
			}
		}

		state.Completed = true;
		SetLastExit(state, Constant.ExitKindCompleted, 0);
		SaveState(stateFile, state);
		WriteFailureReviewFile(stateFile, plan, state);

		PrintFinalReport(plan, state);
		return 0;
	}

	private int ExecuteRandomComboPlan(string stateFile, BisectorState state)
	{
		if (!state.BaselineCompleted)
		{
			bisectorDisabledTransformNames = [];
			RebuildEffectiveDisabledSet();
			state.IterationNumber = Constant.BaselineIterationNumber;

			OutputStatusBisector("Running baseline iteration...");
			OutputStatusBisector($"Iteration: {state.IterationNumber}");
			PrintDisabledTransforms();

			var baselineResult = ExecuteIteration(state.IterationNumber);
			state.BaselineCompleted = true;
			state.BaselinePassed = baselineResult.Passed;
			UpdateCounters(state, baselineResult.Passed);
			SaveState(stateFile, state);
			OutputStatusBisector($"Baseline Result: {(baselineResult.Passed ? "PASS" : "FAIL")}{Environment.NewLine}");
			PrintPassAndIterationCounts(state);

			if (hasCompilationFailure)
			{
				SetLastExit(state, Constant.ExitKindFailure, 1);
				SaveState(stateFile, state);
				WriteFailureReviewFile(stateFile, PlanKind.RandomCombo, state);
				return 1;
			}

			if (mosaSettings.BisectorWorkerIteration)
			{
				SetLastExit(state, Constant.ExitKindContinue, Constant.WorkerContinueExitCode);
				SaveState(stateFile, state);
				return Constant.WorkerContinueExitCode;
			}
		}

		var iterationsThisRun = mosaSettings.BisectorWorkerIteration ? 1 : Math.Max(1, mosaSettings.BisectorIterations);

		for (var i = 0; i < iterationsThisRun; i++)
		{
			var iterationNumber = state.NextIndex + 1;
			state.IterationNumber = iterationNumber;
			bisectorDisabledTransformNames = BuildRandomDisabledSet(state.ObservedTransforms, state.RandomSeed, state.NextIndex);
			RebuildEffectiveDisabledSet();
			var disabledSnapshot = effectiveDisabledTransformNames.OrderBy(x => x).ToList();

			OutputStatusBisector($"Random Iteration {iterationNumber}");
			OutputStatusBisector($"Iteration: {state.IterationNumber}");
			PrintDisabledTransforms();

			var iterationResult = ExecuteIteration(state.IterationNumber);
			state.Results.Add(new PlanResult
			{
				Transform = $"random-{iterationNumber}",
				Passed = iterationResult.Passed,
				DisabledTransforms = disabledSnapshot,
			});

			UpdateCounters(state, iterationResult.Passed);
			state.NextIndex++;
			SaveState(stateFile, state);
			WriteFailureReviewFile(stateFile, PlanKind.RandomCombo, state);

			OutputStatusBisector($"Iteration Result: {(iterationResult.Passed ? "PASS" : "FAIL")}");
			PrintPassAndIterationCounts(state);

			if (hasCompilationFailure)
			{
				SetLastExit(state, Constant.ExitKindFailure, 1);
				SaveState(stateFile, state);
				return 1;
			}
		}

		if (mosaSettings.BisectorWorkerIteration)
		{
			SetLastExit(state, Constant.ExitKindContinue, Constant.WorkerContinueExitCode);
			SaveState(stateFile, state);
			return Constant.WorkerContinueExitCode;
		}

		SetLastExit(state, Constant.ExitKindCompleted, 0);
		SaveState(stateFile, state);
		return 0;
	}
}
