using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class AmbientValueAttribute : Attribute
{
	public object? Value
	{
		get
		{
			throw null;
		}
	}

	public AmbientValueAttribute(bool value)
	{
	}

	public AmbientValueAttribute(byte value)
	{
	}

	public AmbientValueAttribute(char value)
	{
	}

	public AmbientValueAttribute(double value)
	{
	}

	public AmbientValueAttribute(short value)
	{
	}

	public AmbientValueAttribute(int value)
	{
	}

	public AmbientValueAttribute(long value)
	{
	}

	public AmbientValueAttribute(object? value)
	{
	}

	public AmbientValueAttribute(float value)
	{
	}

	public AmbientValueAttribute(string? value)
	{
	}

	[RequiresUnreferencedCode("Generic TypeConverters may require the generic types to be annotated. For example, NullableConverter requires the underlying type to be DynamicallyAccessedMembers All.")]
	public AmbientValueAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type, string value)
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
}
