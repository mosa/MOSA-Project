using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Microsoft.VisualBasic.FileIO;

public class TextFieldParser : IDisposable
{
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string[]? CommentTokens
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string[]? Delimiters
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool EndOfData
	{
		get
		{
			throw null;
		}
	}

	public string ErrorLine
	{
		get
		{
			throw null;
		}
	}

	public long ErrorLineNumber
	{
		get
		{
			throw null;
		}
	}

	public int[]? FieldWidths
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool HasFieldsEnclosedInQuotes
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public long LineNumber
	{
		get
		{
			throw null;
		}
	}

	public FieldType TextFieldType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool TrimWhiteSpace
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TextFieldParser(Stream stream)
	{
	}

	public TextFieldParser(Stream stream, Encoding defaultEncoding)
	{
	}

	public TextFieldParser(Stream stream, Encoding defaultEncoding, bool detectEncoding)
	{
	}

	public TextFieldParser(Stream stream, Encoding defaultEncoding, bool detectEncoding, bool leaveOpen)
	{
	}

	public TextFieldParser(TextReader reader)
	{
	}

	public TextFieldParser(string path)
	{
	}

	public TextFieldParser(string path, Encoding defaultEncoding)
	{
	}

	public TextFieldParser(string path, Encoding defaultEncoding, bool detectEncoding)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void Close()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~TextFieldParser()
	{
	}

	public string? PeekChars(int numberOfChars)
	{
		throw null;
	}

	public string[]? ReadFields()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string? ReadLine()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string? ReadToEnd()
	{
		throw null;
	}

	public void SetDelimiters(params string[]? delimiters)
	{
	}

	public void SetFieldWidths(params int[]? fieldWidths)
	{
	}

	public void Dispose()
	{
	}
}
