/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.InternalLog
{
	public enum CompilerStage { CompilingMethod, CompilingType, Linking, AssemblyStageStart, AssemblyStageEnd, DebugInfo, SchedulingType, SchedulingMethod };

	public static class CompilerStageExtension
	{
		public static string ToText(this CompilerStage stage)
		{
			switch (stage)
			{
				case CompilerStage.CompilingMethod: return "Compiling Method";
				case CompilerStage.CompilingType: return "Compiling Type";
				case CompilerStage.SchedulingType: return "Scheduling Type";
				case CompilerStage.SchedulingMethod: return "Scheduling Method";
				case CompilerStage.Linking: return "Linking";
				case CompilerStage.DebugInfo: return "DebugInfo";
				case CompilerStage.AssemblyStageStart: return "Assembly Stage Started";
				case CompilerStage.AssemblyStageEnd: return "Assembly Stage Ended";
				default: return stage.ToString();
			}
		}
	}
}
