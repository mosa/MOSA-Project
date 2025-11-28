using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data;

public static class DataReaderExtensions
{
	public static bool GetBoolean(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static byte GetByte(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static long GetBytes(this DbDataReader reader, string name, long dataOffset, byte[] buffer, int bufferOffset, int length)
	{
		throw null;
	}

	public static char GetChar(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static long GetChars(this DbDataReader reader, string name, long dataOffset, char[] buffer, int bufferOffset, int length)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static DbDataReader GetData(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static string GetDataTypeName(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static DateTime GetDateTime(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static decimal GetDecimal(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static double GetDouble(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static Type GetFieldType(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static Task<T> GetFieldValueAsync<T>(this DbDataReader reader, string name, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static T GetFieldValue<T>(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static float GetFloat(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static Guid GetGuid(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static short GetInt16(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static int GetInt32(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static long GetInt64(this DbDataReader reader, string name)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Type GetProviderSpecificFieldType(this DbDataReader reader, string name)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static object GetProviderSpecificValue(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static Stream GetStream(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static string GetString(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static TextReader GetTextReader(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static object GetValue(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static bool IsDBNull(this DbDataReader reader, string name)
	{
		throw null;
	}

	public static Task<bool> IsDBNullAsync(this DbDataReader reader, string name, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
