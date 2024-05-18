using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Reflection;

public static class RuntimeReflectionExtensions
{
	public static MethodInfo GetMethodInfo(this Delegate del)
	{
		throw null;
	}

	public static MethodInfo? GetRuntimeBaseDefinition(this MethodInfo method)
	{
		throw null;
	}

	public static EventInfo? GetRuntimeEvent([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] this Type type, string name)
	{
		throw null;
	}

	public static IEnumerable<EventInfo> GetRuntimeEvents([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)] this Type type)
	{
		throw null;
	}

	public static FieldInfo? GetRuntimeField([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] this Type type, string name)
	{
		throw null;
	}

	public static IEnumerable<FieldInfo> GetRuntimeFields([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] this Type type)
	{
		throw null;
	}

	public static InterfaceMapping GetRuntimeInterfaceMap(this TypeInfo typeInfo, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type interfaceType)
	{
		throw null;
	}

	public static MethodInfo? GetRuntimeMethod([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] this Type type, string name, Type[] parameters)
	{
		throw null;
	}

	public static IEnumerable<MethodInfo> GetRuntimeMethods([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] this Type type)
	{
		throw null;
	}

	public static IEnumerable<PropertyInfo> GetRuntimeProperties([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] this Type type)
	{
		throw null;
	}

	public static PropertyInfo? GetRuntimeProperty([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] this Type type, string name)
	{
		throw null;
	}
}
