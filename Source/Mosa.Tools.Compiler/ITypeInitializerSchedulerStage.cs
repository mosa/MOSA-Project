/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Linker;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Tools.Compiler
{
	public interface ITypeInitializerSchedulerStage
	{
		/// <summary>
		/// Gets the initializer method.
		/// </summary>
		/// <value>The method.</value>
		LinkerGeneratedMethod Method { get; }

		/// <summary>
		/// Schedules the specified method for invocation in the main.
		/// </summary>
		/// <param name="method">The method.</param>
		void Schedule(RuntimeMethod method);
	}
}