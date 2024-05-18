using System.Reflection;

namespace System.ComponentModel.Composition.Primitives;

public class ExportedDelegate
{
	protected ExportedDelegate()
	{
	}

	public ExportedDelegate(object? instance, MethodInfo method)
	{
	}

	public virtual Delegate? CreateDelegate(Type delegateType)
	{
		throw null;
	}
}
