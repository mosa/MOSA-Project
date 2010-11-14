/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (aka Michael Ruck or grover) <sharpos@michaelruck.de>
 */

using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{

	public interface ICompilationSchedulerStage
	{
		void ScheduleTypeForCompilation(RuntimeType type);

		//void ScheduleMethodForCompilation(RuntimeMethod method);
	}
}
