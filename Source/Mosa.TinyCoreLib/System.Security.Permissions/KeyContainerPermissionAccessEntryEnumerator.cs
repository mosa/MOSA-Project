using System.Collections;

namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class KeyContainerPermissionAccessEntryEnumerator : IEnumerator
{
	public KeyContainerPermissionAccessEntry Current
	{
		get
		{
			throw null;
		}
	}

	object IEnumerator.Current
	{
		get
		{
			throw null;
		}
	}

	public bool MoveNext()
	{
		throw null;
	}

	public void Reset()
	{
	}
}
