using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;

namespace System;

public abstract class Delegate : ICloneable, ISerializable
{
	public MethodInfo Method
	{
		get
		{
			throw null;
		}
	}

	public object? Target
	{
		get
		{
			throw null;
		}
	}

	[RequiresUnreferencedCode("The target method might be removed")]
	protected Delegate(object target, string method)
	{
	}

	protected Delegate([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type target, string method)
	{
	}

	public virtual object Clone()
	{
		throw null;
	}

	[return: NotNullIfNotNull("a")]
	[return: NotNullIfNotNull("b")]
	public static Delegate? Combine(Delegate? a, Delegate? b)
	{
		throw null;
	}

	public static Delegate? Combine(params Delegate?[]? delegates)
	{
		throw null;
	}

	protected virtual Delegate CombineImpl(Delegate? d)
	{
		throw null;
	}

	public static Delegate CreateDelegate(Type type, object? firstArgument, MethodInfo method)
	{
		throw null;
	}

	public static Delegate? CreateDelegate(Type type, object? firstArgument, MethodInfo method, bool throwOnBindFailure)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The target method might be removed")]
	public static Delegate CreateDelegate(Type type, object target, string method)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The target method might be removed")]
	public static Delegate CreateDelegate(Type type, object target, string method, bool ignoreCase)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The target method might be removed")]
	public static Delegate? CreateDelegate(Type type, object target, string method, bool ignoreCase, bool throwOnBindFailure)
	{
		throw null;
	}

	public static Delegate CreateDelegate(Type type, MethodInfo method)
	{
		throw null;
	}

	public static Delegate? CreateDelegate(Type type, MethodInfo method, bool throwOnBindFailure)
	{
		throw null;
	}

	public static Delegate CreateDelegate(Type type, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type target, string method)
	{
		throw null;
	}

	public static Delegate CreateDelegate(Type type, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type target, string method, bool ignoreCase)
	{
		throw null;
	}

	public static Delegate? CreateDelegate(Type type, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type target, string method, bool ignoreCase, bool throwOnBindFailure)
	{
		throw null;
	}

	public object? DynamicInvoke(params object?[]? args)
	{
		throw null;
	}

	protected virtual object? DynamicInvokeImpl(object?[]? args)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public virtual Delegate[] GetInvocationList()
	{
		throw null;
	}

	protected virtual MethodInfo GetMethodImpl()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public static bool operator ==(Delegate? d1, Delegate? d2)
	{
		throw null;
	}

	public static bool operator !=(Delegate? d1, Delegate? d2)
	{
		throw null;
	}

	public static Delegate? Remove(Delegate? source, Delegate? value)
	{
		throw null;
	}

	public static Delegate? RemoveAll(Delegate? source, Delegate? value)
	{
		throw null;
	}

	protected virtual Delegate? RemoveImpl(Delegate d)
	{
		throw null;
	}
}
