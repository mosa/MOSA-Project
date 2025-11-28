using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Collections;

public sealed class Comparer : IComparer, ISerializable
{
	public static readonly Comparer Default;

	public static readonly Comparer DefaultInvariant;

	public Comparer(CultureInfo culture)
	{
	}

	public int Compare(object? a, object? b)
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
