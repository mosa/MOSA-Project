// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Utility.UnitTestBisector;

internal sealed class AssertCaptureScope : IDisposable
{
	private readonly List<(DefaultTraceListener Listener, bool AssertUiEnabled)> defaultListeners = new();
	private readonly TraceListener listener = new AssertExceptionTraceListener();

	public AssertCaptureScope()
	{
		foreach (TraceListener traceListener in Trace.Listeners)
		{
			if (traceListener is DefaultTraceListener defaultTraceListener)
			{
				defaultListeners.Add((defaultTraceListener, defaultTraceListener.AssertUiEnabled));
				defaultTraceListener.AssertUiEnabled = false;
			}
		}

		Trace.Listeners.Add(listener);
	}

	public void Dispose()
	{
		Trace.Listeners.Remove(listener);

		foreach (var (listener, assertUiEnabled) in defaultListeners)
		{
			listener.AssertUiEnabled = assertUiEnabled;
		}
	}
}
