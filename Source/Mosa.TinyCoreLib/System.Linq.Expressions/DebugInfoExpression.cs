namespace System.Linq.Expressions;

public class DebugInfoExpression : Expression
{
	public SymbolDocumentInfo Document
	{
		get
		{
			throw null;
		}
	}

	public virtual int EndColumn
	{
		get
		{
			throw null;
		}
	}

	public virtual int EndLine
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsClear
	{
		get
		{
			throw null;
		}
	}

	public sealed override ExpressionType NodeType
	{
		get
		{
			throw null;
		}
	}

	public virtual int StartColumn
	{
		get
		{
			throw null;
		}
	}

	public virtual int StartLine
	{
		get
		{
			throw null;
		}
	}

	public sealed override Type Type
	{
		get
		{
			throw null;
		}
	}

	internal DebugInfoExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}
}
