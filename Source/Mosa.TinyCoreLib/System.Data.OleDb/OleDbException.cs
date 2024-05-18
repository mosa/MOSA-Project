using System.ComponentModel;
using System.Data.Common;
using System.Runtime.Serialization;

namespace System.Data.OleDb;

public sealed class OleDbException : DbException
{
	internal sealed class ErrorCodeConverter
	{
	}

	[TypeConverter(typeof(ErrorCodeConverter))]
	public override int ErrorCode
	{
		get
		{
			throw null;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public OleDbErrorCollection Errors
	{
		get
		{
			throw null;
		}
	}

	internal OleDbException()
	{
	}

	public override void GetObjectData(SerializationInfo si, StreamingContext context)
	{
	}
}
