// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Text.Json;
using Mosa.Compiler.Framework;
using Mosa.Utility.Configuration;
using Mosa.Utility.UnitTests;

namespace Mosa.Utility.UnitTestBisector;

public sealed partial class UnitTestBisectorSystem
{
	private readonly record struct IterationResult(bool Passed);

	private readonly Stopwatch stopwatch = new();
	private readonly MosaSettings mosaSettings = new();
	private readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

	private bool hasCompilationFailure;
	private bool hasRestartsExceeded;
	private string lastCompilationFailure;
	private string unitTestFilter;

	private readonly object registerTransformLock = new();

	private HashSet<string> registeredTransformNames = [];
	private HashSet<string> effectiveDisabledTransformNames = [];

	#region Entry Point

	public int Start(string[] args)
	{
		try
		{
			OutputStatusBisector($"Bisector started");

			mosaSettings.LoadArguments(args);
			mosaSettings.UnitTestFailFast = true;
			unitTestFilter = mosaSettings.UnitTestFilter;
			hasCompilationFailure = false;
			hasRestartsExceeded = false;
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

			var stageDisplay = string.IsNullOrEmpty(mosaSettings.BisectorStage) ? "All" : mosaSettings.BisectorStage;

			OutputStatusBisector($"State File: {stateFile}");
			OutputStatusBisector($"Stage: {stageDisplay} | Plan: {plan} | Order: {order}");

			OutputStatus("Discovering Unit Tests...");
			var discoveredUnitTests = UnitTestRunner.Discover(unitTestFilter);
			OutputStatus($"Found Tests: {discoveredUnitTests.Count} in {stopwatch.ElapsedMilliseconds / 1000.0:F1} secs");

			if (discoveredUnitTests.Count == 0)
			{
				OutputStatus("ERROR: No tests matched the filter.");
				return 1;
			}

			if (isBisectorPlan)
			{
				var bisectorState = LoadOrCreateState(stateFile, plan);
				EnsureStateCompatibility(bisectorState, plan, OrderKind.Unspecified);

				if (mosaSettings.BisectorWorkerIteration)
					return ExecuteBisectorPlan(plan, stateFile, bisectorState, discoveredUnitTests);

				// Run all iterations in-process when not running under the supervisor
				while (true)
				{
					ResetIterationState();
					bisectorState = LoadOrCreateState(stateFile, plan);
					var result = ExecuteBisectorPlan(plan, stateFile, bisectorState, discoveredUnitTests);
					if (result != Constant.WorkerContinueExitCode)
						return result;
				}
			}

			var state = LoadOrCreateState(stateFile, plan);
			EnsureStateCompatibility(state, plan, order);

			if (state.Transforms.Count == 0)
			{
				OutputStatusBisector("Running transform discovery iteration...");
				effectiveDisabledTransformNames = [];

				var discoveryResult = ExecuteIteration(state.IterationNumber, discoveredUnitTests);
				OutputStatusBisector($"Discovery Iteration: {(discoveryResult.Passed ? "PASS" : "FAIL")}");
				OutputIterationStatus(state.IterationNumber);

				var transforms = registeredTransformNames
					.OrderBy(name => name)
					.ToList();

				if (transforms.Count == 0)
				{
					OutputStatusBisector("No transforms registered for selected stage/plan/options. Nothing to bisect; marking session complete.");
					state.Completed = true;
					SetLastExit(state, Constant.ExitKindCompleted, 0);
					SaveState(stateFile, state);
					WriteFailureReviewFile(stateFile, plan, state);
					return 0;
				}

				state.Order = order;
				state.RandomSeed = ResolveRandomSeed(state.RandomSeed);
				state.Transforms = BuildIterationSequence(transforms, order, state.RandomSeed);
				state.IterationNumber = Constant.BaselineIterationNumber;
				SaveState(stateFile, state);

				OutputStatusBisector($"Registered transforms for plan: {state.Transforms.Count}");
			}
			else
			{
				state.Order = state.Order == OrderKind.Unspecified ? order : state.Order;
				state.RandomSeed = ResolveRandomSeed(state.RandomSeed);

				OutputStatusBisector($"Loaded transforms from state: {state.Transforms.Count}");
			}

			return plan == PlanKind.RandomCombo
				? ExecuteRandomComboPlan(stateFile, state, discoveredUnitTests)
				: ExecuteDeterministicPlan(stateFile, state, plan, discoveredUnitTests);
		}
		catch (Exception ex)
		{
			OutputStatus($"Exception: {ex.Message}");
			OutputStatus($"Exception: {ex.StackTrace}");
			return 1;
		}
	}

