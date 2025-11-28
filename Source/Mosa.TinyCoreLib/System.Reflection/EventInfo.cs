namespace System.Reflection;

public abstract class EventInfo : MemberInfo
{
	public virtual MethodInfo? AddMethod
	{
		get
		{
			throw null;
		}
	}

	public abstract EventAttributes Attributes { get; }

	public virtual Type? EventHandlerType
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsMulticast
	{
		get
		{
			throw null;
		}
	}

	public bool IsSpecialName
	{
		get
		{
			throw null;
		}
	}

	public override MemberTypes MemberType
	{
		get
		{
			throw null;
		}
	}

	public virtual MethodInfo? RaiseMethod
	{
		get
		{
			throw null;
		}
	}

	public virtual MethodInfo? RemoveMethod
	{
		get
		{
			throw null;
		}
	}

	public virtual void AddEventHandler(object? target, Delegate? handler)
	{
	}

	public override bool Equals(object? obj)
	{
		throw null;
	}

	public MethodInfo? GetAddMethod()
	{
		throw null;
	}

	public abstract MethodInfo? GetAddMethod(bool nonPublic);

	public override int GetHashCode()
	{
		throw null;
	}

	public MethodInfo[] GetOtherMethods()
	{
		throw null;
	}

	public virtual MethodInfo[] GetOtherMethods(bool nonPublic)
	{
		throw null;
	}

	public MethodInfo? GetRaiseMethod()
	{
		throw null;
	}

	public abstract MethodInfo? GetRaiseMethod(bool nonPublic);

	public MethodInfo? GetRemoveMethod()
	{
		throw null;
	}

	public abstract MethodInfo? GetRemoveMethod(bool nonPublic);

	public static bool operator ==(EventInfo? left, EventInfo? right)
	{
		throw null;
	}

	public static bool operator !=(EventInfo? left, EventInfo? right)
	{
		throw null;
	}

	public virtual void RemoveEventHandler(object? target, Delegate? handler)
	{
	}
}
