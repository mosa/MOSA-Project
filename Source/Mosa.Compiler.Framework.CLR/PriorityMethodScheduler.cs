// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Priority_Queue;

namespace Mosa.Compiler.Framework.CLR;

/// <summary>
/// Schedules compilation of types/methods.
/// </summary>
internal sealed class PriorityMethodScheduler : MethodScheduler
{
	#region Data Members

	private readonly HashSet<MethodData> workingSet = new HashSet<MethodData>();
	private readonly SimplePriorityQueue<MethodData> queue = new SimplePriorityQueue<MethodData>();

	private int totalMethods, totalQueued;

	#endregion

	#region Properties

	public override int TotalMethods => totalMethods;

	public override int TotalQueuedMethods => totalQueued;

	#endregion

	public PriorityMethodScheduler(Compiler compiler)
	{
		Compiler = compiler;
		PassCount = 0;
	}

	public override void AddToQueue(MethodData methodData)
	{
		lock (workingSet)
		{
			if (!workingSet.Contains(methodData))
			{
				workingSet.Add(methodData);

				Interlocked.Increment(ref totalMethods);
			}
		}

		lock (queue)
		{
			if (queue.Contains(methodData))
			{
				//Debug.WriteLine($"Already in Queue: {method}");

				return; // already queued
			}

			//Debug.WriteLine($"Queued: {method}");
			var priority = GetCompilePriorityLevel(methodData);

			queue.Enqueue(methodData, priority);

			Interlocked.Increment(ref totalQueued);
		}
	}

	public override MethodData GetMethodToCompile()
	{
		lock (queue)
		{
			if (queue.TryDequeue(out var methodData))
			{
				Interlocked.Decrement(ref totalQueued);

				//Debug.WriteLine($"Dequeued: {method}");

				return methodData;
			}

			return null;
		}
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
}
