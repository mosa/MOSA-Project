using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Text.RegularExpressions;

public sealed class RegexParseException : ArgumentException
{
	public RegexParseError Error
	{
		get
		{
			throw null;
		}
	}

	public int Offset
	{
		get
		{
			throw null;
		}
	}

	private RegexParseException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
