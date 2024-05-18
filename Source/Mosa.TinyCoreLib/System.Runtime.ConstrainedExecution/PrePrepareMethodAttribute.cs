namespace System.Runtime.ConstrainedExecution;

[Obsolete("The Constrained Execution Region (CER) feature is not supported.", DiagnosticId = "SYSLIB0004", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
public sealed class PrePrepareMethodAttribute : Attribute
{
}
