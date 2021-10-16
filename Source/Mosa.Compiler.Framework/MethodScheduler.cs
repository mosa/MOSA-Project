// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using Priority_Queue;
using System.Collections.Generic;
using System.Threading;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Schedules compilation of types/methods.
	/// </summary>
	public sealed class MethodScheduler
	{
		#region Data Members

		public Compiler Compiler;

		private HashSet<MethodData> workingSet = new HashSet<MethodData>();

		private SimplePriorityQueue<MethodData> queue = new SimplePriorityQueue<MethodData>();

		private int totalMethods;
		private int totalQueued;

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

		#endregion Properties

		public MethodScheduler(Compiler compiler)
		{
			Compiler = compiler;
			PassCount = 0;
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

			AddToQueue(method);
		}

		private void AddToQueue(MosaMethod method)
		{
			var methodData = Compiler.GetMethodData(method);
			AddToQueue(methodData);
		}

		private void AddToQueue(MethodData methodData)
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

		public MethodData GetMethodToCompile()
		{
			lock (queue)
			{
				if (queue.TryDequeue(out var methodData))
				{
					Interlocked.Decrement(ref totalQueued);

					//Debug.WriteLine($"Dequeued: {method}");

					return methodData;
				}
				else
				{
					return null;
				}
			}
		}

		public void AddToRecompileQueue(HashSet<MosaMethod> methods)
		{
			foreach (var method in methods)
			{
				AddToQueue(method);
			}
		}

		public void AddToRecompileQueue(HashSet<MethodData> methodDatas)
		{
			foreach (var methodData in methodDatas)
			{
				AddToQueue(methodData);
			}
		}

		public void AddToRecompileQueue(MosaMethod method)
		{
			AddToQueue(method);
		}

		public void AddToRecompileQueue(MethodData methodData)
		{
			AddToQueue(methodData);
		}

		private static int GetCompilePriorityLevel(MethodData methodData)
		{
			if (methodData.DoNotInline)
				return 200;

			int adjustment = 0;

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
}
