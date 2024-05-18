using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Versioning;

namespace System.Runtime.InteropServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public class ComAwareEventInfo : EventInfo
{
	public override EventAttributes Attributes
	{
		get
		{
			throw null;
		}
	}

	public override Type? DeclaringType
	{
		get
		{
			throw null;
		}
	}

	public override int MetadataToken
	{
		get
		{
			throw null;
		}
	}

	public override Module Module
	{
		get
		{
			throw null;
		}
	}

	public override string Name
	{
		get
		{
			throw null;
		}
	}

	public override Type? ReflectedType
	{
		get
		{
			throw null;
		}
	}

	public ComAwareEventInfo([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] Type type, string eventName)
	{
	}

	[SupportedOSPlatform("windows")]
	public override void AddEventHandler(object target, Delegate handler)
	{
	}

	public override MethodInfo? GetAddMethod(bool nonPublic)
	{
		throw null;
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		throw null;
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw null;
	}

	public override IList<CustomAttributeData> GetCustomAttributesData()
	{
		throw null;
	}

	public override MethodInfo[] GetOtherMethods(bool nonPublic)
	{
		throw null;
	}

	public override MethodInfo? GetRaiseMethod(bool nonPublic)
	{
		throw null;
	}

	public override MethodInfo? GetRemoveMethod(bool nonPublic)
	{
		throw null;
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public override void RemoveEventHandler(object target, Delegate handler)
	{
	}
}
