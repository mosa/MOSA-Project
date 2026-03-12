// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Diagnostics;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Lock Monitor
/// </summary>
public sealed class LockMonitor
{
	private struct Constant
	{
		public const long LockWaitWarningThresholdMs = 50;
	}

	private struct LockStats
	{
		public long Count;
		public long TotalWaitMs;
		public long PeakWaitMs;
		public object LockObject;
		public string Name;
		public string Type;
	}

	private readonly Dictionary<object, LockStats> lockMonitorStats = new();
	private readonly Compiler compiler;

	private readonly object _lock = new();

	public LockMonitor(Compiler compiler)
	{
		this.compiler = compiler;
	}

	public void RecordLockWait(Stopwatch lockTimer, object lockObject, string lockName = null, string type = null, string location = null)
	{
		var waitMs = lockTimer.ElapsedMilliseconds;

		LockStats currentStats;

		lock (_lock)
		{
			if (!lockMonitorStats.TryGetValue(lockObject, out var lockStat))
			{
				lockStat = new LockStats
				{
					LockObject = lockObject,
					Name = lockName == null ? lockObject.ToString() : lockName,
					Type = string.Empty // "[" + (type ?? lockObject.GetType().Name) + "]";
				};

				lockMonitorStats.Add(lockObject, lockStat);
			}

			lockStat.Count++;
			lockStat.TotalWaitMs += waitMs;
			if (waitMs > lockStat.PeakWaitMs)
				lockStat.PeakWaitMs = waitMs;

			currentStats = lockStat;
		}

		if (waitMs < Constant.LockWaitWarningThresholdMs)
			return;

		ReportLockContention(lockName, currentStats, waitMs, location);
	}

	public void GetLockContentionSummary(long waitThresholdMs)
	{
		var snapshot = lockMonitorStats
			.Where(x => x.Value.TotalWaitMs > waitThresholdMs)
			.OrderByDescending(x => x.Value.TotalWaitMs)
			.ToList();

		if (snapshot.Count == 0)
			return;

		compiler.PostEvent(CompilerEvent.Diagnostic, "Lock Contention Summary:");

		foreach (var kvp in snapshot)
		{
			var stats = kvp.Value;
			var lockName = kvp.Key;

			var avgWaitMs = stats.Count > 0 ? stats.TotalWaitMs / (double)stats.Count : 0;

			compiler.PostEvent(
				CompilerEvent.Diagnostic,
				$"[Lock Contention] Count: {stats.Count} | Peak: {stats.PeakWaitMs}ms | Avg: {avgWaitMs:F1}ms | Wait: {stats.TotalWaitMs}ms -> {lockName} {stats.Type}");
		}
	}

	private void ReportLockContention(string lockName, LockStats stats, long waitMs, string location = null)
	{
		var avgWaitMs = stats.Count > 0 ? stats.TotalWaitMs / (double)stats.Count : 0;
		location = location == null ? string.Empty : $"[{location}] ";

		compiler.PostEvent(
			CompilerEvent.Diagnostic,
			$"[Lock Contention] Count: {stats.Count} | Current: {waitMs}ms | Peak: {stats.PeakWaitMs}ms | Avg: {avgWaitMs:F1}ms | Wait: {stats.TotalWaitMs}ms -> {location}{lockName} {stats.Type} ");
	}
}
