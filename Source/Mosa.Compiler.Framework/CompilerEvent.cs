// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Trace;

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
		return stage switch
		{
			CompilerEvent.CompileStart => "Compile Started",
			CompilerEvent.CompileEnd => "Compile Completed",
			CompilerEvent.CompilingMethods => "Compiling Methods",
			CompilerEvent.CompilingMethodsCompleted => "Compiling Methods Completed",
			CompilerEvent.MethodCompileStart => "Method Compile Started",
			CompilerEvent.MethodCompileEnd => "Method Compile Completed",
			CompilerEvent.MethodScheduled => "Method Scheduled",
			CompilerEvent.InlineMethodsScheduled => "Inline Methods Scheduled",
			CompilerEvent.LinkingStart => "Linking Started",
			CompilerEvent.LinkingEnd => "Linking Completed",
			CompilerEvent.SetupStart => "Setup Started",
			CompilerEvent.SetupEnd => "Setup Completed",
			CompilerEvent.FinalizationStart => "Finalization Started",
			CompilerEvent.FinalizationEnd => "Finalization Completed",
			CompilerEvent.SetupStageStart => "Setup Stage Started",
			CompilerEvent.SetupStageEnd => "Setup Stage Completed",
			CompilerEvent.FinalizationStageStart => "Finalization Stage Started",
			CompilerEvent.FinalizationStageEnd => "Finalization Stage Completed",
			CompilerEvent.DebugInfo => "Debug Info",
			CompilerEvent.Warning => "Warning",
			CompilerEvent.Error => "Error",
			CompilerEvent.Exception => "Exception",
			_ => stage.ToString()
		};
	}
}
