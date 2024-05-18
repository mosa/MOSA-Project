using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class RangeAttribute : ValidationAttribute
{
	public bool ConvertValueInInvariantCulture
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object Maximum
	{
		get
		{
			throw null;
		}
	}

	public bool MaximumIsExclusive
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object Minimum
	{
		get
		{
			throw null;
		}
	}

	public bool MinimumIsExclusive
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	public Type OperandType
	{
		get
		{
			throw null;
		}
	}

	public bool ParseLimitsInInvariantCulture
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RangeAttribute(double minimum, double maximum)
	{
	}

	public RangeAttribute(int minimum, int maximum)
	{
	}

	[RequiresUnreferencedCode("Generic TypeConverters may require the generic types to be annotated. For example, NullableConverter requires the underlying type to be DynamicallyAccessedMembers All.")]
	public RangeAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type, string minimum, string maximum)
	{
	}

	public override string FormatErrorMessage(string name)
	{
		throw null;
	}

	public override bool IsValid(object? value)
	{
		throw null;
	}
}
