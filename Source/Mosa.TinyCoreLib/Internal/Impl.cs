// Copyright (c) MOSA Project. Licensed under the New BSD License.
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Internal;

internal static class Impl
{
	public static class Debug
	{
		public const string NoMessage = "No message";
		public const string NoDetails = "No details";
		public const string NoCategory = "No category";
		public const string NoValue = "No value";
		public const string FailText = "{0}\nDetails: {1}"; // {0} = Message, {1} = Details
		public const string WriteText = "DEBUG ({0}): {1}"; // {0} = Category, {1} = Value/Message
	}

	public static class ConstructorInfo
	{
		public const string ConstructorName = ".ctor";
		public const string TypeConstructorName = ".cctor";
	}

	public static class RuntimeHelpers
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void InitializeArray(Array array, RuntimeFieldHandle fldHandle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetHashCode(object o);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public new static extern bool Equals(object o1, object o2);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern T UnsafeCast<T>(object o) where T : class;

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IEnumerable<Assembly> GetAssemblies();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object CreateInstance(Type type, params object[] args);

		// The body of this function will be replaced by the EE with unsafe code
		// See getILIntrinsicImplementation for how this happens.
		[Intrinsic]
		public static bool EnumEquals<T>(T x, T y) where T : Enum => x.Equals(y);

		[Intrinsic]
		public static bool IsReferenceOrContainsReferences<T>() => throw new InvalidOperationException();
	}

	public static class Bmi1
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint ResetLowestSetBit(uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint TrailingZeroCount(uint value);
	}

	public static class Lzcnt
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint LeadingZeroCount(uint value);
	}

	public static class Popcnt
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint PopCount(uint value);
	}

	public static class Object
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Type GetType(object obj);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Type MemberwiseClone(object obj);
	}
}
