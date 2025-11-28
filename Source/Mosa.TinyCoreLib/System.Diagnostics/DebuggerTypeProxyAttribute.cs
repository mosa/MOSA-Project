using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public sealed class DebuggerTypeProxyAttribute : Attribute
{
	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	public string ProxyTypeName
	{
		get
		{
			throw null;
		}
	}

	public Type? Target
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? TargetTypeName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DebuggerTypeProxyAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] string typeName)
	{
	}

	public DebuggerTypeProxyAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type)
	{
	}
}
