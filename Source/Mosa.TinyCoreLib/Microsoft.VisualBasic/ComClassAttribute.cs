using System;

namespace Microsoft.VisualBasic;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ComClassAttribute : Attribute
{
	public string? ClassID
	{
		get
		{
			throw null;
		}
	}

	public string? EventID
	{
		get
		{
			throw null;
		}
	}

	public string? InterfaceID
	{
		get
		{
			throw null;
		}
	}

	public bool InterfaceShadows
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ComClassAttribute()
	{
	}

	public ComClassAttribute(string? _ClassID)
	{
	}

	public ComClassAttribute(string? _ClassID, string? _InterfaceID)
	{
	}

	public ComClassAttribute(string? _ClassID, string? _InterfaceID, string? _EventId)
	{
	}
}
