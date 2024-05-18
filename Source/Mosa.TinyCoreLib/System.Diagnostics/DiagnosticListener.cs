using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

public class DiagnosticListener : DiagnosticSource, IDisposable, IObservable<KeyValuePair<string, object?>>
{
	public static IObservable<DiagnosticListener> AllListeners
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public DiagnosticListener(string name)
	{
	}

	public virtual void Dispose()
	{
	}

	public bool IsEnabled()
	{
		throw null;
	}

	public override bool IsEnabled(string name)
	{
		throw null;
	}

	public override bool IsEnabled(string name, object? arg1, object? arg2 = null)
	{
		throw null;
	}

	public virtual IDisposable Subscribe(IObserver<KeyValuePair<string, object?>> observer)
	{
		throw null;
	}

	public virtual IDisposable Subscribe(IObserver<KeyValuePair<string, object?>> observer, Func<string, object?, object?, bool>? isEnabled)
	{
		throw null;
	}

	public virtual IDisposable Subscribe(IObserver<KeyValuePair<string, object?>> observer, Predicate<string>? isEnabled)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The type of object being written to DiagnosticSource cannot be discovered statically.")]
	public override void Write(string name, object? value)
	{
	}

	public override void OnActivityExport(Activity activity, object? payload)
	{
	}

	public override void OnActivityImport(Activity activity, object? payload)
	{
	}

	public virtual IDisposable Subscribe(IObserver<KeyValuePair<string, object?>> observer, Func<string, object?, object?, bool>? isEnabled, Action<Activity, object?>? onActivityImport = null, Action<Activity, object?>? onActivityExport = null)
	{
		throw null;
	}
}
