/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 */

using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{

	public interface ICompilationScheduler2
	{
		/// <summary>
		/// Tracks the type allocated.
		/// </summary>
		/// <param name="type">The type.</param>
		void TrackTypeAllocated(RuntimeType type);

		/// <summary>
		/// Tracks the method invoked.
		/// </summary>
		/// <param name="method">The method.</param>
		void TrackMethodInvoked(RuntimeMethod method);

		/// <summary>
		/// Tracks the field referenced.
		/// </summary>
		/// <param name="field">The field.</param>
		void TrackFieldReferenced(RuntimeField field);

		/// <summary>
		/// Gets the method to compile.
		/// </summary>
		/// <returns></returns>
		RuntimeMethod GetMethodToCompile();
	}
}
