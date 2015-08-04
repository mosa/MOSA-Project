// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Trace
{
	public interface ITraceFactory
	{
		TraceLog CreateTraceLog(string section);
	}
}