	private void ResetIterationState()
	{
		hasCompilationFailure = false;
		hasRestartsExceeded = false;
		lastCompilationFailure = null;
		registeredTransformNames = [];
		effectiveDisabledTransformNames = [];
	}

	#endregion Entry Point

	#region Plan Execution

	private int ExecuteBisectorPlan(PlanKind plan, string stateFile, BisectorState state, List<UnitTestInfo> discoveredUnitTests)
	{
		if (!IsBisectorPlan(plan))
			throw new InvalidOperationException($"Plan '{plan}' is not a bisector plan.");

		if (state.Completed)
		{
			OutputStatusBisector("Bisector state already completed.");
			return 0;
		}

		// Phase 1: discovery / baseline
		if (!state.BaselineCompleted || state.Transforms.Count == 0)
		{
			OutputStatusBisector("Running transform discovery iteration...");
			effectiveDisabledTransformNames = [];

			var discoveryResult = ExecuteIteration(state.IterationNumber, discoveredUnitTests);
			state.BaselineCompleted = true;
			state.BaselinePassed = discoveryResult.Passed;
			RecalculateCounters(state);
			OutputStatusBisector($"Discovery Iteration: {(discoveryResult.Passed ? "PASS" : "FAIL")}");
			OutputIterationStatus(state.IterationNumber, state);

			if (hasCompilationFailure || hasRestartsExceeded)
			{
				SaveState(stateFile, state);
				WriteFailureReviewFile(stateFile, plan, state);
				return 1;
			}

			var transforms = registeredTransformNames
				.OrderBy(name => name)
				.ToList();

			if (transforms.Count == 0)
			{
				OutputStatusBisector("No transforms registered for selected stage/plan/options. Nothing to bisect; marking session complete.");
				state.Completed = true;
				SetLastExit(state, Constant.ExitKindCompleted, 0);
				SaveState(stateFile, state);
				WriteFailureReviewFile(stateFile, plan, state);
				return 0;
			}

			OutputStatusBisector($"Registered Transforms: {transforms.Count}");

			state.Transforms = transforms;
			state.Results = [];
			state.NextIndex = 0;
			state.BisectorSessionStarted = false;
			state.MaskingPreCheckCompleted = false;

			SetLastExit(state, Constant.ExitKindContinue, Constant.WorkerContinueExitCode);
			SaveState(stateFile, state);
			return Constant.WorkerContinueExitCode;
		}

		OutputStatusBisector($"Loaded transforms from state: {state.Transforms.Count}");

		var baselineResult = new IterationResult(state.BaselinePassed);

		// Phase 2: determine session name and invert flag
		string sessionName;
		bool invertOutcome;

		if (plan == PlanKind.FailureInducing)
		{
			sessionName = "Failure-Inducing";
			invertOutcome = false;
		}
		else if (!baselineResult.Passed)
		{
			// Masking requires a passing baseline; fall back to failure-inducing
			OutputStatusBisector("Masking baseline is FAIL. Falling back to failure-inducing bisector to narrow failing transforms.");
			sessionName = "Failure-Inducing";
			invertOutcome = false;
		}
		else
		{
			// Phase 2a: masking pre-check (run once, then continue)
			if (!state.MaskingPreCheckCompleted)
			{
				OutputStatusBisector("Running masking pre-check (all transforms disabled)...");
				effectiveDisabledTransformNames = [.. state.Transforms];
				var preCheckResult = ExecuteIteration(state.IterationNumber, discoveredUnitTests);
				effectiveDisabledTransformNames = [];

				if (hasCompilationFailure || hasRestartsExceeded)
				{
					SaveState(stateFile, state);
					WriteFailureReviewFile(stateFile, plan, state);
					return 1;
				}

				state.MaskingPreCheckCompleted = true;
				state.MaskingPreCheckPassed = preCheckResult.Passed;
				OutputStatusBisector($"Masking Pre-Check -> Actual: {(preCheckResult.Passed ? "PASS" : "FAIL")}");
				OutputIterationStatus(state.IterationNumber);

				if (preCheckResult.Passed)
				{
					state.Completed = true;
					SetLastExit(state, Constant.ExitKindCompleted, 0);
					SaveState(stateFile, state);
					WriteFailureReviewFile(stateFile, plan, state);
					OutputStatusBisector("Masking pre-check passed (all-disabled still passes). No masking transforms identified.");
					return 0;
				}

				SetLastExit(state, Constant.ExitKindContinue, Constant.WorkerContinueExitCode);
				SaveState(stateFile, state);
				return Constant.WorkerContinueExitCode;
			}

			if (state.MaskingPreCheckPassed)
			{
				state.Completed = true;
				SetLastExit(state, Constant.ExitKindCompleted, 0);
				SaveState(stateFile, state);
				WriteFailureReviewFile(stateFile, plan, state);
				return 0;
			}

			sessionName = "Masking";
			invertOutcome = true;
		}

		// Persist session name/invert for resume correctness
		if (!state.BisectorSessionStarted)
		{
			state.BisectorSessionName = sessionName;
			state.BisectorSessionInvertOutcome = invertOutcome;
			state.BisectorSessionStarted = true;
			state.Results = [];
			state.NextIndex = 0;
			SaveState(stateFile, state);
		}
		else
		{
			sessionName = state.BisectorSessionName ?? sessionName;
			invertOutcome = state.BisectorSessionInvertOutcome;
		}

		return RunBisectorSessionIteration(stateFile, state, sessionName, invertOutcome, baselineResult, discoveredUnitTests);
	}

