// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Trace
{
	public enum CompilerEvent
	{
		CompileStart,
		CompileEnd,

		CompilingMethods,
		CompilingMethodsCompleted,

		MethodCompileStart,
		MethodCompileEnd,

		LinkingStart,
		LinkingEnd,

		SetupStart,
		SetupEnd,

		SetupStageStart,
		SetupStageEnd,

		FinalizationStart,
		FinalizationEnd,

		FinalizationStageStart,
		FinalizationStageEnd,

		MethodScheduled,   // unused
		InlineMethodsScheduled,

		DebugInfo,

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
				case CompilerEvent.CompileStart: return "Compile Started";
				case CompilerEvent.CompileEnd: return "Compile Completed";

				case CompilerEvent.CompilingMethods: return "Compiling Methods";
				case CompilerEvent.CompilingMethodsCompleted: return "Compiling Methods Completed";

				case CompilerEvent.MethodCompileStart: return "Method Compile Started";
				case CompilerEvent.MethodCompileEnd: return "Method Compile Completed";

				case CompilerEvent.MethodScheduled: return "Method Scheduled";
				case CompilerEvent.InlineMethodsScheduled: return "Inline Methods Scheduled";

				case CompilerEvent.LinkingStart: return "Linking Started";
				case CompilerEvent.LinkingEnd: return "Linking Completed";

				case CompilerEvent.SetupStart: return "Setup Started";
				case CompilerEvent.SetupEnd: return "Setup Completed";

				case CompilerEvent.FinalizationStart: return "Finalization Started";
				case CompilerEvent.FinalizationEnd: return "Finalization Completed";

				case CompilerEvent.SetupStageStart: return "Setup Stage Started";
				case CompilerEvent.SetupStageEnd: return "Setup Stage Completed";

				case CompilerEvent.FinalizationStageStart: return "Finalization Stage Started";
				case CompilerEvent.FinalizationStageEnd: return "Finalization Stage Completed";

				case CompilerEvent.DebugInfo: return "Debug Info";

				case CompilerEvent.Warning: return "Warning";
				case CompilerEvent.Error: return "Error";
				case CompilerEvent.Exception: return "Exception";

				default: return stage.ToString();
			}
		}
	}
}
