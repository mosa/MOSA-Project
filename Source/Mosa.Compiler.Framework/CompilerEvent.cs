// Copyright (c) MOSA Project. Licensed under the New BSD License.

public enum CompilerEvent
{
	CompilerStart,
	CompilerEnd,

	CompilingMethodsStart,
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
		return stage switch
		{
			CompilerEvent.CompilerStart => "Compiling",
			CompilerEvent.CompilerEnd => "Compiling [Completed]",
			CompilerEvent.CompilingMethodsStart => "Compiling Methods",
			CompilerEvent.CompilingMethodsCompleted => "Compiling Methods [Completed]",
			CompilerEvent.MethodCompileStart => "Compiling Method",
			CompilerEvent.MethodCompileEnd => "Compiling Method [Completed]",
			CompilerEvent.MethodScheduled => "Method Scheduled",
			CompilerEvent.InlineMethodsScheduled => "Inline Methods Scheduled",
			CompilerEvent.LinkingStart => "Linking",
			CompilerEvent.LinkingEnd => "Linking [Completed]",
			CompilerEvent.SetupStart => "Setting Up",
			CompilerEvent.SetupEnd => "Setting Up [Completed]",
			CompilerEvent.FinalizationStart => "Finalizing",
			CompilerEvent.FinalizationEnd => "Finalizing [Completed]",
			CompilerEvent.SetupStageStart => "Setting Up Stage ",
			CompilerEvent.SetupStageEnd => "Setting Up Stage [Completed]",
			CompilerEvent.FinalizationStageStart => "Finalizing Stage",
			CompilerEvent.FinalizationStageEnd => "Finalizing Stage [Completed]",
			CompilerEvent.DebugInfo => "Debug Info",
			CompilerEvent.Warning => "Warning",
			CompilerEvent.Error => "Error",
			CompilerEvent.Exception => "Exception",
			_ => stage.ToString()
		};
	}
}
