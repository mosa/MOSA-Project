using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;

namespace System;

public abstract class MulticastDelegate : Delegate
{
	[RequiresUnreferencedCode("The target method might be removed")]
	protected MulticastDelegate(object target, string method)
		: base((object)null, (string)null)
	{
	}

	protected MulticastDelegate([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type target, string method)
		: base((object)null, (string)null)
	{
	}

	protected sealed override Delegate CombineImpl(Delegate? follow)
	{
		throw null;
	}

	public sealed override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public sealed override int GetHashCode()
	{
		throw null;
	}

	public sealed override Delegate[] GetInvocationList()
	{
		throw null;
	}

	protected override MethodInfo GetMethodImpl()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public static bool operator ==(MulticastDelegate? d1, MulticastDelegate? d2)
	{
		throw null;
	}

	public static bool operator !=(MulticastDelegate? d1, MulticastDelegate? d2)
	{
		throw null;
	}

	protected sealed override Delegate? RemoveImpl(Delegate value)
	{
		throw null;
	}
}
