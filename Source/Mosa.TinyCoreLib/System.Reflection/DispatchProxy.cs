using System.Diagnostics.CodeAnalysis;

namespace System.Reflection;

public abstract class DispatchProxy
{
	[RequiresDynamicCode("Creating a proxy instance requires generating code at runtime")]
	public static object Create([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type interfaceType, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type proxyType)
	{
		throw null;
	}

	[RequiresDynamicCode("Creating a proxy instance requires generating code at runtime")]
	public static T Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TProxy>() where TProxy : DispatchProxy
	{
		throw null;
	}

	protected abstract object? Invoke(MethodInfo? targetMethod, object?[]? args);
}
