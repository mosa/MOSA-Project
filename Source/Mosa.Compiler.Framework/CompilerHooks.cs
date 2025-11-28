// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.Configuration;

namespace Mosa.Compiler.Framework;

public class CompilerHooks
{
	#region Delegates definitions

	public delegate void NotifyStatusHandler(string status);

	public delegate void NotifyProgressHandler(int totalMethods, int completedMethods);

	public delegate void NotifyEventHandler(CompilerEvent compilerEvent, string message, int threadID);

	public delegate void NotifyTraceLogHandler(TraceLog traceLog);

	public delegate void NotifyMethodCompiledHandler(MosaMethod method);

	public delegate NotifyTraceLogHandler NotifyMethodInstructionTraceHandler(MosaMethod method);

	public delegate NotifyTraceLogHandler NotifyMethodTranformTraceHandler(MosaMethod method);

	public delegate void ExtendCompilerPipelineHandler(Pipeline<BaseCompilerStage> pipeline);

	public delegate void ExtendMethodCompilerPipelineHandler(Pipeline<BaseMethodCompilerStage> pipeline, MosaSettings mosaSettings);

	public delegate int? GetMethodTraceLevelHandler(MosaMethod method);

	#endregion Delegates definitions

	public NotifyStatusHandler NotifyStatus;

	public NotifyProgressHandler NotifyProgress;

	public NotifyEventHandler NotifyEvent;

	public NotifyTraceLogHandler NotifyTraceLog;

	public NotifyMethodCompiledHandler NotifyMethodCompiled;

	public NotifyMethodInstructionTraceHandler NotifyMethodInstructionTrace;

	public NotifyMethodTranformTraceHandler NotifyMethodTranformTrace;

	public ExtendCompilerPipelineHandler ExtendCompilerPipeline;

	public ExtendMethodCompilerPipelineHandler ExtendMethodCompilerPipeline;

	public GetMethodTraceLevelHandler GetMethodTraceLevel;
}
