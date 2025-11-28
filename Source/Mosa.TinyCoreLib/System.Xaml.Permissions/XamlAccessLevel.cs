using System.Reflection;

namespace System.Xaml.Permissions;

public class XamlAccessLevel
{
	public AssemblyName AssemblyAccessToAssemblyName
	{
		get
		{
			throw null;
		}
	}

	public string? PrivateAccessToTypeName
	{
		get
		{
			throw null;
		}
	}

	internal XamlAccessLevel()
	{
	}

	public static XamlAccessLevel AssemblyAccessTo(Assembly assembly)
	{
		throw null;
	}

	public static XamlAccessLevel AssemblyAccessTo(AssemblyName assemblyName)
	{
		throw null;
	}

	public static XamlAccessLevel PrivateAccessTo(string assemblyQualifiedTypeName)
	{
		throw null;
	}

	public static XamlAccessLevel PrivateAccessTo(Type type)
	{
		throw null;
	}
}
