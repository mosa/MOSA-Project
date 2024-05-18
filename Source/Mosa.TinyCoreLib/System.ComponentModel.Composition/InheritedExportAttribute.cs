namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class InheritedExportAttribute : ExportAttribute
{
	public InheritedExportAttribute()
	{
	}

	public InheritedExportAttribute(string? contractName)
	{
	}

	public InheritedExportAttribute(string? contractName, Type? contractType)
	{
	}

	public InheritedExportAttribute(Type? contractType)
	{
	}
}
