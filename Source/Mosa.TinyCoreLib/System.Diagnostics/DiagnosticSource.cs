using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

public abstract class DiagnosticSource
{
	public abstract bool IsEnabled(string name);

	public virtual bool IsEnabled(string name, object? arg1, object? arg2 = null)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The type of object being written to DiagnosticSource cannot be discovered statically.")]
	public abstract void Write(string name, object? value);

	[RequiresUnreferencedCode("Only the properties of the T type will be preserved. Properties of referenced types and properties of derived types may be trimmed.")]
	public void Write<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(string name, T value)
	{
	}

	public virtual void OnActivityExport(Activity activity, object? payload)
	{
	}

	public virtual void OnActivityImport(Activity activity, object? payload)
	{
	}

	[RequiresUnreferencedCode("The type of object being written to DiagnosticSource cannot be discovered statically.")]
	public Activity StartActivity(Activity activity, object? args)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Only the properties of the T type will be preserved. Properties of referenced types and properties of derived types may be trimmed.")]
	public Activity StartActivity<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(Activity activity, T args)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The type of object being written to DiagnosticSource cannot be discovered statically.")]
	public void StopActivity(Activity activity, object? args)
	{
	}

	[RequiresUnreferencedCode("Only the properties of the T type will be preserved. Properties of referenced types and properties of derived types may be trimmed.")]
	public void StopActivity<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(Activity activity, T args)
	{
		throw null;
	}
}
