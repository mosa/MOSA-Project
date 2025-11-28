using System.Collections.Generic;

namespace System.Reflection;

public class MethodBody
{
	public virtual IList<ExceptionHandlingClause> ExceptionHandlingClauses
	{
		get
		{
			throw null;
		}
	}

	public virtual bool InitLocals
	{
		get
		{
			throw null;
		}
	}

	public virtual int LocalSignatureMetadataToken
	{
		get
		{
			throw null;
		}
	}

	public virtual IList<LocalVariableInfo> LocalVariables
	{
		get
		{
			throw null;
		}
	}

	public virtual int MaxStackSize
	{
		get
		{
			throw null;
		}
	}

	protected MethodBody()
	{
	}

	public virtual byte[]? GetILAsByteArray()
	{
		throw null;
	}
}
