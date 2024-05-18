namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public sealed class MaybeNullWhenAttribute : Attribute
{
	public bool ReturnValue
	{
		get
		{
			throw null;
		}
	}

	public MaybeNullWhenAttribute(bool returnValue)
	{
	}
}
