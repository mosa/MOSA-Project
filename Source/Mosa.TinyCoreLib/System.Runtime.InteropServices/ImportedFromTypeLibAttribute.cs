namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class ImportedFromTypeLibAttribute : Attribute
{
	public string Value
	{
		get
		{
			throw null;
		}
	}

	public ImportedFromTypeLibAttribute(string tlbFile)
	{
	}
}
