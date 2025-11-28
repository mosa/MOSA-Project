using System.Security.AccessControl;

namespace Microsoft.Win32;

public static class RegistryAclExtensions
{
	public static RegistrySecurity GetAccessControl(this RegistryKey key)
	{
		throw null;
	}

	public static RegistrySecurity GetAccessControl(this RegistryKey key, AccessControlSections includeSections)
	{
		throw null;
	}

	public static void SetAccessControl(this RegistryKey key, RegistrySecurity registrySecurity)
	{
	}
}