	private int RunBisectorSessionIteration(string stateFile, BisectorState state, string sessionName, bool invertOutcome, IterationResult baselineResult, List<UnitTestInfo> discoveredUnitTests)
	{
		effectiveDisabledTransformNames = [];

		var enablePairwise = mosaSettings.BisectorPairwise || invertOutcome;
		var bisector = new Bisector<string>(state.Transforms, enablePairwise: enablePairwise);
		var reportedBadItems = new HashSet<string>(StringComparer.Ordinal);

		// Feed bisector the baseline
		bisector.GetNextDisabledItems();
		var mappedBaseline = MapOutcome(baselineResult.Passed, invertOutcome);
		bisector.AcceptResult(mappedBaseline);
		OutputStatusBisector($"{sessionName} Baseline -> Actual: {(baselineResult.Passed ? "PASS" : "FAIL")}, Mapped: {(mappedBaseline ? "PASS" : "FAIL")}");

		// Replay prior session results to restore bisector state
		for (var i = 0; i < state.Results.Count; i++)
		{
			var priorResult = state.Results[i];
			bisector.GetNextDisabledItems();
			bisector.AcceptResult(MapOutcome(priorResult.Passed, invertOutcome));
		}

		OutputNewlyConfirmedBadItems(sessionName, bisector, reportedBadItems);

		if (bisector.IsComplete)
		{
			OutputStatusBisector($"{sessionName} bisector complete.");
			OutputFinalReport(sessionName, bisector);
			state.Completed = true;
			SetLastExit(state, Constant.ExitKindCompleted, 0);
			SaveState(stateFile, state);
			WriteFailureReviewFile(stateFile, state.Plan, state);
			return 0;
		}

		// Run one iteration
		effectiveDisabledTransformNames = [.. bisector.GetNextDisabledItems()];
		var status = bisector.GetStatus();
		state.IterationNumber = status.Iteration + 1;
		OutputIterationHeader(sessionName, status);

		var iterationResult = ExecuteIteration(state.IterationNumber, discoveredUnitTests);
		var mappedResult = MapOutcome(iterationResult.Passed, invertOutcome);

		state.Results.Add(new PlanResult
		{
			Transform = $"iter-{status.Iteration + 1}-{status.Level}-{status.Phase}",
			Passed = iterationResult.Passed,
			DisabledTransforms = effectiveDisabledTransformNames.OrderBy(x => x).ToList(),
		});
		state.NextIndex = state.Results.Count;

		SaveState(stateFile, state);
		WriteFailureReviewFile(stateFile, state.Plan, state);

		bisector.AcceptResult(mappedResult);
		OutputStatusBisector($"Iteration Result -> Actual: {(iterationResult.Passed ? "PASS" : "FAIL")}, Mapped: {(mappedResult ? "PASS" : "FAIL")}");
		OutputIterationStatus(state.IterationNumber);
		OutputNewlyConfirmedBadItems(sessionName, bisector, reportedBadItems);
		OutputBisectorStatus(bisector.GetStatus());

		if (hasRestartsExceeded)
		{
			SetLastExit(state, Constant.ExitKindFailure, 1);
			SaveState(stateFile, state);
			WriteFailureReviewFile(stateFile, state.Plan, state);
			return 1;
		}

		if (bisector.IsComplete)
		{
			OutputStatusBisector($"{sessionName} bisector complete.");
			OutputFinalReport(sessionName, bisector);
			state.Completed = true;
			SetLastExit(state, Constant.ExitKindCompleted, 0);
			SaveState(stateFile, state);
			WriteFailureReviewFile(stateFile, state.Plan, state);
			return 0;
		}

		SetLastExit(state, Constant.ExitKindContinue, Constant.WorkerContinueExitCode);
		SaveState(stateFile, state);
		return Constant.WorkerContinueExitCode;
	}

