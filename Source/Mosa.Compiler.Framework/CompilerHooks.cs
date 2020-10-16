// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	public class CompilerHooks
	{
		#region Delegates definitions

		public delegate void NotifyStatusHandler(string status);

		public delegate void NotifyProgressHandler(int totalMethods, int completedMethods);

		public delegate void NotifyEventHandler(CompilerEvent compilerEvent, string message, int threadID);

		public delegate void NotifyTraceLogHandler(TraceLog traceLog);

		public delegate void NotifyMethodCompiledHandler(MosaMethod method);

		public delegate IList<CustomELFSection> CustomElfSectionsHandler();

		public delegate NotifyTraceLogHandler NotifyMethodInstructionTraceHandler(MosaMethod method);

		public delegate void ExtendCompilerPipelineHandler(Pipeline<BaseCompilerStage> pipeline);

		public delegate void ExtendMethodCompilerPipelineHandler(Pipeline<BaseMethodCompilerStage> pipeline);

		#endregion Delegates definitions

		public NotifyStatusHandler NotifyStatus;

		public NotifyProgressHandler NotifyProgress;

		public NotifyEventHandler NotifyEvent;

		public NotifyTraceLogHandler NotifyTraceLog;

		public NotifyMethodCompiledHandler NotifyMethodCompiled;

		public CustomElfSectionsHandler CustomElfSections;

		public NotifyMethodInstructionTraceHandler NotifyMethodInstructionTrace;

		public ExtendCompilerPipelineHandler ExtendCompilerPipeline;

		public ExtendMethodCompilerPipelineHandler ExtendMethodCompilerPipeline;
	}
}
