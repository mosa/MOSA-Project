// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Utility.Configuration;

namespace Mosa.Utility.UnitTests;

public class UnitTestSystem
{
	#region Data

	private readonly Stopwatch Stopwatch = new();
	private readonly MosaSettings MosaSettings = new();

	#endregion Data

	public int Start(string[] args)
	{
		try
		{
			MosaSettings.LoadArguments(args);
			Stopwatch.Start();

			OutputStatus("Discovering Unit Tests...");

			var discoveredUnitTests = UnitTestRunner.Discover(MosaSettings.UnitTestFilter);

			OutputStatus($"Found Tests: {discoveredUnitTests.Count} in {Stopwatch.ElapsedMilliseconds / 1000.0:F2} secs");
			OutputStatus("Starting Unit Test Engine...");

			var unitTestEngine = new UnitTestEngine(MosaSettings, OutputStatus);

			if (unitTestEngine.IsAborted)
			{
				OutputStatus("ERROR: Compilation aborted!");
				return 1;
			}

			var executeStart = Stopwatch.ElapsedMilliseconds;

			var unitTests = UnitTestRunner.Run(unitTestEngine, discoveredUnitTests);

			var elapse = Stopwatch.ElapsedMilliseconds;

			OutputStatus($"Unit Testing: {(elapse - executeStart) / 1000.0:F2} secs");
			OutputStatus($"Total: {elapse / 1000.0} secs");

			unitTestEngine.Terminate();

			var failures = 0;
			var passed = 0;
			var skipped = 0;
			var incomplete = 0;

			foreach (var unitTest in unitTests)
			{
				switch (unitTest.Status)
				{
					case UnitTestStatus.Passed: passed++; break;
					case UnitTestStatus.Skipped: skipped++; break;
					case UnitTestStatus.Pending: incomplete++; break;
					case UnitTestStatus.FailedByCrash:
					case UnitTestStatus.Failed:
						failures++;
						OutputStatus(UnitTestSerializer.FormatUnitTestResult(unitTest));
						break;
				}
			}

			OutputStatus("Unit Test Results:");
			OutputStatus($"  Passed:     {passed}");
			OutputStatus($"  Skipped:    {skipped}");
			OutputStatus($"  Incomplete: {incomplete}");
			OutputStatus($"  Failures:   {failures}");
			OutputStatus($"  Total:      {passed + skipped + failures + incomplete}");

			if (unitTestEngine.IsAborted)
			{
				OutputStatus("ERROR: Unit tests aborted due to failures!");
				return 1;
			}

			if (failures + incomplete == 0)
			{
				OutputStatus("All unit tests passed successfully!");
				return 0;
			}
			else
			{
				OutputStatus("ERROR: Failures occurred in the unit tests!");
				return failures + incomplete;
			}
		}
		catch (Exception ex)
		{
			OutputStatus($"Exception: {ex.Message}");
			OutputStatus($"Exception: {ex.StackTrace}");
			return 1;
		}
	}

	private void OutputStatus(string status)
	{
		Console.WriteLine($"{Stopwatch.Elapsed.TotalSeconds:00.00} | {status}");
	}
}

