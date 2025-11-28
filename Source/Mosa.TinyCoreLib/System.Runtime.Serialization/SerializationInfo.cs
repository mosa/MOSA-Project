namespace System.Runtime.Serialization;

public sealed class SerializationInfo
{
	public string AssemblyName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string FullTypeName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsAssemblyNameSetExplicit
	{
		get
		{
			throw null;
		}
	}

	public bool IsFullTypeNameSetExplicit
	{
		get
		{
			throw null;
		}
	}

	public int MemberCount
	{
		get
		{
			throw null;
		}
	}

	public Type ObjectType
	{
		get
		{
			throw null;
		}
	}

	public static DeserializationToken StartDeserialization()
	{
		throw null;
	}

	[CLSCompliant(false)]
	[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public SerializationInfo(Type type, IFormatterConverter converter)
	{
	}

	[CLSCompliant(false)]
	[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public SerializationInfo(Type type, IFormatterConverter converter, bool requireSameTokenInPartialTrust)
	{
	}

	public void AddValue(string name, bool value)
	{
	}

	public void AddValue(string name, byte value)
	{
	}

	public void AddValue(string name, char value)
	{
	}

	public void AddValue(string name, DateTime value)
	{
	}

	public void AddValue(string name, decimal value)
	{
	}

	public void AddValue(string name, double value)
	{
	}

	public void AddValue(string name, short value)
	{
	}

	public void AddValue(string name, int value)
	{
	}

	public void AddValue(string name, long value)
	{
	}

	public void AddValue(string name, object? value)
	{
	}

	public void AddValue(string name, object? value, Type type)
	{
	}

	[CLSCompliant(false)]
	public void AddValue(string name, sbyte value)
	{
	}

	public void AddValue(string name, float value)
	{
	}

	[CLSCompliant(false)]
	public void AddValue(string name, ushort value)
	{
	}

	[CLSCompliant(false)]
	public void AddValue(string name, uint value)
	{
	}

	[CLSCompliant(false)]
	public void AddValue(string name, ulong value)
	{
	}

	public bool GetBoolean(string name)
	{
		throw null;
	}

	public byte GetByte(string name)
	{
		throw null;
	}

	public char GetChar(string name)
	{
		throw null;
	}

	public DateTime GetDateTime(string name)
	{
		throw null;
	}

	public decimal GetDecimal(string name)
	{
		throw null;
	}

	public double GetDouble(string name)
	{
		throw null;
	}

	public SerializationInfoEnumerator GetEnumerator()
	{
		throw null;
	}

	public short GetInt16(string name)
	{
		throw null;
	}

	public int GetInt32(string name)
	{
		throw null;
	}

	public long GetInt64(string name)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public sbyte GetSByte(string name)
	{
		throw null;
	}

	public float GetSingle(string name)
	{
		throw null;
	}

	public string? GetString(string name)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public ushort GetUInt16(string name)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public uint GetUInt32(string name)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public ulong GetUInt64(string name)
	{
		throw null;
	}

	public object? GetValue(string name, Type type)
	{
		throw null;
	}

	public void SetType(Type type)
	{
	}
}
