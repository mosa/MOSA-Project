/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Schedules compilation of types/methods.
	/// </summary>
	public sealed class CompilationScheduler : ICompilationScheduler
	{
		#region Data Members

		private readonly List<MosaType> typesAllocated = new List<MosaType>();
		private readonly List<MosaMethod> methodsInvoked = new List<MosaMethod>();

		private readonly HashSet<MosaType> typeScheduled = new HashSet<MosaType>();
		private readonly HashSet<MosaMethod> methodScheduled = new HashSet<MosaMethod>();
		private readonly Queue<MosaMethod> methodQueue = new Queue<MosaMethod>();

		private readonly Mosa.Compiler.MosaTypeSystem.TypeSystem typeSystem;

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
			//Debug.Assert(!type.HasOpenGenericParams);

			if (type.IsModule)
				return;

			typesAllocated.AddIfNew(type);

			if (compileAllMethods)
			{
				CompileType(type);
			}
		}

		void ICompilationScheduler.TrackMethodInvoked(MosaMethod method)
		{
			//Debug.Assert(!method.HasOpenGenericParams);

			methodsInvoked.AddIfNew(method);

			(this as ICompilationScheduler).TrackTypeAllocated(method.DeclaringType);
		}

		void ICompilationScheduler.TrackFieldReferenced(MosaField field)
		{
			//Debug.Assert(!field.FieldType.HasOpenGenericParams);

			(this as ICompilationScheduler).TrackTypeAllocated(field.DeclaringType);
		}

		/// <summary>
		/// Determines whether the method scheduled to be compiled.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns>
		///   <c>true</c> if method is scheduled to be compiled; otherwise, <c>false</c>.
		/// </returns>
		bool ICompilationScheduler.IsMethodScheduled(MosaMethod method)
		{
			return methodScheduled.Contains(method);
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
	}
}