// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Trace
{
	public enum CompilerEvent
	{
		CompilingMethod,
		CompiledMethod,
		CompilingType,
		Linking,
		PreCompileStageStart,
		PreCompileStageEnd,
		PostCompileStageStart,
		PostCompileStageEnd,
		DebugInfo,
		SchedulingType,
		SchedulingMethod,
		Plug,
		Error,
		Exception,
		Warning,
		Counter,
		Special,
		StatusUpdate,
	};

	public static class CompilerEventExtension
	{
		public static string ToText(this CompilerEvent stage)
		{
			switch (stage)
			{
				case CompilerEvent.CompilingMethod: return "Compiling Method";
				case CompilerEvent.CompiledMethod: return "Compiled Method";
				case CompilerEvent.CompilingType: return "Compiling Type";
				case CompilerEvent.SchedulingType: return "Scheduling Type";
				case CompilerEvent.SchedulingMethod: return "Scheduling Method";
				case CompilerEvent.Linking: return "Linking";
				case CompilerEvent.DebugInfo: return "Debug Info";
				case CompilerEvent.PreCompileStageStart: return "Pre-Compile Stage Started";
				case CompilerEvent.PreCompileStageEnd: return "Pre-Compile Stage Ended";
				case CompilerEvent.PostCompileStageStart: return "Post-Compile Stage Started";
				case CompilerEvent.PostCompileStageEnd: return "Post-Compile Stage Ended";
				case CompilerEvent.Error: return "Error";
				case CompilerEvent.Exception: return "Exception";
				case CompilerEvent.Warning: return "Warning";
				default: return stage.ToString();
			}
		}
	}
}
