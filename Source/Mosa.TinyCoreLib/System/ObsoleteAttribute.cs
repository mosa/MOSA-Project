namespace System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
public sealed class ObsoleteAttribute : Attribute
{
	public string? DiagnosticId
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsError
	{
		get
		{
			throw null;
		}
	}

	public string? Message
	{
		get
		{
			throw null;
		}
	}

	public string? UrlFormat
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ObsoleteAttribute()
	{
	}

	public ObsoleteAttribute(string? message)
	{
	}

	public ObsoleteAttribute(string? message, bool error)
	{
	}
}
