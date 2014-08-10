/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;

namespace System.Reflection
{
	/// <summary>
	/// Binding Flags.
	/// </summary>
	[Serializable]
	[Flags]
	public enum BindingFlags
	{
		/// <summary>
		/// Specifies no binding flag.
		/// </summary>
		Default = 0x00000000,

		/// <summary>
		/// Specifies that the case of the member name should not be considered when binding.
		/// </summary>
		IgnoreCase = 0x00000001,

		/// <summary>
		/// Specifies that only members declared at the level of the supplied type's hierarchy should be considered. Inherited members are not considered.
		/// </summary>
		DeclaredOnly = 0x00000002,

		/// <summary>
		/// Specifies that instance members are to be included in the search.
		/// </summary>
		Instance = 0x00000004,

		/// <summary>
		/// Specifies that static members are to be included in the search.
		/// </summary>
		Static = 0x00000008,

		/// <summary>
		/// Specifies that public members are to be included in the search.
		/// </summary>
		Public = 0x00000010,

		/// <summary>
		/// Specifies that non-public members are to be included in the search.
		/// </summary>
		NonPublic = 0x00000020,

		/// <summary>
		/// Specifies that public and protected static members up the hierarchy should be returned. Private static members in inherited classes are not returned. Static members include fields, methods, events, and properties. Nested types are not returned.
		/// </summary>
		FlattenHierarchy = 0x00000040,

		/// <summary>
		/// Specifies that a method is to be invoked. This must not be a constructor or a type initializer.
		/// </summary>
		InvokeMethod = 0x00000100,

		/// <summary>
		/// Specifies that Reflection should create an instance of the specified type. Calls the constructor that matches the given arguments. The supplied member name is ignored. If the type of lookup is not specified, (Instance | Public) will apply. It is not possible to call a type initializer.
		/// </summary>
		CreateInstance = 0x00000200,

		/// <summary>
		/// Specifies that the value of the specified field should be returned.
		/// </summary>
		GetField = 0x00000400,

		/// <summary>
		/// Specifies that the value of the specified field should be set.
		/// </summary>
		SetField = 0x00000800,

		/// <summary>
		/// Specifies that the value of the specified property should be returned.
		/// </summary>
		GetProperty = 0x00001000,

		/// <summary>
		/// Specifies that the value of the specified property should be set.
		/// </summary>
		SetProperty = 0x00002000,

		/// <summary>
		/// Specifies that types of the supplied arguments must exactly match the types of the corresponding formal parameters. Reflection throws an exception if the caller supplies a non-null Binder object, since that implies that the caller is supplying BindToXXX implementations that will pick the appropriate method.
		/// Reflection models the accessibility rules of the common type system. For example, if the caller is in the same assembly, the caller does not need special permissions for internal members. Otherwise, the caller needs ReflectionPermission. This is consistent with lookup of members that are protected, private, and so on.
		/// 
		/// The general principle is that ChangeType should perform only widening coercions, which never lose data. An example of a widening coercion is coercing a value that is a 32-bit signed integer to a value that is a 64-bit signed integer. This is distinguished from a narrowing coercion, which may lose data. An example of a narrowing coercion is coercing a 64-bit signed integer to a 32-bit signed integer.
		/// 
		/// The default binder ignores this flag, while custom binders can implement the semantics of this flag.
		/// </summary>
		ExactBinding = 0x00010000,

		/// <summary>
		/// Not implemented.
		/// </summary>
		SuppressChangeType = 0x00020000,

		/// <summary>
		/// Returns the set of members whose parameter count matches the number of supplied arguments. This binding flag is used for methods with parameters that have default values and methods with variable arguments (varargs). This flag should only be used with Type.InvokeMember.
		/// Parameters with default values are used only in calls where trailing arguments are omitted. They must be the last arguments.
		/// </summary>
		OptionalParamBinding = 0x00040000,
	}

}