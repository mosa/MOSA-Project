using System.Diagnostics.CodeAnalysis;

namespace System.Data;

public interface IDataRecord
{
	int FieldCount { get; }

	object this[int i] { get; }

	object this[string name] { get; }

	bool GetBoolean(int i);

	byte GetByte(int i);

	long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferoffset, int length);

	char GetChar(int i);

	long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length);

	IDataReader GetData(int i);

	string GetDataTypeName(int i);

	DateTime GetDateTime(int i);

	decimal GetDecimal(int i);

	double GetDouble(int i);

	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
	Type GetFieldType(int i);

	float GetFloat(int i);

	Guid GetGuid(int i);

	short GetInt16(int i);

	int GetInt32(int i);

	long GetInt64(int i);

	string GetName(int i);

	int GetOrdinal(string name);

	string GetString(int i);

	object GetValue(int i);

	int GetValues(object[] values);

	bool IsDBNull(int i);
}
