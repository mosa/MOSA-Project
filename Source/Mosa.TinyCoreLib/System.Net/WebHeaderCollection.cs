using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Net;

public class WebHeaderCollection : NameValueCollection, IEnumerable, ISerializable
{
	public override string[] AllKeys
	{
		get
		{
			throw null;
		}
	}

	public override int Count
	{
		get
		{
			throw null;
		}
	}

	public string? this[HttpRequestHeader header]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? this[HttpResponseHeader header]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override KeysCollection Keys
	{
		get
		{
			throw null;
		}
	}

	public WebHeaderCollection()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected WebHeaderCollection(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public void Add(HttpRequestHeader header, string? value)
	{
	}

	public void Add(HttpResponseHeader header, string? value)
	{
	}

	public void Add(string header)
	{
	}

	public override void Add(string name, string? value)
	{
	}

	protected void AddWithoutValidate(string headerName, string? headerValue)
	{
	}

	public override void Clear()
	{
	}

	public override string? Get(int index)
	{
		throw null;
	}

	public override string? Get(string? name)
	{
		throw null;
	}

	public override IEnumerator GetEnumerator()
	{
		throw null;
	}

	public override string GetKey(int index)
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public override string[]? GetValues(int index)
	{
		throw null;
	}

	public override string[]? GetValues(string header)
	{
		throw null;
	}

	public static bool IsRestricted(string headerName)
	{
		throw null;
	}

	public static bool IsRestricted(string headerName, bool response)
	{
		throw null;
	}

	public override void OnDeserialization(object? sender)
	{
	}

	public void Remove(HttpRequestHeader header)
	{
	}

	public void Remove(HttpResponseHeader header)
	{
	}

	public override void Remove(string name)
	{
	}

	public void Set(HttpRequestHeader header, string? value)
	{
	}

	public void Set(HttpResponseHeader header, string? value)
	{
	}

	public override void Set(string name, string? value)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public byte[] ToByteArray()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
