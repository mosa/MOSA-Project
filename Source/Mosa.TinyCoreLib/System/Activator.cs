using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting;

namespace System;

public static class Activator
{
	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public static ObjectHandle? CreateInstance(string assemblyName, string typeName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public static ObjectHandle? CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder? binder, object?[]? args, CultureInfo? culture, object?[]? activationAttributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public static ObjectHandle? CreateInstance(string assemblyName, string typeName, object?[]? activationAttributes)
	{
		throw null;
	}

	public static object? CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type)
	{
		throw null;
	}

	public static object? CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type type, bool nonPublic)
	{
		throw null;
	}

	public static object? CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, params object?[]? args)
	{
		throw null;
	}

	public static object? CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, object?[]? args, object?[]? activationAttributes)
	{
		throw null;
	}

	public static object? CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type type, BindingFlags bindingAttr, Binder? binder, object?[]? args, CultureInfo? culture)
	{
		throw null;
	}

	public static object? CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type type, BindingFlags bindingAttr, Binder? binder, object?[]? args, CultureInfo? culture, object?[]? activationAttributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public static ObjectHandle? CreateInstanceFrom(string assemblyFile, string typeName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public static ObjectHandle? CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder? binder, object?[]? args, CultureInfo? culture, object?[]? activationAttributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public static ObjectHandle? CreateInstanceFrom(string assemblyFile, string typeName, object?[]? activationAttributes)
	{
		throw null;
	}

	public static T CreateInstance<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>()
	{
		throw null;
	}
}
