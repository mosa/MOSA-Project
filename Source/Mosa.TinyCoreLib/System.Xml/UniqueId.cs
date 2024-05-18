using System.Diagnostics.CodeAnalysis;

namespace System.Xml;

public class UniqueId
{
	public int CharArrayLength
	{
		get
		{
			throw null;
		}
	}

	public bool IsGuid
	{
		get
		{
			throw null;
		}
	}

	public UniqueId()
	{
	}

	public UniqueId(byte[] guid)
	{
	}

	public UniqueId(byte[] guid, int offset)
	{
	}

	public UniqueId(char[] chars, int offset, int count)
	{
	}

	public UniqueId(Guid guid)
	{
	}

	public UniqueId(string value)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(UniqueId? id1, UniqueId? id2)
	{
		throw null;
	}

	public static bool operator !=(UniqueId? id1, UniqueId? id2)
	{
		throw null;
	}

	public int ToCharArray(char[] chars, int offset)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public bool TryGetGuid(byte[] buffer, int offset)
	{
		throw null;
	}

	public bool TryGetGuid(out Guid guid)
	{
		throw null;
	}
}
