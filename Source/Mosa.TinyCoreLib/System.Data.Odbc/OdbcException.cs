using System.Data.Common;
using System.Runtime.Serialization;

namespace System.Data.Odbc;

public sealed class OdbcException : DbException
{
	public OdbcErrorCollection Errors
	{
		get
		{
			throw null;
		}
	}

	public override string Source
	{
		get
		{
			throw null;
		}
	}

	internal OdbcException()
	{
	}

	public override void GetObjectData(SerializationInfo si, StreamingContext context)
	{
	}
}
