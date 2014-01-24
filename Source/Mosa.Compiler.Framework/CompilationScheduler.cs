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
			if (type.IsModule)
				return;

			typesAllocated.AddIfNew(type);

			if (compileAllMethods)
				CompileType(type);
		}

		void ICompilationScheduler.TrackMethodInvoked(MosaMethod method)
		{
			methodsInvoked.AddIfNew(method);

			if (compileAllMethods)
				CompileMethod(method);
		}

		void ICompilationScheduler.TrackFieldReferenced(MosaField field)
		{
			if (compileAllMethods)
				CompileType(field.DeclaringType);
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

			if (type.IsBaseGeneric || type.IsOpenGenericType)
				return;

			if (!(type.IsObject || type.IsValueType || type.IsEnum || type.IsString || type.IsInterface || type.IsLinkerGenerated))
				return;

			if (typeScheduled.Contains(type))
				return;

			typeScheduled.Add(type);

			foreach (var method in type.Methods)
				CompileMethod(method);
		}

		private void CompileMethod(MosaMethod method)
		{
			if (method.IsBaseGeneric || method.DeclaringType.IsInterface || method.IsAbstract)
				return;

			if (method.IsOpenGenericType || method.DeclaringType.IsOpenGenericType)
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