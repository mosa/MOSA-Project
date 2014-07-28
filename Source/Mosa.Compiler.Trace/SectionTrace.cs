/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.InternalTrace
{
	public class SectionTrace
	{
		private CompilerTrace compilerTrace;

		public string Stage { get; set; }

		public MosaMethod Method { get; set; }

		public string Section { get; set; }

		public bool Active { get; private set; }

		public SectionTrace(CompilerTrace compilerTrace)
		{
			this.compilerTrace = compilerTrace;
			this.Active = true;
		}

		public SectionTrace(SectionTrace trace)
			: this(trace.compilerTrace)
		{
			this.Method = trace.Method;
			this.Section = this.Section;
			this.Stage = this.Stage;
			this.Active = compilerTrace.TraceFilter.IsMatch(this.Method, this.Stage);
		}

		public SectionTrace(SectionTrace trace, string section)
			: this(trace.compilerTrace)
		{
			this.Method = trace.Method;
			this.Stage = trace.Stage;
			this.Section = section;
			this.Active = compilerTrace.TraceFilter.IsMatch(this.Method, this.Stage);
		}

		public SectionTrace(CompilerTrace compilerTrace, MosaMethod method, string stage)
			: this(compilerTrace)
		{
			this.Stage = stage;
			this.Method = method;
			this.Active = compilerTrace.TraceFilter.IsMatch(this.Method, this.Stage);
		}

		public SectionTrace(CompilerTrace compilerTrace, MosaMethod method, string stage, string section)
			: this(compilerTrace)
		{
			this.Stage = stage;
			this.Section = section;
			this.Method = method;
			this.Active = compilerTrace.TraceFilter.IsMatch(this.Method, this.Stage);
		}

		public void Log(CompilerEvent compilerEvent, string message)
		{
			if (!Active)
				return;

			compilerTrace.CompilerEventListener.SubmitTraceEvent(compilerEvent, message);
		}

		public void Log(string line)
		{
			if (!Active)
				return;

			if (Section == null)
			{
				compilerTrace.TraceListener.SubmitDebugStageInformation(Method, Stage, line);
			}
			else
			{
				compilerTrace.TraceListener.SubmitDebugStageInformation(Method, Stage + "-" + Section, line);
			}
		}
	}
}