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
		void ScheduleTypeForCompilation(RuntimeType type);

		//void ScheduleMethodForCompilation(RuntimeMethod method);
	}
}
