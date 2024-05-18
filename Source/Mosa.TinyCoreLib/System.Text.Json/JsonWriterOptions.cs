using System.Text.Encodings.Web;

namespace System.Text.Json;

public struct JsonWriterOptions
{
	private object _dummy;

	private int _dummyPrimitive;

	public JavaScriptEncoder? Encoder
	{
		readonly get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Indented
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int MaxDepth
	{
		readonly get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool SkipValidation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}
}
