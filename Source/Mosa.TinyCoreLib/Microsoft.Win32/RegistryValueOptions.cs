using System;

namespace Microsoft.Win32;

[Flags]
public enum RegistryValueOptions
{
	None = 0,
	DoNotExpandEnvironmentNames = 1
}
