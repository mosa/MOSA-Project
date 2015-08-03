// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Trace
{
	public enum CompilerEvent
	{
		CompilingMethod,
		CompilingType,
		Linking,
		CompilerStageStart,
		CompilerStageEnd,
		DebugInfo,
		SchedulingType,
		SchedulingMethod,
		Plug,
		Error,
		Exception,
		Warning,
		Counter,
		Special,
	};

	public static class CompilerEventExtension
	{
		public static string ToText(this CompilerEvent stage)
		{
			switch (stage)
			{
				case CompilerEvent.CompilingMethod: return "Compiling Method";
				case CompilerEvent.CompilingType: return "Compiling Type";
				case CompilerEvent.SchedulingType: return "Scheduling Type";
				case CompilerEvent.SchedulingMethod: return "Scheduling Method";
				case CompilerEvent.Linking: return "Linking";
				case CompilerEvent.DebugInfo: return "Debug Info";
				case CompilerEvent.CompilerStageStart: return "Stage Started";
				case CompilerEvent.CompilerStageEnd: return "Stage Ended";
				case CompilerEvent.Error: return "Error";
				case CompilerEvent.Exception: return "Exception";
				case CompilerEvent.Warning: return "Warning";
				default: return stage.ToString();
			}
		}
	}
}