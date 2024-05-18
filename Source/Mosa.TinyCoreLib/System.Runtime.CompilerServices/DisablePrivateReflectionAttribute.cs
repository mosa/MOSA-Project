namespace System.Runtime.CompilerServices;

[Obsolete("DisablePrivateReflectionAttribute has no effect in .NET 6.0+.", DiagnosticId = "SYSLIB0015", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public sealed class DisablePrivateReflectionAttribute : Attribute
{
}
