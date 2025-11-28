using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Common;

public static class DbProviderFactories
{
	public static DbProviderFactory? GetFactory(DbConnection connection)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Provider type and its members might be trimmed if not referenced directly.")]
	public static DbProviderFactory GetFactory(DataRow providerRow)
	{
		throw null;
	}

	public static DbProviderFactory GetFactory(string providerInvariantName)
	{
		throw null;
	}

	public static DataTable GetFactoryClasses()
	{
		throw null;
	}

	public static IEnumerable<string> GetProviderInvariantNames()
	{
		throw null;
	}

	public static void RegisterFactory(string providerInvariantName, DbProviderFactory factory)
	{
	}

	public static void RegisterFactory(string providerInvariantName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] string factoryTypeAssemblyQualifiedName)
	{
	}

	public static void RegisterFactory(string providerInvariantName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type providerFactoryClass)
	{
	}

	public static bool TryGetFactory(string providerInvariantName, [NotNullWhen(true)] out DbProviderFactory? factory)
	{
		throw null;
	}

	public static bool UnregisterFactory(string providerInvariantName)
	{
		throw null;
	}
}
