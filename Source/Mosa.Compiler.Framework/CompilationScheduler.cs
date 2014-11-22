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
	public sealed class CompilationScheduler : ICompilationScheduler
	{
		#region Data Members

		private readonly HashSet<MosaType> typeScheduled = new HashSet<MosaType>();

		private readonly Queue<MosaMethod> methodQueue = new Queue<MosaMethod>();
		private readonly HashSet<MosaMethod> methodScheduled = new HashSet<MosaMethod>();

		private readonly TypeSystem typeSystem;

		private readonly bool compileAllMethods;

		#endregion Data Members

		public CompilationScheduler(TypeSystem typeSystem, bool compileAllMethods)
		{
			this.typeSystem = typeSystem;
			this.compileAllMethods = compileAllMethods;

			if (compileAllMethods)
			{
				// forces all types to get compiled
				foreach (MosaType type in typeSystem.AllTypes)
				{
					CompileType(type);
				}
			}
		}

		#region ICompilationScheduler members

		void ICompilationScheduler.TrackTypeAllocated(MosaType type)
		{
			if (type.IsModule)
				return;

			if (compileAllMethods)
			{
				CompileType(type);
			}
		}

		void ICompilationScheduler.TrackMethodInvoked(MosaMethod method)
		{
			CompileMethod(method);

			(this as ICompilationScheduler).TrackTypeAllocated(method.DeclaringType);
		}

		void ICompilationScheduler.TrackFieldReferenced(MosaField field)
		{
			(this as ICompilationScheduler).TrackTypeAllocated(field.DeclaringType);
		}

		#endregion ICompilationScheduler members

		private void CompileType(MosaType type)
		{
			if (type.IsModule)
				return;

			if (type.IsInterface)
				return;

			if (type.IsInterface || type.IsPointer)
				return;

			if (typeScheduled.Contains(type))
				return;

			typeScheduled.Add(type);

			foreach (var method in type.Methods)
			{
				CompileMethod(method);
			}
		}

		private void CompileMethod(MosaMethod method)
		{
			if (method.IsAbstract)
				return;

			if (methodScheduled.Contains(method))
				return;

			methodScheduled.Add(method);
			methodQueue.Enqueue(method);
		}

		MosaMethod ICompilationScheduler.GetMethodToCompile()
		{
			if (methodQueue.Count == 0)
				return null;

			return methodQueue.Dequeue();
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