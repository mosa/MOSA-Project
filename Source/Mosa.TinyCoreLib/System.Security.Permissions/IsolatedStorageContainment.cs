namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public enum IsolatedStorageContainment
{
	None = 0,
	DomainIsolationByUser = 16,
	ApplicationIsolationByUser = 21,
	AssemblyIsolationByUser = 32,
	DomainIsolationByMachine = 48,
	AssemblyIsolationByMachine = 64,
	ApplicationIsolationByMachine = 69,
	DomainIsolationByRoamingUser = 80,
	AssemblyIsolationByRoamingUser = 96,
	ApplicationIsolationByRoamingUser = 101,
	AdministerIsolatedStorageByUser = 112,
	UnrestrictedIsolatedStorage = 240
}
