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
using Mosa.Compiler.Framework.Linker;

namespace Mosa.Compiler.Framework
{
	public interface ITypeInitializerSchedulerStage
	{
		/// <summary>
		/// Gets the type initializer method.
		/// </summary>
		LinkerGeneratedMethod TypeInitializerMethod { get; }

		/// <summary>
		/// Schedules the specified method for invocation in the main.
		/// </summary>
		/// <param name="method">The method.</param>
		void Schedule(RuntimeMethod method);
	}
}