// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Trace
{
	public enum CompilerEvent
	{
		CompileStart,
		CompileEnd,

		CompilingMethod,
		CompiledMethod,
		CompilingType,

		Linking,
		LinkingCompleted,
		CompilingMethods,
		CompilingMethodsCompleted,

		PreCompileStart,
		PreCompileEnd,

		PreCompileStageStart,
		PreCompileStageEnd,

		PostCompileStart,
		PostCompileEnd,

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
		Stopped,
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
				case CompilerEvent.DebugInfo: return "Debug Info";

				case CompilerEvent.CompileStart: return "Compile Start";
				case CompilerEvent.CompileEnd: return "Compile End";

				case CompilerEvent.CompilingMethods: return "Compiling Methods";
				case CompilerEvent.CompilingMethodsCompleted: return "Compiling Methods Completed";

				case CompilerEvent.Linking: return "Linking";
				case CompilerEvent.LinkingCompleted: return "Linking Completed";

				case CompilerEvent.PreCompileStart: return "Pre-Compile Start";
				case CompilerEvent.PreCompileEnd: return "Pre-Compile End";
				case CompilerEvent.PostCompileStart: return "Post-Compile Start";
				case CompilerEvent.PostCompileEnd: return "Post-Compile End";

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
