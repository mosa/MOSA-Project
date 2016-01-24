// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Schedules compilation of types/methods.
	/// </summary>
	public sealed class CompilationScheduler
	{
		#region Data Members

		private readonly UniqueQueueThreadSafe<MosaMethod> queue = new UniqueQueueThreadSafe<MosaMethod>();
		private readonly HashSet<MosaMethod> methods = new HashSet<MosaMethod>();

		private readonly UniqueQueueThreadSafe<CompilerMethodData> inlineQueue = new UniqueQueueThreadSafe<CompilerMethodData>();

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

		public CompilationScheduler()
		{
			PassCount = 0;
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
			if (method.IsAbstract || method.HasOpenGenericParams)
				return;

			if (method.IsLinkerGenerated)
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

		public void TrackTypeAllocated(MosaType type)
		{
		}

		public void TrackMethodInvoked(MosaMethod method)
		{
		}

		public void TrackFieldReferenced(MosaField field)
		{
			// TODO
		}

		public MosaMethod GetMethodToCompile()
		{
			return queue.Dequeue();
		}

		public void AddToInlineQueue(CompilerMethodData methodData)
		{
			Debug.Assert(!methodData.Method.IsAbstract && !methodData.Method.HasOpenGenericParams);

			//Debug.Assert(!methodData.Method.IsLinkerGenerated);

			inlineQueue.Enqueue(methodData);
		}

		public bool StartNextPass()
		{
			int i = 0;

			lock (inlineQueue)
			{
				while (inlineQueue.Count != 0)
				{
					var methodData = inlineQueue.Dequeue();

					foreach (var callee in methodData.CalledBy)
					{
						Schedule(callee);
						i++;
					}
				}
			}

			PassCount++;

			return i != 0;
		}
	}
}
