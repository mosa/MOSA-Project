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
	private HashSet<string> bisectorDisabledTransformNames = [];
	private HashSet<string> effectiveDisabledTransformNames = [];
	private HashSet<string> forcedDisabledTransformNames = [];

	private Bisector<string> bisector;

	public int Start(string[] args)
	{
		try
		{
			MosaSettings.LoadArguments(args);

			MosaSettings.UnitTestFailFast = true;

			Stopwatch.Start();

			LoadForcedDisabledTransforms();

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

			if (observedTransformNames.Count == 0)
			{
				OutputStatus("ERROR: No observed transforms were captured for the selected stage.");
				return 1;
			}

			OutputStatus($"Observed Transforms: {observedTransformNames.Count}");
			OutputStatus($"Discovery Iteration: {(discoveryResult.Passed ? "PASS" : "FAIL")}");
			ReportForcedDisabledNotObserved();

			if (!discoveryResult.Passed)
			{
				OutputStatus("Running failure-inducing bisector...");
				RunBisectorSession("Failure-Inducing", invertOutcome: false, discoveryResult);

				if (!MosaSettings.UnitTestBisectorMasking)
					return 0;

				OutputStatus("Running masking bisector (items whose removal induces failure)...");
				RunBisectorSession("Masking", invertOutcome: true, discoveryResult);
				return 0;
			}

			if (!MosaSettings.UnitTestBisectorMasking)
			{
				OutputStatus("Discovery passed and masking mode disabled. Nothing to bisect.");
				return 0;
			}

			OutputStatus("Running masking bisector (items whose removal induces failure)...");
			RunBisectorSession("Masking", invertOutcome: true, discoveryResult);
			return 0;
		}
		catch (Exception ex)
		{
			OutputStatus($"Exception: {ex.Message}");
			OutputStatus($"Exception: {ex.StackTrace}");
			return 1;
		}
	}

	private void RunBisectorSession(string sessionName, bool invertOutcome, IterationResult discoveryResult)
	{
		bisectorDisabledTransformNames = [];
		RebuildEffectiveDisabledSet();
		bisector = new Bisector<string>(observedTransformNames.Where(name => !forcedDisabledTransformNames.Contains(name)));

		// Consume the baseline using discovery outcome to avoid rerunning identical baseline iteration.
		bisectorDisabledTransformNames = [.. bisector.GetNextDisabledItems()];
		RebuildEffectiveDisabledSet();
		var mappedBaseline = MapOutcome(discoveryResult.Passed, invertOutcome);
		bisector.AcceptResult(mappedBaseline);
		OutputStatus($"{sessionName} Baseline -> Actual: {(discoveryResult.Passed ? "PASS" : "FAIL")}, Mapped: {(mappedBaseline ? "PASS" : "FAIL")}");

		while (!bisector.IsComplete)
		{
			bisectorDisabledTransformNames = [.. bisector.GetNextDisabledItems()];
			RebuildEffectiveDisabledSet();
			PrintIterationHeader(sessionName, bisector.GetStatus());
			PrintDisabledTransforms();

			var iterationResult = ExecuteIteration();
			var mappedResult = MapOutcome(iterationResult.Passed, invertOutcome);

			bisector.AcceptResult(mappedResult);
			OutputStatus($"Iteration Result -> Actual: {(iterationResult.Passed ? "PASS" : "FAIL")}, Mapped: {(mappedResult ? "PASS" : "FAIL")}");
			PrintStatus(bisector.GetStatus());
		}

		OutputStatus($"{sessionName} bisector complete.");
		PrintFinalReport(sessionName, bisector);
	}

	private static bool MapOutcome(bool passed, bool invertOutcome)
	{
		return invertOutcome ? !passed : passed;
	}

	private IterationResult ExecuteIteration()
	{
		using var assertCapture = new AssertCaptureScope();

		try
		{
			using var unitTestEngine = new UnitTestEngine(MosaSettings, OutputStatus, CreateCompilerHooks);
			if (unitTestEngine.IsAborted)
			{
				OutputStatus("Iteration compiler run aborted. Treating as FAIL.");
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

			return new IterationResult(passed);
		}
		catch (AssertFailureException ex)
		{
			OutputStatus($"Debug.Assert captured and treated as FAIL: {ex.Message}");
			return new IterationResult(false);
		}
		catch (Exception ex)
		{
			OutputStatus($"Iteration exception treated as FAIL: {ex.Message}");
			OutputStatus($"Iteration exception stack: {ex.StackTrace}");
			return new IterationResult(false);
		}
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
			if (observedTransformNames.Add(transformName) && bisector != null && !forcedDisabledTransformNames.Contains(transformName))
			{
				bisector.ObserveItem(transformName);
			}
		}
	}

	private bool IsTransformDisabled(string stageName, string transformName)
	{
		if (!string.Equals(stageName, selectedStageName, StringComparison.Ordinal))
			return false;

		return effectiveDisabledTransformNames.Contains(transformName);
	}

	private void LoadForcedDisabledTransforms()
	{
		forcedDisabledTransformNames = [];

		var filename = MosaSettings.UnitTestBisectorDisabledTransformsFile;
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

		OutputStatus($"Forced Disabled Transforms File: {filename}");
		OutputStatus($"Forced Disabled Transforms Loaded: {forcedDisabledTransformNames.Count}");
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

		OutputStatus($"WARNING: {notObserved.Count} forced-disabled transforms were not observed in selected stage:");
		foreach (var name in notObserved)
		{
			OutputStatus($"  {name}");
		}
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

	private void PrintIterationHeader(string sessionName, Bisector<string>.BisectorStatus status)
	{
		OutputStatus($"{sessionName} Iteration: {status.Iteration + 1}");
		OutputStatus($"Level: {status.Level}");
		OutputStatus($"Phase: {status.Phase}");
		OutputStatus($"Stage: {selectedStageType.FullName} ({selectedStageName})");
	}

	private void PrintDisabledTransforms()
	{
		OutputStatus($"Forced Disabled: {forcedDisabledTransformNames.Count}");
		OutputStatus($"Bisector Disabled: {bisectorDisabledTransformNames.Count}");
		OutputStatus($"Effective Disabled: {effectiveDisabledTransformNames.Count}");

		//OutputStatus($"Transforms Disabled:");
		//foreach (var transform in bisectorDisabledTransformNames.OrderBy(t => t))
		//{
		//	OutputStatus($"  {transform}");
		//}
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

	private void PrintFinalReport(string sessionName, Bisector<string> sessionBisector)
	{
		OutputStatus($"{sessionName} Final Stage: {selectedStageType.FullName} ({selectedStageName})");
		OutputStatus("Forced Disabled Items:");
		foreach (var transform in forcedDisabledTransformNames.OrderBy(t => t))
		{
			OutputStatus($"  {transform}");
		}

		OutputStatus("Confirmed Bad Items:");
		foreach (var transform in sessionBisector.ConfirmedBadItems.OrderBy(t => t))
		{
			OutputStatus($"  {transform}");
		}

		OutputStatus("Confirmed Bad Pairs:");
		foreach (var pair in sessionBisector.ConfirmedBadPairs.OrderBy(p => p.Item1).ThenBy(p => p.Item2))
		{
			OutputStatus($"  {pair.Item1} + {pair.Item2}");
		}

		OutputStatus("Remaining Suspects:");
		foreach (var transform in sessionBisector.RemainingSuspectItems.OrderBy(t => t))
		{
			OutputStatus($"  {transform}");
		}
	}

	private void OutputStatus(string status)
	{
		Console.WriteLine($"{Stopwatch.Elapsed.TotalSeconds:00.00} | {status}");
	}

	private readonly record struct IterationResult(bool Passed);

	private sealed class AssertCaptureScope : IDisposable
	{
		private readonly List<(DefaultTraceListener Listener, bool AssertUiEnabled)> defaultListeners = new();
		private readonly TraceListener listener = new AssertExceptionTraceListener();

		public AssertCaptureScope()
		{
			foreach (TraceListener traceListener in Trace.Listeners)
			{
				if (traceListener is DefaultTraceListener defaultTraceListener)
				{
					defaultListeners.Add((defaultTraceListener, defaultTraceListener.AssertUiEnabled));
					defaultTraceListener.AssertUiEnabled = false;
				}
			}

			Trace.Listeners.Add(listener);
		}

		public void Dispose()
		{
			Trace.Listeners.Remove(listener);

			foreach (var (listener, assertUiEnabled) in defaultListeners)
			{
				listener.AssertUiEnabled = assertUiEnabled;
			}
		}
	}

	private sealed class AssertExceptionTraceListener : TraceListener
	{
		public override void Write(string message)
		{
		}

		public override void WriteLine(string message)
		{
		}

		public override void Fail(string message, string detailMessage)
		{
			throw new AssertFailureException(message, detailMessage);
		}
	}

	private sealed class AssertFailureException : Exception
	{
		public AssertFailureException(string message, string detailMessage)
			: base(string.IsNullOrWhiteSpace(detailMessage) ? message : $"{message} {detailMessage}")
		{
		}
	}
}
