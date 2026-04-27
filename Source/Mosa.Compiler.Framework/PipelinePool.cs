// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Threading.Channels;

namespace Mosa.Compiler.Framework;

internal sealed class PipelinePool : IAsyncDisposable
{
	private readonly MethodScheduler MethodScheduler;
	private readonly Compiler Compiler;

	private readonly CancellationTokenSource cts = new();

	// Coalesced “work may exist” signal:
	// bounded capacity=1 means multiple enqueue signals collapse into one.
	private readonly Channel<bool> workSignal =
		Channel.CreateBounded<bool>(new BoundedChannelOptions(1)
		{
			FullMode = BoundedChannelFullMode.DropWrite
		});

	// Free pipeline slots (threadSlot == slot index)
	private readonly Channel<int> freeSlots = Channel.CreateUnbounded<int>();

	// One-slot inbox per pipeline slot (enforces exclusivity; no per-item Task.Run)
	private readonly Channel<MethodData>[] inbox;

	private readonly Task[] workers;

	// Dispatcher task
	private readonly Task dispatcher;

	// Completion (optional): lets ExecuteCompile wait until done
	private readonly TaskCompletionSource completed =
		new(TaskCreationOptions.RunContinuationsAsynchronously);

	private int active; // number of slots currently compiling

	public Task Completion => completed.Task;

	/// <summary>
	/// Gets the number of worker threads currently processing methods.
	/// </summary>
	public int ActiveWorkers => Volatile.Read(ref active);

	/// <summary>
	/// Gets the maximum number of worker threads.
	/// </summary>
	public int MaxWorkers { get; }

	public PipelinePool(MethodScheduler scheduler, Compiler compiler, int maxThreads)
	{
		MethodScheduler = scheduler;
		Compiler = compiler;
		MaxWorkers = maxThreads;

		inbox = new Channel<MethodData>[maxThreads];
		workers = new Task[maxThreads];

		for (int i = 0; i < maxThreads; i++)
		{
			inbox[i] = Channel.CreateBounded<MethodData>(new BoundedChannelOptions(1)
			{
				FullMode = BoundedChannelFullMode.Wait,
				SingleReader = true,
				SingleWriter = true
			});

			freeSlots.Writer.TryWrite(i);

			int slot = i;
			workers[i] = Task.Run(() => WorkerLoop(slot));
		}

		// Kick-start: queue may already be pre-populated before subscription starts
		workSignal.Writer.TryWrite(true);

		dispatcher = Task.Run(DispatcherLoop);
	}

	public void NotifyWorkAdded() => workSignal.Writer.TryWrite(true);

	private async Task DispatcherLoop()
	{
		var ct = cts.Token;

		while (!ct.IsCancellationRequested)
		{
			// wait until there might be work
			await workSignal.Reader.ReadAsync(ct).ConfigureAwait(false);

			while (!ct.IsCancellationRequested)
			{
				if (Compiler.IsStopped)
				{
					TryCompleteIfDone();
					break;
				}

				// capacity first: get a free slot
				int slot = await freeSlots.Reader.ReadAsync(ct).ConfigureAwait(false);

				if (Compiler.IsStopped)
				{
					freeSlots.Writer.TryWrite(slot);
					TryCompleteIfDone();
					break;
				}

				// only now pop (preserves your prioritization property)
				var methodData = MethodScheduler.Get();

				if (methodData is null)
				{
					// no work right now: return the slot
					freeSlots.Writer.TryWrite(slot);

					// If nobody is active and queue empty, we’re done
					TryCompleteIfDone();
					break;
				}

				// assign to that slot’s worker
				await inbox[slot].Writer.WriteAsync(methodData, ct).ConfigureAwait(false);
			}
		}
	}

	private async Task WorkerLoop(int slot)
	{
		var ct = cts.Token;
		var reader = inbox[slot].Reader;

		while (!ct.IsCancellationRequested)
		{
			MethodData work;
			try
			{
				work = await reader.ReadAsync(ct).ConfigureAwait(false);
			}
			catch (OperationCanceledException)
			{
				break;
			}

			Interlocked.Increment(ref active);

			try
			{
				Compiler.CompileMethod(work, slot);
			}
			finally
			{
				Interlocked.Decrement(ref active);

				// return slot to pool
				freeSlots.Writer.TryWrite(slot);

				// If queue is empty and no active workers, complete
				TryCompleteIfDone();
			}
		}
	}

	private void TryCompleteIfDone()
	{
		// If stopped, complete once no workers are active.
		if (Compiler.IsStopped)
		{
			if (Volatile.Read(ref active) == 0)
				completed.TrySetResult();

			return;
		}

		// This is the termination condition: queue empty AND no active workers.
		if (Volatile.Read(ref active) != 0)
			return;

		// Check if queue is truly empty (no queued methods)
		if (MethodScheduler.TotalQueuedMethods != 0)
			return;

		// Both conditions met: no active workers and queue is empty
		completed.TrySetResult();
	}

	public async ValueTask DisposeAsync()
	{
		cts.Cancel();

		workSignal.Writer.TryComplete();
		freeSlots.Writer.TryComplete();

		foreach (var ch in inbox)
			ch.Writer.TryComplete();

		try { await dispatcher.ConfigureAwait(false); } catch { }
		try { await Task.WhenAll(workers).ConfigureAwait(false); } catch { }

		cts.Dispose();
	}
}
