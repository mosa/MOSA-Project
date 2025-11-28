using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Data;

public class PropertyCollection : Hashtable, ICloneable
{
	public PropertyCollection()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected PropertyCollection(SerializationInfo info, StreamingContext context)
	{
	}

	public override object Clone()
	{
		throw null;
	}
}
