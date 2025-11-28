using System.Diagnostics.CodeAnalysis;

namespace System.Reflection;

public static class TypeExtensions
{
	public static ConstructorInfo? GetConstructor([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] this Type type, Type[] types)
	{
		throw null;
	}

	public static ConstructorInfo[] GetConstructors([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] this Type type)
	{
		throw null;
	}

	public static ConstructorInfo[] GetConstructors([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] this Type type, BindingFlags bindingAttr)
	{
		throw null;
	}

	public static MemberInfo[] GetDefaultMembers([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicEvents)] this Type type)
	{
		throw null;
	}

	public static EventInfo? GetEvent([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] this Type type, string name)
	{
		throw null;
	}

	public static EventInfo? GetEvent([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)] this Type type, string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	public static EventInfo[] GetEvents([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] this Type type)
	{
		throw null;
	}

	public static EventInfo[] GetEvents([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)] this Type type, BindingFlags bindingAttr)
	{
		throw null;
	}

	public static FieldInfo? GetField([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] this Type type, string name)
	{
		throw null;
	}

	public static FieldInfo? GetField([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] this Type type, string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	public static FieldInfo[] GetFields([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] this Type type)
	{
		throw null;
	}

	public static FieldInfo[] GetFields([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] this Type type, BindingFlags bindingAttr)
	{
		throw null;
	}

	public static Type[] GetGenericArguments(this Type type)
	{
		throw null;
	}

	public static Type[] GetInterfaces([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] this Type type)
	{
		throw null;
	}

	public static MemberInfo[] GetMember([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicEvents)] this Type type, string name)
	{
		throw null;
	}

	public static MemberInfo[] GetMember([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] this Type type, string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	public static MemberInfo[] GetMembers([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicEvents)] this Type type)
	{
		throw null;
	}

	public static MemberInfo[] GetMembers([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] this Type type, BindingFlags bindingAttr)
	{
		throw null;
	}

	public static MethodInfo? GetMethod([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] this Type type, string name)
	{
		throw null;
	}

	public static MethodInfo? GetMethod([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] this Type type, string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	public static MethodInfo? GetMethod([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] this Type type, string name, Type[] types)
	{
		throw null;
	}

	public static MethodInfo[] GetMethods([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] this Type type)
	{
		throw null;
	}

	public static MethodInfo[] GetMethods([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] this Type type, BindingFlags bindingAttr)
	{
		throw null;
	}

	public static Type? GetNestedType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)] this Type type, string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	public static Type[] GetNestedTypes([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)] this Type type, BindingFlags bindingAttr)
	{
		throw null;
	}

	public static PropertyInfo[] GetProperties([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] this Type type)
	{
		throw null;
	}

	public static PropertyInfo[] GetProperties([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] this Type type, BindingFlags bindingAttr)
	{
		throw null;
	}

	public static PropertyInfo? GetProperty([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] this Type type, string name)
	{
		throw null;
	}

	public static PropertyInfo? GetProperty([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] this Type type, string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	public static PropertyInfo? GetProperty([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] this Type type, string name, Type? returnType)
	{
		throw null;
	}

	public static PropertyInfo? GetProperty([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] this Type type, string name, Type? returnType, Type[] types)
	{
		throw null;
	}

	public static bool IsAssignableFrom(this Type type, [NotNullWhen(true)] Type? c)
	{
		throw null;
	}

	public static bool IsInstanceOfType(this Type type, [NotNullWhen(true)] object? o)
	{
		throw null;
	}
}
