using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public class DefaultValueAttribute : Attribute
{
	public virtual object? Value
	{
		get
		{
			throw null;
		}
	}

	public DefaultValueAttribute(bool value)
	{
	}

	public DefaultValueAttribute(byte value)
	{
	}

	public DefaultValueAttribute(char value)
	{
	}

	public DefaultValueAttribute(double value)
	{
	}

	public DefaultValueAttribute(short value)
	{
	}

	public DefaultValueAttribute(int value)
	{
	}

	public DefaultValueAttribute(long value)
	{
	}

	public DefaultValueAttribute(object? value)
	{
	}

	[CLSCompliant(false)]
	public DefaultValueAttribute(sbyte value)
	{
	}

	public DefaultValueAttribute(float value)
	{
	}

	public DefaultValueAttribute(string? value)
	{
	}

	[RequiresUnreferencedCode("Generic TypeConverters may require the generic types to be annotated. For example, NullableConverter requires the underlying type to be DynamicallyAccessedMembers All.")]
	public DefaultValueAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type, string? value)
	{
	}

	[CLSCompliant(false)]
	public DefaultValueAttribute(ushort value)
	{
	}

	[CLSCompliant(false)]
	public DefaultValueAttribute(uint value)
	{
	}

	[CLSCompliant(false)]
	public DefaultValueAttribute(ulong value)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	protected void SetValue(object? value)
	{
	}
}
