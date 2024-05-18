namespace System.Security;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public interface IStackWalk
{
	void Assert();

	void Demand();

	void Deny();

	void PermitOnly();
}
