namespace System;

[CLSCompliant(false)]
public interface IConvertible
{
	TypeCode GetTypeCode();

	bool ToBoolean(IFormatProvider? provider);

	byte ToByte(IFormatProvider? provider);

	char ToChar(IFormatProvider? provider);

	DateTime ToDateTime(IFormatProvider? provider);

	decimal ToDecimal(IFormatProvider? provider);

	double ToDouble(IFormatProvider? provider);

	short ToInt16(IFormatProvider? provider);

	int ToInt32(IFormatProvider? provider);

	long ToInt64(IFormatProvider? provider);

	sbyte ToSByte(IFormatProvider? provider);

	float ToSingle(IFormatProvider? provider);

	string ToString(IFormatProvider? provider);

	object ToType(Type conversionType, IFormatProvider? provider);

	ushort ToUInt16(IFormatProvider? provider);

	uint ToUInt32(IFormatProvider? provider);

	ulong ToUInt64(IFormatProvider? provider);
}
