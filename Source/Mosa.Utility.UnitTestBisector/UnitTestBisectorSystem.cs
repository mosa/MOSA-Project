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

			OutputStatusBisector("Resolving stage type...");
			selectedStageType = ResolveStageType(MosaSettings.UnitTestBisectorStage);
			selectedStageName = selectedStageType.Name;
			OutputStatusBisector($"Stage: {selectedStageType.FullName} ({selectedStageName})");

			OutputStatus("Discovering Unit Tests...");
			discoveredUnitTests = Discovery.DiscoverUnitTests(MosaSettings.UnitTestFilter);
			OutputStatus($"Found Tests: {discoveredUnitTests.Count} in {Stopwatch.ElapsedMilliseconds / 1000.0:F2} secs");

			if (discoveredUnitTests.Count == 0)
			{
				OutputStatus("ERROR: No tests matched the filter.");
				return 1;
			}

			OutputStatusBisector("Starting discovery iteration...");
			var discoveryResult = ExecuteIteration();

			if (observedTransformNames.Count == 0)
			{
				OutputStatusBisector("ERROR: No observed transforms were captured for the selected stage.");
				return 1;
			}

			OutputStatusBisector($"Observed Transforms: {observedTransformNames.Count}");
			OutputStatusBisector($"Discovery Iteration: {(discoveryResult.Passed ? "PASS" : "FAIL")}");
			ReportForcedDisabledNotObserved();

			if (!discoveryResult.Passed)
			{
				OutputStatusBisector("Running failure-inducing bisector...");
				RunBisectorSession("Failure-Inducing", invertOutcome: false, discoveryResult);

				if (!MosaSettings.UnitTestBisectorMasking)
					return 0;

				OutputStatusBisector("Running masking bisector (items whose removal induces failure)...");
				RunBisectorSession("Masking", invertOutcome: true, discoveryResult);
				return 0;
			}

			if (!MosaSettings.UnitTestBisectorMasking)
			{
				OutputStatusBisector("Discovery passed and masking mode disabled. Nothing to bisect.");
				return 0;
			}

			OutputStatusBisector("Running masking bisector (items whose removal induces failure)...");
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
		bisector = new Bisector<string>(observedTransformNames.Where(name => !forcedDisabledTransformNames.Contains(name)), enablePairwise: MosaSettings.UnitTestBisectorPairwise);
		var reportedBadItems = new HashSet<string>(StringComparer.Ordinal);

		// Consume the baseline using discovery outcome to avoid rerunning identical baseline iteration.
		bisectorDisabledTransformNames = [.. bisector.GetNextDisabledItems()];
		RebuildEffectiveDisabledSet();
		var mappedBaseline = MapOutcome(discoveryResult.Passed, invertOutcome);
		bisector.AcceptResult(mappedBaseline);
		OutputStatusBisector($"{sessionName} Baseline -> Actual: {(discoveryResult.Passed ? "PASS" : "FAIL")}, Mapped: {(mappedBaseline ? "PASS" : "FAIL")}");
		PrintNewlyConfirmedBadItems(sessionName, bisector, reportedBadItems);

		while (!bisector.IsComplete)
		{
			bisectorDisabledTransformNames = [.. bisector.GetNextDisabledItems()];
			RebuildEffectiveDisabledSet();
			PrintIterationHeader(sessionName, bisector.GetStatus());
			PrintDisabledTransforms();

			var iterationResult = ExecuteIteration();
			var mappedResult = MapOutcome(iterationResult.Passed, invertOutcome);

			bisector.AcceptResult(mappedResult);
			OutputStatusBisector($"Iteration Result -> Actual: {(iterationResult.Passed ? "PASS" : "FAIL")}, Mapped: {(mappedResult ? "PASS" : "FAIL")}");
			PrintNewlyConfirmedBadItems(sessionName, bisector, reportedBadItems);
			PrintStatus(bisector.GetStatus());
		}

		OutputStatusBisector($"{sessionName} bisector complete.");
		PrintFinalReport(sessionName, bisector);

		// Clean up session resources
		GC.Collect();
		GC.WaitForPendingFinalizers();
	}

	private void PrintNewlyConfirmedBadItems(string sessionName, Bisector<string> sessionBisector, HashSet<string> reportedBadItems)
	{
		foreach (var transform in sessionBisector.ConfirmedBadItems.OrderBy(t => t))
		{
			if (reportedBadItems.Add(transform))
			{
				OutputStatusBisector($"{sessionName} Known Bad Item: {transform}");
			}
		}
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
				OutputStatusBisector("Iteration compiler run aborted. Treating as FAIL.");
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
			OutputStatusBisector($"Debug.Assert captured and treated as FAIL: {ex.Message}");
			return new IterationResult(false);
		}
		catch (Exception ex)
		{
			OutputStatusBisector($"Iteration exception treated as FAIL: {ex.Message}");
			OutputStatusBisector($"Iteration exception stack: {ex.StackTrace}");
			return new IterationResult(false);
		}
		finally
		{
			// Force garbage collection on failure path too
			GC.Collect();
			GC.WaitForPendingFinalizers();
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

			unitTests.Add(unitTest);
		}

		return unitTests;
	}

	private void PrintIterationHeader(string sessionName, Bisector<string>.BisectorStatus status)
	{
		OutputStatusBisector($"{sessionName} Iteration: {status.Iteration + 1}");
		OutputStatusBisector($"Level: {status.Level}");
		OutputStatusBisector($"Phase: {status.Phase}");
		OutputStatusBisector($"Stage: {selectedStageType.FullName} ({selectedStageName})");
	}

	private void PrintDisabledTransforms()
	{
		OutputStatusBisector($"Forced Disabled: {forcedDisabledTransformNames.Count}");
		OutputStatusBisector($"Bisector Disabled: {bisectorDisabledTransformNames.Count}");
		OutputStatusBisector($"Effective Disabled: {effectiveDisabledTransformNames.Count}");

		//OutputStatusBisector($"Transforms Disabled:");
		//foreach (var transform in bisectorDisabledTransformNames.OrderBy(t => t))
		//{
		//	OutputStatusBisector($"  {transform}");
		//}
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
		OutputStatusBisector($"{sessionName} Final Stage: {selectedStageType.FullName} ({selectedStageName})");
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

	private void OutputStatusBisector(string status)
	{
		Console.WriteLine($"{Stopwatch.Elapsed.TotalSeconds:00.00} | [Bisector] {status}");
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
