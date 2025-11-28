namespace System.Security.AccessControl;

public enum ResourceType
{
	Unknown,
	FileObject,
	Service,
	Printer,
	RegistryKey,
	LMShare,
	KernelObject,
	WindowObject,
	DSObject,
	DSObjectAll,
	ProviderDefined,
	WmiGuidObject,
	RegistryWow6432Key
}
