// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Schedules compilation of types/methods.
/// </summary>
public sealed class MethodScheduler
{
	private struct Constant
	{
		public const int QueueReportIntervalSeconds = 1; // Report queue status every second
	}

	#region Data Members

	public Compiler Compiler;

	private readonly HashSet<MethodData> workingSet = new HashSet<MethodData>();

	private readonly PriorityQueue<MethodData, int> queue = new PriorityQueue<MethodData, int>();

	private readonly HashSet<MethodData> queueSet = new HashSet<MethodData>();

	private readonly HashSet<MethodData> currentlyCompiling = new HashSet<MethodData>();

	private readonly HashSet<MethodData> deferredQueue = new HashSet<MethodData>();

	// Reference to pipeline pool for tracking active workers
	private PipelinePool pipelinePool;

	private int totalMethods;
	private int totalQueued;
	private int totalDeferred;

	// Queue profiling metrics
	private readonly Stopwatch queueProfileTimer = Stopwatch.StartNew();

	private int peakQueueSize;

	private long totalDequeueOperations;
	private long totalEnqueueOperations;
	private long lastQueueReportTicks;
	private long lastReportedDequeueCount;
	private long lastReportedEnqueueCount;

	// CPU monitoring
	private readonly Process currentProcess = Process.GetCurrentProcess();

	private TimeSpan lastCpuTime;
	private long lastCpuCheckTicks;
	private readonly int processorCount = Environment.ProcessorCount;

	#endregion Data Members

	#region Properties

	public int PassCount { get; }

	/// <summary>
	/// Gets the total methods.
	/// </summary>
	/// <value>
	/// The total methods.
	/// </value>
	public int TotalMethods => totalMethods;

	/// <summary>
	/// Gets the queued methods.
	/// </summary>
	/// <value>
	/// The queued methods.
	/// </value>
	public int TotalQueuedMethods => totalQueued;

	/// <summary>
	/// Gets the deferred methods waiting for recompilation.
	/// </summary>
	/// <value>
	/// The deferred methods.
	/// </value>
	public int TotalDeferredMethods => totalDeferred;

	/// <summary>
	/// Gets the peak queue size observed.
	/// </summary>
	public int PeakQueueSize => peakQueueSize;

	/// <summary>
	/// Gets the total number of dequeue operations.
	/// </summary>
	public long TotalDequeueOperations => totalDequeueOperations;

	/// <summary>
	/// Gets the total number of enqueue operations.
	/// </summary>
	public long TotalEnqueueOperations => totalEnqueueOperations;

	#endregion Properties

	public MethodScheduler(Compiler compiler)
	{
		Compiler = compiler;
		PassCount = 0;
		lastQueueReportTicks = queueProfileTimer.ElapsedTicks;
		lastReportedDequeueCount = 0;
		lastReportedEnqueueCount = 0;
		lastCpuTime = currentProcess.TotalProcessorTime;
		lastCpuCheckTicks = queueProfileTimer.ElapsedTicks;
	}

	/// <summary>
	/// Sets the pipeline pool reference for tracking active workers.
	/// </summary>
	internal void SetPipelinePool(PipelinePool pool)
	{
		pipelinePool = pool;
	}

	public void ScheduleAll(TypeSystem typeSystem)
	{
		foreach (var type in typeSystem.AllTypes)
		{
			Schedule(type);
		}
	}

	public bool IsCompilable(MosaType type)
	{
		if (type.IsModule)
			return false;

		if (type.IsInterface)
			return false;

		if (type.HasOpenGenericParams || type.IsPointer)
			return false;

		return true;
	}

	public bool IsCompilable(MosaMethod method)
	{
		if (method.IsAbstract && !method.HasImplementation)
			return false;

		if (method.HasOpenGenericParams)
			return false;

		if (method.IsCompilerGenerated)
			return false;

		return true;
	}

	public void Schedule(MosaType type)
	{
		if (!IsCompilable(type))
			return;

		foreach (var method in type.Methods)
		{
			Schedule(method);
		}
	}

	public void Schedule(MosaMethod method)
	{
		if (!IsCompilable(method))
			return;

		Add(method);
	}

	public void Add(MosaMethod method)
	{
		var methodData = Compiler.GetMethodData(method);
		Add(methodData);
	}

	public void Add(MethodData methodData)
	{
		int queueSize;

		var lockTimer = Stopwatch.StartNew();
		lock (queue)
		{
			Compiler.LockMonitor.RecordLockWait(lockTimer, queue, "MethodScheduler.queue");

			AddInsideLock(methodData);
			queueSize = totalQueued;
		}

		UpdateQueueMetrics(queueSize);
		SignalEnqueued();
	}

