/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.InternalTrace
{
	public class CompilerTrace
	{
		private IInternalTrace internalTrace;
		
		public string Stage { get; set; }

		public RuntimeMethod Method { get; set; }

		public string Section { get; set; }

		public bool Active { get; private set; }

		public CompilerTrace(IInternalTrace internalTrace)
		{
			this.internalTrace = internalTrace;
			this.Active = true;
		}

		public CompilerTrace(CompilerTrace trace)
			: this(trace.internalTrace)
		{
			this.Method = trace.Method;
			this.Section = this.Section;
			this.Stage = this.Stage;
			this.Active = internalTrace.TraceFilter.IsMatch(this.Method, this.Stage);
		}

		public CompilerTrace(CompilerTrace trace, string section)
			: this(trace.internalTrace)
		{
			this.Method = trace.Method;
			this.Stage = trace.Stage;
			this.Section = section;
			this.Active = internalTrace.TraceFilter.IsMatch(this.Method, this.Stage);
		}

		public CompilerTrace(IInternalTrace internalTrace, RuntimeMethod method, string stage)
			: this(internalTrace)
		{
			this.Stage = stage;
			this.Method = method;
			this.Active = internalTrace.TraceFilter.IsMatch(this.Method, this.Stage);
		}

		public CompilerTrace(IInternalTrace internalTrace, RuntimeMethod method, string stage, string section)
			: this(internalTrace)
		{
			this.Stage = stage;
			this.Section = section;
			this.Method = method;
			this.Active = internalTrace.TraceFilter.IsMatch(this.Method, this.Stage);
		}

		public void Log(CompilerEvent compilerEvent, string message)
		{
			if (!Active)
				return;

			internalTrace.CompilerEventListener.SubmitTraceEvent(compilerEvent, message);
		}

		public void Log(string line)
		{
			if (!Active)
				return;

			if (Section == null)
			{
				internalTrace.TraceListener.SubmitDebugStageInformation(Method, Stage, line);
			}
			else
			{
				internalTrace.TraceListener.SubmitDebugStageInformation(Method, Stage + "-" + Section, line);
			}
		}
	}
}