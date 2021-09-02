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
				case CompilerEvent.CompileStart: return "Compile Start";
				case CompilerEvent.CompileEnd: return "Compile Complete";

				case CompilerEvent.CompilingMethods: return "Compiling Methods";
				case CompilerEvent.CompilingMethodsCompleted: return "Compiling Methods Completed";

				case CompilerEvent.MethodCompileStart: return "Method Compile Start";
				case CompilerEvent.MethodCompileEnd: return "Method Compile Complet";

				case CompilerEvent.MethodScheduled: return "Method Scheduled";
				case CompilerEvent.InlineMethodsScheduled: return "Inline Methods Schedule";

				case CompilerEvent.LinkingStart: return "Linking Start";
				case CompilerEvent.LinkingEnd: return "Linking Complete";

				case CompilerEvent.SetupStart: return "Setup Start";
				case CompilerEvent.SetupEnd: return "Setup Complete";

				case CompilerEvent.FinalizationStart: return "Finalization Start";
				case CompilerEvent.FinalizationEnd: return "Finalization Complete";

				case CompilerEvent.SetupStageStart: return "Setup Stage Start";
				case CompilerEvent.SetupStageEnd: return "Setup Stage Complete";

				case CompilerEvent.FinalizationStageStart: return "Finalization Stage Start";
				case CompilerEvent.FinalizationStageEnd: return "Finalization Stage Complete";

				case CompilerEvent.DebugInfo: return "Debug Info";

				case CompilerEvent.Warning: return "Warning";
				case CompilerEvent.Error: return "Error";
				case CompilerEvent.Exception: return "Exception";

				default: return stage.ToString();
			}
		}
	}
}
