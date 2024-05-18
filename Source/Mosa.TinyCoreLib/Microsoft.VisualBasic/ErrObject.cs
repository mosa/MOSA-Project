using System;

namespace Microsoft.VisualBasic;

public sealed class ErrObject
{
	public string Description
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Erl
	{
		get
		{
			throw null;
		}
	}

	public int HelpContext
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string HelpFile
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int LastDllError
	{
		get
		{
			throw null;
		}
	}

	public int Number
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Source
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal ErrObject()
	{
	}

	public void Clear()
	{
	}

	public Exception? GetException()
	{
		throw null;
	}

	public void Raise(int Number, object? Source = null, object? Description = null, object? HelpFile = null, object? HelpContext = null)
	{
	}
}
