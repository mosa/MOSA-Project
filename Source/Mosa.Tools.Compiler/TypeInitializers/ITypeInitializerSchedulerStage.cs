/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <sharpos@michaelruck.de>
 */

using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler.TypeInitializers
{
	public interface ITypeInitializerSchedulerStage
	{
		/// <summary>
		/// Gets the intializer method.
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