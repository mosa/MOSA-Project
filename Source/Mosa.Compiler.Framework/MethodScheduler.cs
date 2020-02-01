// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Schedules compilation of types/methods.
	/// </summary>
	public sealed class MethodScheduler
	{
		#region Data Members

		public Compiler Compiler;
		private int Timestamp;

		private readonly Queue<MosaMethod> scheduleQueue = new Queue<MosaMethod>();

		private readonly HashSet<MosaMethod> scheduleSet = new HashSet<MosaMethod>();

		private readonly HashSet<MosaMethod> methods = new HashSet<MosaMethod>();

		//private readonly Dictionary<MosaMethod, int> inlineQueue = new Dictionary<MosaMethod, int>();

		private readonly HashSet<MosaMethod> recompileSet = new HashSet<MosaMethod>();

		private readonly object _timestamplock = new object();

		#endregion Data Members

		#region Properties

		public int PassCount { get; }

		/// <summary>
		/// Gets the total methods.
		/// </summary>
		/// <value>
		/// The total methods.
		/// </value>
		public int TotalMethods { get { lock (methods) { return methods.Count; } } }

		/// <summary>
		/// Gets the queued methods.
		/// </summary>
		/// <value>
		/// The queued methods.
		/// </value>
		public int TotalQueuedMethods { get { lock (scheduleQueue) { return scheduleQueue.Count; } } }

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

			AddToQueue(method);

			lock (methods)
			{
				if (!methods.Contains(method))
				{
					methods.Add(method);
				}
			}
		}

		private void AddToQueue(MosaMethod method)
		{
			lock (scheduleQueue)
			{
				if (!scheduleSet.Contains(method))
				{
					scheduleSet.Add(method);
					scheduleQueue.Enqueue(method);
				}
			}
		}

		public MosaMethod GetMethodToCompile()
		{
			var method = GetScheduledMethod();

			if (method != null)
				return method;

			FlushInlineQueue();

			method = GetScheduledMethod();

			return method;
		}

		private MosaMethod GetScheduledMethod()
		{
			lock (scheduleQueue)
			{
				if (scheduleQueue.Count == 0)
					return null;

				var method = scheduleQueue.Dequeue();
				scheduleSet.Remove(method);

				return method;
			}
		}

		public void DescheduledAllMethods()
		{
			lock (scheduleQueue)
			{
				scheduleQueue.Clear();
				scheduleSet.Clear();
			}
		}

		public int GetTimestamp()
		{
			lock (_timestamplock)
			{
				return ++Timestamp;
			}
		}

		public void AddToRecompileQueue(HashSet<MosaMethod> methods)
		{
			lock (recompileSet)
			{
				recompileSet.UnionWith(methods);
			}
		}

		public void AddToRecompileQueue(MosaMethod method)
		{
			lock (recompileSet)
			{
				recompileSet.Add(method);
			}
		}

		public void FlushInlineQueue()
		{
			bool action = false;

			lock (recompileSet)
			{
				foreach (var method in recompileSet)
				{
					lock (scheduleQueue)
					{
						AddToQueue(method);
					}
				}

				recompileSet.Clear();
				action = recompileSet.Count != 0;
			}

			if (action)
			{
				Compiler.PostEvent(CompilerEvent.InlineMethodsScheduled);
			}
		}
	}
}
