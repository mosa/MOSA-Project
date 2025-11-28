namespace System.Runtime.ConstrainedExecution;

[Obsolete("The Constrained Execution Region (CER) feature is not supported.", DiagnosticId = "SYSLIB0004", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public enum Cer
{
	None,
	MayFail,
	Success
}
