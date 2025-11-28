using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
public sealed class SwitchAttribute : Attribute
{
	public string? SwitchDescription
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string SwitchName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Type SwitchType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SwitchAttribute(string switchName, Type switchType)
	{
	}

	[RequiresUnreferencedCode("Types may be trimmed from the assembly.")]
	public static SwitchAttribute[] GetAll(Assembly assembly)
	{
		throw null;
	}
}