	private int ExecuteDeterministicPlan(string stateFile, BisectorState state, PlanKind plan, List<UnitTestInfo> discoveredUnitTests)
	{
		if (!state.BaselineCompleted)
		{
			effectiveDisabledTransformNames = BuildDisabledSetForBaseline(plan, state.Transforms);
			state.IterationNumber = Constant.BaselineIterationNumber;

			OutputStatusBisector("Running baseline iteration...");
			OutputStatusBisector($"Iteration: {state.IterationNumber}");

			var baselineResult = ExecuteIteration(state.IterationNumber, discoveredUnitTests);
			state.BaselineCompleted = true;
			state.BaselinePassed = baselineResult.Passed;
			SaveState(stateFile, state);

			OutputStatusBisector($"Baseline Result: {(baselineResult.Passed ? "PASS" : "FAIL")}");
			OutputIterationStatus(state.IterationNumber, state);

			if (hasCompilationFailure || hasRestartsExceeded)
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
			OutputIterationStatus(state.IterationNumber, state, state.Transforms.Count);
		}

		while (state.NextIndex < state.Transforms.Count)
		{
			var transform = state.Transforms[state.NextIndex];
			state.IterationNumber = state.NextIndex + 1;
			effectiveDisabledTransformNames = BuildDisabledSetForTransform(plan, state.Transforms, transform);
			var disabledSnapshot = effectiveDisabledTransformNames.OrderBy(x => x).ToList();

			OutputStatusBisector($"Transform: {transform}");

			var iterationResult = ExecuteIteration(state.IterationNumber, discoveredUnitTests);
			state.Results.Add(new PlanResult
			{
				Transform = transform,
				Passed = iterationResult.Passed,
				DisabledTransforms = disabledSnapshot,
			});
			state.NextIndex++;
			SaveState(stateFile, state);
			WriteFailureReviewFile(stateFile, plan, state);

			OutputStatusBisector($"Iteration Result: {(iterationResult.Passed ? "PASS" : "FAIL")}");
			if (!iterationResult.Passed)
				OutputStatusBisector($"Failure state captured for review: transform={transform}, disabled={disabledSnapshot.Count}");
			OutputIterationStatus(state.IterationNumber, state, state.Transforms.Count);

			if (hasCompilationFailure || hasRestartsExceeded)
			{
				SetLastExit(state, Constant.ExitKindFailure, 1);
				SaveState(stateFile, state);
				WriteFailureReviewFile(stateFile, plan, state);
				return 1;
			}

			if (mosaSettings.BisectorWorkerIteration && state.NextIndex < state.Transforms.Count)
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

		OutputFinalReport(plan, state);
		return 0;
	}

	private int ExecuteRandomComboPlan(string stateFile, BisectorState state, List<UnitTestInfo> discoveredUnitTests)
	{
		if (!state.BaselineCompleted)
		{
			effectiveDisabledTransformNames = [];
			state.IterationNumber = Constant.BaselineIterationNumber;

			OutputStatusBisector("Running baseline iteration...");

			var baselineResult = ExecuteIteration(state.IterationNumber, discoveredUnitTests);
			state.BaselineCompleted = true;
			state.BaselinePassed = baselineResult.Passed;
			SaveState(stateFile, state);

			OutputStatusBisector($"Baseline Result: {(baselineResult.Passed ? "PASS" : "FAIL")}");
			OutputIterationStatus(state.IterationNumber, state);

			if (hasCompilationFailure || hasRestartsExceeded)
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
			effectiveDisabledTransformNames = BuildRandomDisabledSet(state.Transforms, state.RandomSeed, state.NextIndex);
			var disabledSnapshot = effectiveDisabledTransformNames.OrderBy(x => x).ToList();

			OutputStatusBisector($"Random Iteration {iterationNumber}");

			var iterationResult = ExecuteIteration(state.IterationNumber, discoveredUnitTests);
			state.Results.Add(new PlanResult
			{
				Transform = $"random-{iterationNumber}",
				Passed = iterationResult.Passed,
				DisabledTransforms = disabledSnapshot,
			});

			state.NextIndex++;
			SaveState(stateFile, state);
			WriteFailureReviewFile(stateFile, PlanKind.RandomCombo, state);

			OutputStatusBisector($"Iteration Result: {(iterationResult.Passed ? "PASS" : "FAIL")}");
			OutputIterationStatus(state.IterationNumber, state);

			if (hasCompilationFailure || hasRestartsExceeded)
			{
				SetLastExit(state, Constant.ExitKindFailure, 1);
				SaveState(stateFile, state);
				WriteFailureReviewFile(stateFile, PlanKind.RandomCombo, state);
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

	#endregion Plan Execution

	#region Iteration Execution

	private IterationResult ExecuteIteration(int iterationNumber, List<UnitTestInfo> discoveredUnitTests)
	{
		using var assertCapture = new AssertCaptureScope();
		OutputIterationStatus(iterationNumber);

		try
		{
			using var unitTestEngine = new UnitTestEngine(mosaSettings, OutputStatus, CreateCompilerHooks);
			if (unitTestEngine.IsAborted)
			{
				if (!string.IsNullOrWhiteSpace(unitTestEngine.CompilationFailure))
				{
					CaptureCompilationFailure(unitTestEngine.CompilationFailure);
					OutputStatusBisector("Iteration compiler run aborted. Treating as FAIL.");
				}
				else
				{
					hasRestartsExceeded = true;
					OutputStatusBisector("Iteration aborted: unit test system restarts exceeded. Treating as FAIL.");
				}
				return new IterationResult(false);
			}

			var unitTests = UnitTestRunner.Run(unitTestEngine, discoveredUnitTests);

			if (unitTestEngine.IsAborted)
			{
				hasRestartsExceeded = true;
				OutputStatusBisector("Iteration aborted during run: unit test system restarts exceeded. Treating as FAIL.");
				unitTestEngine.Terminate();
				return new IterationResult(false);
			}

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

			return new IterationResult(passed);
		}
		catch (AssertFailureException ex)
		{
			OutputStatusBisector($"Debug.Assert captured and treated as FAIL: {ex.Message}");
			return new IterationResult(false);
		}
		catch (Exception ex)
		{
			OutputStatusBisector($"Iteration exception treated as FAIL: {ex.Message}");
			OutputStatusBisector($"Iteration exception stack: {ex.StackTrace}");
			return new IterationResult(false);
		}
	}

	private CompilerHooks CreateCompilerHooks()
	{
		return new CompilerHooks
		{
			RegisterTransform = RegisterTransform,
			IsTransformDisabled = IsTransformDisabled,
		};
	}

	private void RegisterTransform(string stageName, string transformName)
	{
		if (!string.IsNullOrEmpty(mosaSettings.BisectorStage) && !string.Equals(stageName, mosaSettings.BisectorStage, StringComparison.Ordinal))
			return;

		lock (registerTransformLock)
		{
			registeredTransformNames.Add(transformName);
		}
	}

	private bool IsTransformDisabled(string stageName, string transformName)
	{
		if (!string.IsNullOrEmpty(mosaSettings.BisectorStage) && !string.Equals(stageName, mosaSettings.BisectorStage, StringComparison.Ordinal))
			return false;

		return effectiveDisabledTransformNames.Contains(transformName);
	}

	#endregion Iteration Execution

	#region State Management

	private BisectorState LoadOrCreateState(string stateFile, PlanKind plan)
	{
		if (!File.Exists(stateFile))
		{
			return new BisectorState
			{
				Plan = plan,
				StageName = mosaSettings.BisectorStage,
				UnitTestFilter = unitTestFilter,
				IterationNumber = Constant.BaselineIterationNumber,
				LastExitKind = Constant.ExitKindUnknown,
				LastExitCode = 0,
			};
		}

		var content = File.ReadAllText(stateFile);
		var state = JsonSerializer.Deserialize<BisectorState>(content);
		if (state == null)
			throw new InvalidOperationException($"Unable to deserialize state file: {stateFile}");

		state.Results ??= [];
		state.Transforms ??= [];
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

	private static void SetLastExit(BisectorState state, string exitKind, int exitCode)
	{
		state.LastExitKind = exitKind;
		state.LastExitCode = exitCode;
	}

	private void EnsureStateCompatibility(BisectorState state, PlanKind plan, OrderKind order)
	{
		if (!string.Equals(state.StageName, mosaSettings.BisectorStage, StringComparison.Ordinal))
			throw new InvalidOperationException($"State file stage does not match current {Constant.OptionBisectStage}.");

		if (state.Plan != plan)
			throw new InvalidOperationException($"State file plan does not match current {Constant.OptionBisectPlan}.");

		if (!string.Equals(state.UnitTestFilter, unitTestFilter, StringComparison.Ordinal))
			throw new InvalidOperationException($"State file UnitTest filter does not match current {Constant.OptionFilter}.");

		if (state.Order == OrderKind.Unspecified)
			state.Order = order;

		if (state.Order != order)
			throw new InvalidOperationException($"State file order does not match current {Constant.OptionBisectOrder}.");
	}

	private static void RecalculateCounters(BisectorState state)
	{
		state.TotalIterationCount = state.Results.Count
			+ (state.BaselineCompleted ? 1 : 0)
			+ (state.MaskingPreCheckCompleted ? 1 : 0);
		state.PassCount = state.Results.Count(r => r.Passed)
			+ (state.BaselineCompleted && state.BaselinePassed ? 1 : 0)
			+ (state.MaskingPreCheckCompleted && state.MaskingPreCheckPassed ? 1 : 0);
		state.FailureCount = state.TotalIterationCount - state.PassCount;
	}

	#endregion State Management

	#region Plan & Transform Helpers

	private static bool IsBisectorPlan(PlanKind plan)
	{
		return plan is PlanKind.FailureInducing or PlanKind.Masking;
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
			"random" => OrderKind.Random,
			_ => throw new InvalidOperationException($"Invalid order value '{order}'. Valid values: original, random."),
		};
	}

	private static bool MapOutcome(bool passed, bool invertOutcome)
	{
		return invertOutcome ? !passed : passed;
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

	private static List<string> BuildIterationSequence(List<string> transforms, OrderKind order, int randomSeed)
	{
		if (order == OrderKind.Random)
			return Shuffle(transforms, randomSeed);

		return transforms;
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

	private int ResolveRandomSeed(int existingSeed)
	{
		if (existingSeed != 0)
			return existingSeed;

		if (mosaSettings.BisectorRandomSeed != 0)
			return mosaSettings.BisectorRandomSeed;

		return Random.Shared.Next(1, int.MaxValue);
	}

	#endregion Plan & Transform Helpers

	#region File I/O

	private string GetFullStateFilePath()
	{
		var stateFile = mosaSettings.BisectorStateFile;

		if (!Path.IsPathRooted(stateFile))
			stateFile = Path.GetFullPath(stateFile);

		return stateFile;
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
			$"Stage: {(string.IsNullOrEmpty(mosaSettings.BisectorStage) ? "All" : mosaSettings.BisectorStage)}",
			$"Plan: {plan}",
			$"State File: {stateFile}",
			$"Baseline: {(state.BaselinePassed ? "PASS" : "FAIL")}",
			$"Completed: {state.Completed}",
			$"Iteration: {state.IterationNumber}",
			$"Progress: {state.NextIndex}/{state.Transforms.Count}",
			$"Total Iterations: {state.TotalIterationCount}",
			$"Total Passes: {state.PassCount}",
			$"Total Failures: {state.FailureCount}",
			$"Compilation Failure: {(hasCompilationFailure ? "YES" : "No")}",
			$"Restarts Exceeded: {(hasRestartsExceeded ? "YES" : "No")}",
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
					lines.Add($"    {disabled}");

				lines.Add(string.Empty);
			}
		}

		File.WriteAllLines(reviewFile, lines);
	}

	#endregion File I/O

	#region Output

	private void OutputIterationHeader(string sessionName, Bisector<string>.BisectorStatus status)
	{
		OutputStatusBisector($"{sessionName} Iteration: {status.Iteration + 1} | Level: {status.Level} | Phase: {status.Phase} | Stage: {(string.IsNullOrEmpty(mosaSettings.BisectorStage) ? "All" : mosaSettings.BisectorStage)}");
	}

	private void OutputBisectorStatus(Bisector<string>.BisectorStatus status)
	{
		OutputStatusBisector($"Iteration: {status.Iteration} | Transforms: {status.TotalItemCount} | Suspects: {status.SuspectItemCount} | BadItems: {status.ConfirmedBadItemCount} | BadPairs: {status.ConfirmedBadPairCount}");
		OutputStatusBisector($"PairwiseCompleted: {status.PairwiseTestsCompleted} | PairwiseRemaining: {status.PairwiseTestsRemaining}");
	}

	private void OutputIterationStatus(int iterationNumber, BisectorState state = null, int? totalCount = null)
	{
		var totalPart = totalCount.HasValue ? $"/{totalCount}" : string.Empty;
		var statePart = state != null ? $" | Passes: {state.PassCount} | Failures: {state.FailureCount}" : string.Empty;
		OutputStatusBisector($"Iteration: {iterationNumber}{totalPart} | Transforms: {registeredTransformNames.Count} | Disabled: {effectiveDisabledTransformNames.Count}{statePart}");
	}

	private void OutputNewlyConfirmedBadItems(string sessionName, Bisector<string> sessionBisector, HashSet<string> reportedBadItems)
	{
		foreach (var transform in sessionBisector.ConfirmedBadItems.OrderBy(t => t))
		{
			if (reportedBadItems.Add(transform))
				OutputStatusBisector($"{sessionName} Known Bad Item: {transform}");
		}
	}

	private void OutputFinalReport(string sessionName, Bisector<string> sessionBisector)
	{
		OutputStatusBisector($"{sessionName} Final Stage: {(string.IsNullOrEmpty(mosaSettings.BisectorStage) ? "All" : mosaSettings.BisectorStage)}");
		OutputStatusBisector("Confirmed Bad Items:");
		foreach (var transform in sessionBisector.ConfirmedBadItems.OrderBy(t => t))
			OutputStatusBisector($"  {transform}");

		OutputStatusBisector("Confirmed Bad Pairs:");
		foreach (var pair in sessionBisector.ConfirmedBadPairs.OrderBy(p => p.Item1).ThenBy(p => p.Item2))
			OutputStatusBisector($"  {pair.Item1} + {pair.Item2}");

		OutputStatusBisector("Remaining Suspects:");
		foreach (var transform in sessionBisector.RemainingSuspectItems.OrderBy(t => t))
			OutputStatusBisector($"  {transform}");
	}

	private void OutputFinalReport(PlanKind plan, BisectorState state)
	{
		OutputStatusBisector($"Plan complete: {plan}");
		OutputStatusBisector($"Final Stage: {(string.IsNullOrEmpty(mosaSettings.BisectorStage) ? "All" : mosaSettings.BisectorStage)}");
		OutputStatusBisector($"Baseline: {(state.BaselinePassed ? "PASS" : "FAIL")}");
		OutputStatusBisector($"Iterations: {state.TotalIterationCount} | Passes: {state.PassCount} | Failures: {state.FailureCount}");

		OutputStatusBisector("Failed Transforms:");
		foreach (var result in state.Results.Where(r => !r.Passed).OrderBy(r => r.Transform))
			OutputStatusBisector($"  {result.Transform}");
	}

	private void OutputStatusBisector(string status)
	{
		Console.WriteLine($"{stopwatch.Elapsed.TotalSeconds:00.00} | [Bisector] {status}");
	}

	private void OutputStatus(string status)
	{
		Console.WriteLine($"{stopwatch.Elapsed.TotalSeconds:00.00} | {status}");
	}

	#endregion Output
}

