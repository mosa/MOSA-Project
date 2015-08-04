// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

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

		#endregion Data Members

		public CompilationScheduler()
		{
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
	}
}
