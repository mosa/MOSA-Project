using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Data;

public sealed class DBConcurrencyException : SystemException
{
	public DataRow? Row
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public int RowCount
	{
		get
		{
			throw null;
		}
	}

	public DBConcurrencyException()
	{
	}

	public DBConcurrencyException(string? message)
	{
	}

	public DBConcurrencyException(string? message, Exception? inner)
	{
	}

	public DBConcurrencyException(string? message, Exception? inner, DataRow[]? dataRows)
	{
	}

	public void CopyToRows(DataRow[] array)
	{
	}

	public void CopyToRows(DataRow[] array, int arrayIndex)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