	public void Add(HashSet<MosaMethod> methods)
	{
		int queueSize;

		var lockTimer = Stopwatch.StartNew();
		lock (queue)
		{
			Compiler.LockMonitor.RecordLockWait(lockTimer, queue, "MethodScheduler.queue");

			foreach (var method in methods)
			{
				var methodData = Compiler.GetMethodData(method);

				AddInsideLock(methodData);
			}

			queueSize = totalQueued;
		}

		UpdateQueueMetrics(queueSize);
		SignalEnqueued();
	}

	private void AddInsideLock(MethodData methodData)
	{
		if (!workingSet.Contains(methodData))
		{
			workingSet.Add(methodData);

			Interlocked.Increment(ref totalMethods);
		}

		// If currently being compiled, defer it
		if (currentlyCompiling.Contains(methodData))
		{
			if (!deferredQueue.Contains(methodData))
			{
				deferredQueue.Add(methodData);
				Interlocked.Increment(ref totalDeferred);
			}
			return; // Don't add to priority queue yet
		}

		if (queueSet.Contains(methodData))
			return; // already queued

		var priority = GetCompilePriorityLevel(methodData);

		queue.Enqueue(methodData, priority);
		queueSet.Add(methodData);

		Interlocked.Increment(ref totalQueued);
		Interlocked.Increment(ref totalEnqueueOperations);
	}

	public MethodData Get()
	{
		MethodData methodData;
		int queueSize;

		var lockTimer = Stopwatch.StartNew();
		lock (queue)
		{
			Compiler.LockMonitor.RecordLockWait(lockTimer, queue, "MethodScheduler.queue");

			if (queue.TryDequeue(out methodData, out var priority))
			{
				queueSet.Remove(methodData);
				currentlyCompiling.Add(methodData);  // Track as being compiled

				Interlocked.Decrement(ref totalQueued);
				Interlocked.Increment(ref totalDequeueOperations);

				queueSize = totalQueued;
			}
			else
			{
				queueSize = 0;
			}
		}

		UpdateQueueMetrics(queueSize);

		return methodData;
	}

	public void MarkCompleted(MethodData methodData)
	{
		bool shouldRequeue = false;
		int queueSize;

		var lockTimer = Stopwatch.StartNew();
		lock (queue)
		{
			Compiler.LockMonitor.RecordLockWait(lockTimer, queue, "MethodScheduler.queue");

			currentlyCompiling.Remove(methodData);

			// Check if it needs recompilation
			if (deferredQueue.Remove(methodData))
			{
				Interlocked.Decrement(ref totalDeferred);
				shouldRequeue = true;
				AddInsideLock(methodData);
			}

			queueSize = totalQueued;
		}

		if (shouldRequeue)
		{
			UpdateQueueMetrics(queueSize);
			SignalEnqueued();
		}
	}

	private void UpdateQueueMetrics(int currentQueueSize)
	{
		// Update peak queue size
		int currentPeak = peakQueueSize;
		while (currentQueueSize > currentPeak)
		{
			var original = Interlocked.CompareExchange(ref peakQueueSize, currentQueueSize, currentPeak);
			if (original == currentPeak)
				break;
			currentPeak = original;
		}

		// Periodic queue status reporting
		var currentTicks = queueProfileTimer.ElapsedTicks;
		var timeThresholdMet = currentTicks - lastQueueReportTicks >= Stopwatch.Frequency * Constant.QueueReportIntervalSeconds;

		if (timeThresholdMet)
		{
			var wasLastReportTicks = Interlocked.Read(ref lastQueueReportTicks);
			// Use CompareExchange to ensure only one thread reports (thread-safe)
			if (Interlocked.CompareExchange(ref lastQueueReportTicks, currentTicks, wasLastReportTicks) == wasLastReportTicks)
			{
				var currentDequeueCount = totalDequeueOperations;
				var previousDequeueCount = Interlocked.Exchange(ref lastReportedDequeueCount, currentDequeueCount);
				var currentEnqueueCount = totalEnqueueOperations;
				var previousEnqueueCount = Interlocked.Exchange(ref lastReportedEnqueueCount, currentEnqueueCount);

				ReportQueueStatus(currentQueueSize, currentTicks, wasLastReportTicks,
					currentDequeueCount, previousDequeueCount, currentEnqueueCount, previousEnqueueCount);
			}
		}
	}

