namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public enum SecurityAction
{
	Demand = 2,
	Assert,
	Deny,
	PermitOnly,
	LinkDemand,
	InheritanceDemand,
	RequestMinimum,
	RequestOptional,
	RequestRefuse
}
