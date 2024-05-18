using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Win32;

public sealed class RegistryKey : MarshalByRefObject, IDisposable
{
	public SafeRegistryHandle Handle
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public int SubKeyCount
	{
		get
		{
			throw null;
		}
	}

	public int ValueCount
	{
		get
		{
			throw null;
		}
	}

	public RegistryView View
	{
		get
		{
			throw null;
		}
	}

	internal RegistryKey()
	{
	}

	public void Close()
	{
	}

	public RegistryKey CreateSubKey(string subkey)
	{
		throw null;
	}

	public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck)
	{
		throw null;
	}

	public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions)
	{
		throw null;
	}

	public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions, RegistrySecurity? registrySecurity)
	{
		throw null;
	}

	public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity? registrySecurity)
	{
		throw null;
	}

	public RegistryKey CreateSubKey(string subkey, bool writable)
	{
		throw null;
	}

	public RegistryKey CreateSubKey(string subkey, bool writable, RegistryOptions options)
	{
		throw null;
	}

	public void DeleteSubKey(string subkey)
	{
	}

	public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
	{
	}

	public void DeleteSubKeyTree(string subkey)
	{
	}

	public void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey)
	{
	}

	public void DeleteValue(string name)
	{
	}

	public void DeleteValue(string name, bool throwOnMissingValue)
	{
	}

	public void Dispose()
	{
	}

	public void Flush()
	{
	}

	public static RegistryKey FromHandle(SafeRegistryHandle handle)
	{
		throw null;
	}

	public static RegistryKey FromHandle(SafeRegistryHandle handle, RegistryView view)
	{
		throw null;
	}

	public RegistrySecurity GetAccessControl()
	{
		throw null;
	}

	public RegistrySecurity GetAccessControl(AccessControlSections includeSections)
	{
		throw null;
	}

	public string[] GetSubKeyNames()
	{
		throw null;
	}

	public object? GetValue(string? name)
	{
		throw null;
	}

	[return: NotNullIfNotNull("defaultValue")]
	public object? GetValue(string? name, object? defaultValue)
	{
		throw null;
	}

	[return: NotNullIfNotNull("defaultValue")]
	public object? GetValue(string? name, object? defaultValue, RegistryValueOptions options)
	{
		throw null;
	}

	public RegistryValueKind GetValueKind(string? name)
	{
		throw null;
	}

	public string[] GetValueNames()
	{
		throw null;
	}

	public static RegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view)
	{
		throw null;
	}

	public static RegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName)
	{
		throw null;
	}

	public static RegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName, RegistryView view)
	{
		throw null;
	}

	public RegistryKey? OpenSubKey(string name)
	{
		throw null;
	}

	public RegistryKey? OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck)
	{
		throw null;
	}

	public RegistryKey? OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
	{
		throw null;
	}

	public RegistryKey? OpenSubKey(string name, bool writable)
	{
		throw null;
	}

	public RegistryKey? OpenSubKey(string name, RegistryRights rights)
	{
		throw null;
	}

	public void SetAccessControl(RegistrySecurity registrySecurity)
	{
	}

	public void SetValue(string? name, object value)
	{
	}

	public void SetValue(string? name, object value, RegistryValueKind valueKind)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
