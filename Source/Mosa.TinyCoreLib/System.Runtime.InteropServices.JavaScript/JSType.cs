using System.Runtime.Versioning;

namespace System.Runtime.InteropServices.JavaScript;

[SupportedOSPlatform("browser")]
public abstract class JSType
{
	public sealed class Void : JSType
	{
		internal Void()
		{
			throw null;
		}
	}

	public sealed class Discard : JSType
	{
		internal Discard()
		{
			throw null;
		}
	}

	public sealed class Boolean : JSType
	{
		internal Boolean()
		{
			throw null;
		}
	}

	public sealed class Number : JSType
	{
		internal Number()
		{
			throw null;
		}
	}

	public sealed class BigInt : JSType
	{
		internal BigInt()
		{
			throw null;
		}
	}

	public sealed class Date : JSType
	{
		internal Date()
		{
			throw null;
		}
	}

	public sealed class String : JSType
	{
		internal String()
		{
			throw null;
		}
	}

	public sealed class Object : JSType
	{
		internal Object()
		{
			throw null;
		}
	}

	public sealed class Error : JSType
	{
		internal Error()
		{
			throw null;
		}
	}

	public sealed class MemoryView : JSType
	{
		internal MemoryView()
		{
			throw null;
		}
	}

	public sealed class Array<T> : JSType where T : JSType
	{
		internal Array()
		{
			throw null;
		}
	}

	public sealed class Promise<T> : JSType where T : JSType
	{
		internal Promise()
		{
			throw null;
		}
	}

	public sealed class Function : JSType
	{
		internal Function()
		{
			throw null;
		}
	}

	public sealed class Function<T> : JSType where T : JSType
	{
		internal Function()
		{
			throw null;
		}
	}

	public sealed class Function<T1, T2> : JSType where T1 : JSType where T2 : JSType
	{
		internal Function()
		{
			throw null;
		}
	}

	public sealed class Function<T1, T2, T3> : JSType where T1 : JSType where T2 : JSType where T3 : JSType
	{
		internal Function()
		{
			throw null;
		}
	}

	public sealed class Function<T1, T2, T3, T4> : JSType where T1 : JSType where T2 : JSType where T3 : JSType where T4 : JSType
	{
		internal Function()
		{
			throw null;
		}
	}

	public sealed class Any : JSType
	{
		internal Any()
		{
			throw null;
		}
	}

	internal JSType()
	{
		throw null;
	}
}
