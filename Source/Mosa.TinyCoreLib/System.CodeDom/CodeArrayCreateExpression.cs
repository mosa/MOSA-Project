namespace System.CodeDom;

public class CodeArrayCreateExpression : CodeExpression
{
	public CodeTypeReference CreateType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeExpressionCollection Initializers
	{
		get
		{
			throw null;
		}
	}

	public int Size
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeExpression SizeExpression
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeArrayCreateExpression()
	{
	}

	public CodeArrayCreateExpression(CodeTypeReference createType, CodeExpression size)
	{
	}

	public CodeArrayCreateExpression(CodeTypeReference createType, params CodeExpression[] initializers)
	{
	}

	public CodeArrayCreateExpression(CodeTypeReference createType, int size)
	{
	}

	public CodeArrayCreateExpression(string createType, CodeExpression size)
	{
	}

	public CodeArrayCreateExpression(string createType, params CodeExpression[] initializers)
	{
	}

	public CodeArrayCreateExpression(string createType, int size)
	{
	}

	public CodeArrayCreateExpression(Type createType, CodeExpression size)
	{
	}

	public CodeArrayCreateExpression(Type createType, params CodeExpression[] initializers)
	{
	}

	public CodeArrayCreateExpression(Type createType, int size)
	{
	}
}
