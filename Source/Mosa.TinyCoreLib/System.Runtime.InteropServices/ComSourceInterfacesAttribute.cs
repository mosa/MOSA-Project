using System.ComponentModel;

namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class ComSourceInterfacesAttribute : Attribute
{
	public string Value
	{
		get
		{
			throw null;
		}
	}

	public ComSourceInterfacesAttribute(string sourceInterfaces)
	{
	}

	public ComSourceInterfacesAttribute(Type sourceInterface)
	{
	}

	public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2)
	{
	}

	public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3)
	{
	}

	public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3, Type sourceInterface4)
	{
	}
}
