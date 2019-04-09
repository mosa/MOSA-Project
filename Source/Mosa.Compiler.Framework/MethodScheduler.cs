// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Schedules compilation of types/methods.
	/// </summary>
	public sealed class MethodScheduler
	{
		#region Data Members

		private int Timestamp;

		private readonly UniqueQueueThreadSafe<MosaMethod> queue = new UniqueQueueThreadSafe<MosaMethod>();
		private readonly HashSet<MosaMethod> methods = new HashSet<MosaMethod>();

		private readonly Dictionary<MosaMethod, int> inlineQueue = new Dictionary<MosaMethod, int>();

		private object _timestamplock = new object();

		public Compiler Compiler;

		#endregion Data Members

		#region Properties

		public int PassCount { get; private set; }

		/// <summary>
		/// Gets the total methods.
		/// </summary>
		/// <value>
		/// The total methods.
		/// </value>
		public int TotalMethods { get { return methods.Count; } }

		/// <summary>
		/// Gets the queued methods.
		/// </summary>
		/// <value>
		/// The queued methods.
		/// </value>
		public int TotalQueuedMethods { get { return queue.Count; } }

		#endregion Properties

		public MethodScheduler(Compiler compiler)
		{
			Compiler = compiler;
			PassCount = 0;
			Timestamp = 0;
		}

		public void ScheduleAll(TypeSystem typeSystem)
		{
			foreach (var type in typeSystem.AllTypes)
			{
				Schedule(type);
			}
		}

		public void Schedule(MosaType type)
		{
			if (type.IsModule)
				return;

			if (type.IsInterface)
				return;

			if (type.HasOpenGenericParams || type.IsPointer)
				return;

			foreach (var method in type.Methods)
			{
				Schedule(method);
			}
		}

		public void Schedule(MosaMethod method)
		{
			if (method.IsAbstract && !method.HasImplementation)
				return;

			if (method.HasOpenGenericParams)
				return;

			if (method.IsCompilerGenerated)
				return;

			queue.Enqueue(method);

			lock (methods)
			{
				if (!methods.Contains(method))
				{
					methods.Add(method);
				}
			}
		}

		public bool IsScheduled(MosaMethod method)
		{
			lock (methods)
			{
				return methods.Contains(method);
			}
		}

		public MosaMethod GetMethodToCompile()
		{
			var method = queue.Dequeue();

			if (method != null)
				return method;

			FlushInlineQueue();

			method = queue.Dequeue();

			if (method != null)
				return method;

			return null;
		}

		public void DescheduledAllMethods()
		{
			while (queue.Count != 0)
			{
				queue.Dequeue();
			}
		}

		public int GetTimestamp()
		{
			lock (_timestamplock)
			{
				return ++Timestamp;
			}
		}

		public void AddToInlineQueueByCallee(MethodData calleeMethod, int timestamp)
		{
			lock (inlineQueue)
			{
				foreach (var method in calleeMethod.CalledBy)
				{
					if (!inlineQueue.TryGetValue(method, out int existingtimestamp))
					{
						inlineQueue.Add(method, timestamp);
					}
					else
					{
						if (existingtimestamp < timestamp)
							return;

						inlineQueue.Remove(method);
						inlineQueue.Add(method, existingtimestamp);
					}
				}
			}
		}

		public void AddToInlineQueue(MosaMethod method, int timestamp)
		{
			Debug.Assert(!method.HasOpenGenericParams);

			lock (inlineQueue)
			{
				if (!inlineQueue.TryGetValue(method, out int existingtimestamp))
				{
					inlineQueue.Add(method, timestamp);
				}
				else
				{
					if (existingtimestamp < timestamp)
						return;

					inlineQueue.Remove(method);
					inlineQueue.Add(method, existingtimestamp);
				}
			}
		}

		public void FlushInlineQueue()
		{
			lock (inlineQueue)
			{
				foreach (var item in inlineQueue)
				{
					var method = item.Key;
					var timestamp = item.Value;

					var methodData = Compiler.CompilerData.GetMethodData(method);

					if (methodData.InlineTimestamp > timestamp)
						continue;   // nothing to do

					queue.Enqueue(method);
				}

				inlineQueue.Clear();
			}
		}
	}
}
