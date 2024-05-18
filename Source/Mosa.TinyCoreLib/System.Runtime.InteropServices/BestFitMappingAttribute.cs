namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false)]
public sealed class BestFitMappingAttribute : Attribute
{
	public bool ThrowOnUnmappableChar;

	public bool BestFitMapping
	{
		get
		{
			throw null;
		}
	}

	public BestFitMappingAttribute(bool BestFitMapping)
	{
	}
}
