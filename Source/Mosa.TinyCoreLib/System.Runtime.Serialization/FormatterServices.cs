using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization.Formatters;

namespace System.Runtime.Serialization;

[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public static class FormatterServices
{
	public static void CheckTypeSecurity(Type t, TypeFilterLevel securityLevel)
	{
	}

	public static object?[] GetObjectData(object obj, MemberInfo[] members)
	{
		throw null;
	}

	public static object GetSafeUninitializedObject([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type type)
	{
		throw null;
	}

	public static MemberInfo[] GetSerializableMembers([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type)
	{
		throw null;
	}

	public static MemberInfo[] GetSerializableMembers([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type, StreamingContext context)
	{
		throw null;
	}

	public static ISerializationSurrogate GetSurrogateForCyclicalReference(ISerializationSurrogate innerSurrogate)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public static Type? GetTypeFromAssembly(Assembly assem, string name)
	{
		throw null;
	}

	public static object GetUninitializedObject([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type type)
	{
		throw null;
	}

	public static object PopulateObjectMembers(object obj, MemberInfo[] members, object?[] data)
	{
		throw null;
	}
}
