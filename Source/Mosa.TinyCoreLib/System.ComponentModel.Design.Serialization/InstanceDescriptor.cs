using System.Collections;
using System.Reflection;

namespace System.ComponentModel.Design.Serialization;

public sealed class InstanceDescriptor
{
	public ICollection Arguments
	{
		get
		{
			throw null;
		}
	}

	public bool IsComplete
	{
		get
		{
			throw null;
		}
	}

	public MemberInfo? MemberInfo
	{
		get
		{
			throw null;
		}
	}

	public InstanceDescriptor(MemberInfo? member, ICollection? arguments)
	{
	}

	public InstanceDescriptor(MemberInfo? member, ICollection? arguments, bool isComplete)
	{
	}

	public object? Invoke()
	{
		throw null;
	}
}
