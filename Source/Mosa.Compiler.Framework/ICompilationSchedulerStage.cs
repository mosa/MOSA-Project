/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (aka Michael Ruck or grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{

	public interface ICompilationSchedulerStage
	{
		/// <summary>
		/// Schedules the type for compilation.
		/// </summary>
		/// <param name="type">The type.</param>
		void ScheduleTypeForCompilation(RuntimeType type);

		/// <summary>
		/// Schedules the method for compilation.
		/// </summary>
		/// <param name="method">The method.</param>
		void ScheduleMethodForCompilation(RuntimeMethod method);
	}
}
