// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Utility.UnitTestBisector;

internal sealed class AssertExceptionTraceListener : TraceListener
{
	public override void Write(string message)
	{
	}

	public override void WriteLine(string message)
	{
	}

	public override void Fail(string message, string detailMessage)
	{
		throw new AssertFailureException(message, detailMessage);
	}
}
