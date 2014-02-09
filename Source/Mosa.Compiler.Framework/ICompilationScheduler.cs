/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 */

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework
{
	public interface ICompilationScheduler
	{
		/// <summary>
		/// Tracks the type allocated.
		/// </summary>
		/// <param name="type">The type.</param>
		void TrackTypeAllocated(MosaType type);

		/// <summary>
		/// Tracks the method invoked.
		/// </summary>
		/// <param name="method">The method.</param>
		void TrackMethodInvoked(MosaMethod method);

		/// <summary>
		/// Tracks the field referenced.
		/// </summary>
		/// <param name="field">The field.</param>
		void TrackFieldReferenced(MosaField field);

		/// <summary>
		/// Gets the method to compile.
		/// </summary>
		/// <returns></returns>
		MosaMethod GetMethodToCompile();

		/// <summary>
		/// Determines whether the method scheduled to be compiled.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns>
		///   <c>true</c> if method is scheduled to be compiled; otherwise, <c>false</c>.
		/// </returns>
		bool IsMethodScheduled(MosaMethod method);
	}
}