namespace Microsoft.Win32;

public static class Registry
{
	public static readonly RegistryKey ClassesRoot;

	public static readonly RegistryKey CurrentConfig;

	public static readonly RegistryKey CurrentUser;

	public static readonly RegistryKey LocalMachine;

	public static readonly RegistryKey PerformanceData;

	public static readonly RegistryKey Users;

	public static object? GetValue(string keyName, string? valueName, object? defaultValue)
	{
		throw null;
	}

	public static void SetValue(string keyName, string? valueName, object value)
	{
	}

	public static void SetValue(string keyName, string? valueName, object value, RegistryValueKind valueKind)
	{
	}
}
