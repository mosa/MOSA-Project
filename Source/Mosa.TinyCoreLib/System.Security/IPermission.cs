namespace System.Security;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public interface IPermission : ISecurityEncodable
{
	IPermission Copy();

	void Demand();

	IPermission? Intersect(IPermission? target);

	bool IsSubsetOf(IPermission? target);

	IPermission? Union(IPermission? target);
}
