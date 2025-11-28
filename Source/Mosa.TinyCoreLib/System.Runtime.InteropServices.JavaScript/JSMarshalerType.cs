using System.ComponentModel;
using System.Runtime.Versioning;

namespace System.Runtime.InteropServices.JavaScript;

[SupportedOSPlatform("browser")]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class JSMarshalerType
{
	public static JSMarshalerType Void
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType Discard
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType Boolean
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType Byte
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType Char
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType Int16
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType Int32
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType Int52
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType BigInt64
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType Double
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType Single
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType IntPtr
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType JSObject
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType Object
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType String
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType Exception
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType DateTime
	{
		get
		{
			throw null;
		}
	}

	public static JSMarshalerType DateTimeOffset
	{
		get
		{
			throw null;
		}
	}

	private JSMarshalerType()
	{
		throw null;
	}

	public static JSMarshalerType Nullable(JSMarshalerType primitive)
	{
		throw null;
	}

	public static JSMarshalerType Task()
	{
		throw null;
	}

	public static JSMarshalerType Task(JSMarshalerType result)
	{
		throw null;
	}

	public static JSMarshalerType Array(JSMarshalerType element)
	{
		throw null;
	}

	public static JSMarshalerType ArraySegment(JSMarshalerType element)
	{
		throw null;
	}

	public static JSMarshalerType Span(JSMarshalerType element)
	{
		throw null;
	}

	public static JSMarshalerType Action()
	{
		throw null;
	}

	public static JSMarshalerType Action(JSMarshalerType arg1)
	{
		throw null;
	}

	public static JSMarshalerType Action(JSMarshalerType arg1, JSMarshalerType arg2)
	{
		throw null;
	}

	public static JSMarshalerType Action(JSMarshalerType arg1, JSMarshalerType arg2, JSMarshalerType arg3)
	{
		throw null;
	}

	public static JSMarshalerType Function(JSMarshalerType result)
	{
		throw null;
	}

	public static JSMarshalerType Function(JSMarshalerType arg1, JSMarshalerType result)
	{
		throw null;
	}

	public static JSMarshalerType Function(JSMarshalerType arg1, JSMarshalerType arg2, JSMarshalerType result)
	{
		throw null;
	}

	public static JSMarshalerType Function(JSMarshalerType arg1, JSMarshalerType arg2, JSMarshalerType arg3, JSMarshalerType result)
	{
		throw null;
	}
}