	private void ReportQueueStatus(int currentQueueSize, long currentTicks, long previousTicks,
		long currentDequeueCount, long previousDequeueCount, long currentEnqueueCount, long previousEnqueueCount)
	{
		// Calculate instantaneous rates (since last report)
		var ticksDelta = currentTicks - previousTicks;
		var secondsDelta = ticksDelta / (double)Stopwatch.Frequency;

		var dequeueDelta = currentDequeueCount - previousDequeueCount;
		var enqueueDelta = currentEnqueueCount - previousEnqueueCount;

		var dequeueRate = secondsDelta > 0 ? (uint)(dequeueDelta / secondsDelta) : 0;
		var enqueueRate = secondsDelta > 0 ? (uint)(enqueueDelta / secondsDelta) : 0;

		var activeWorkers = pipelinePool?.ActiveWorkers ?? 0;
		var maxWorkers = pipelinePool?.MaxWorkers ?? 0;
		var utilizationPercent = maxWorkers > 0 ? (activeWorkers * 100.0 / maxWorkers) : 0;
		var idleWorkers = maxWorkers - activeWorkers;

		// Calculate CPU usage with equivalent core count
		var cpuPercent = CalculateCpuUsage(currentTicks);
		var equivalentCores = (cpuPercent * processorCount) / 100.0;

		var deferredCount = totalDeferred;

		Compiler.PostEvent(
			CompilerEvent.Debug,
			$"[Queue] Size: {currentQueueSize} | Deferred: {deferredCount} | " +
			$"Active: {activeWorkers}/{maxWorkers} ({utilizationPercent:F1}%) | Idle: {idleWorkers} | " +
			$"Enqueue: {enqueueRate}/s | Dequeue: {dequeueRate}/s | " +
			$"CPU: {cpuPercent:F1}% ({equivalentCores:F1}/{processorCount} cores)"
		);
	}

	private double CalculateCpuUsage(long currentTicks)
	{
		try
		{
			currentProcess.Refresh();
			var currentCpuTime = currentProcess.TotalProcessorTime;
			var cpuTimeDelta = (currentCpuTime - lastCpuTime).TotalMilliseconds;

			var ticksDelta = currentTicks - lastCpuCheckTicks;
			var wallTimeDelta = (ticksDelta / (double)Stopwatch.Frequency) * 1000.0; // Convert to milliseconds

			lastCpuTime = currentCpuTime;
			lastCpuCheckTicks = currentTicks;

			if (wallTimeDelta > 0 && wallTimeDelta < 60000) // Sanity check: < 60 seconds
			{
				// CPU percentage divided by cores to match Task Manager (0-100% scale)
				var cpuPercent = (cpuTimeDelta / wallTimeDelta / processorCount) * 100.0;
				return Math.Clamp(cpuPercent, 0.0, 100.0);
			}
		}
		catch
		{
			// Ignore any errors in CPU calculation
		}

		return 0.0;
	}

	private static int GetCompilePriorityLevel(MethodData methodData)
	{
		if (methodData.DoNotInline)
			return 200;

		var adjustment = 0;

		if (methodData.HasAggressiveInliningAttribute)
			adjustment += 75;

		if (methodData.Inlined)
			adjustment += 20;

		if (methodData.Method.DeclaringType.IsValueType)
			adjustment += 15;

		if (methodData.Method.IsStatic)
			adjustment += 5;

		if (methodData.HasProtectedRegions)
			adjustment -= 10;

		if (methodData.VirtualCodeSize > 100)
			adjustment -= 75;

		if (methodData.VirtualCodeSize > 50)
			adjustment -= 50;

		if (methodData.VirtualCodeSize < 50)
			adjustment += 5;

		if (methodData.VirtualCodeSize < 30)
			adjustment += 5;

		if (methodData.VirtualCodeSize < 10)
			adjustment += 10;

		if (methodData.AggressiveInlineRequested)
			adjustment += 20;

		if (methodData.Method.IsConstructor)
			adjustment += 10;

		if (methodData.Method.IsTypeConstructor)
			adjustment += 3;

		if (methodData.Version > 3)
			adjustment -= 7;

		if (methodData.Version > 5)
			adjustment -= 15;

		//if (methodData.Method.FullName.StartsWith("System."))
		//	adjustment += 5;

		return 100 - adjustment;
	}

	#region Subscription

	private Action? _onEnqueued;

	public IDisposable Subscribe(Action onEnqueued)
	{
		_onEnqueued += onEnqueued;
		return new Unsubscriber(() => _onEnqueued -= onEnqueued);
	}

	private void SignalEnqueued() => _onEnqueued?.Invoke();

	private sealed class Unsubscriber : IDisposable
	{
		private readonly Action _dispose;

		public Unsubscriber(Action dispose) => _dispose = dispose;

		public void Dispose() => _dispose();
	}

	#endregion Subscription

	public void ResetRates()
	{
		Interlocked.Exchange(ref totalEnqueueOperations, 0);
		Interlocked.Exchange(ref totalDequeueOperations, 0);
		Interlocked.Exchange(ref lastReportedDequeueCount, 0);
		Interlocked.Exchange(ref lastReportedEnqueueCount, 0);
	}
}
