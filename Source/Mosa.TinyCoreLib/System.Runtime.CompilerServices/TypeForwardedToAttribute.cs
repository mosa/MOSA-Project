namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
public sealed class TypeForwardedToAttribute : Attribute
{
	public Type Destination
	{
		get
		{
			throw null;
		}
	}

	public TypeForwardedToAttribute(Type destination)
	{
	}
}
