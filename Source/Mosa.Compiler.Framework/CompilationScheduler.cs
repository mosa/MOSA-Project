/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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

		private readonly Queue<MosaMethod> methodQueue = new Queue<MosaMethod>();
		private readonly HashSet<MosaMethod> methodScheduled = new HashSet<MosaMethod>(new MosaMethodFullNameComparer());
		//private readonly HashSet<MosaMethod> methodCompiled = new HashSet<MosaMethod>();

		private readonly TypeSystem typeSystem;

		private object mylock = new object();

		#endregion Data Members

		public CompilationScheduler(TypeSystem typeSystem)
		{
			this.typeSystem = typeSystem;
		}

		public void ScheduleAll()
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

			if (type.IsInterface || type.IsPointer)
				return;

			foreach (var method in type.Methods)
			{
				Schedule(method);
			}
		}

		public void Schedule(MosaMethod method)
		{
			if (method.IsAbstract)
				return;

			if (methodScheduled.Contains(method))
				return;

			if (method.IsLinkerGenerated)
				return;

			lock (mylock)
			{
				methodScheduled.Add(method);
				methodQueue.Enqueue(method);
			}
		}

		public bool IsScheduled(MosaMethod method)
		{
			lock (mylock)
			{
				return methodScheduled.Contains(method);
			}
		}

		public void TrackTypeAllocated(MosaType type)
		{
			// TODO
		}

		public void TrackMethodInvoked(MosaMethod method)
		{
			// TODO
		}

		public void TrackFieldReferenced(MosaField field)
		{
			// TODO
		}

		public MosaMethod GetMethodToCompile()
		{
			lock (mylock)
			{
				if (methodQueue.Count == 0)
					return null;

				return methodQueue.Dequeue();
			}
		}

		/// <summary>
		/// Gets the total methods.
		/// </summary>
		/// <value>
		/// The total methods.
		/// </value>
		public int TotalMethods { get { return methodScheduled.Count; } }

		/// <summary>
		/// Gets the queued methods.
		/// </summary>
		/// <value>
		/// The queued methods.
		/// </value>
		public int TotalQueuedMethods { get { return methodQueue.Count; } }
	}
}