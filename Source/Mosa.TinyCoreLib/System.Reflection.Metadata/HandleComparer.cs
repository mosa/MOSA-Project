using System.Collections.Generic;

namespace System.Reflection.Metadata;

public sealed class HandleComparer : IComparer<EntityHandle>, IComparer<Handle>, IEqualityComparer<EntityHandle>, IEqualityComparer<Handle>
{
	public static HandleComparer Default
	{
		get
		{
			throw null;
		}
	}

	internal HandleComparer()
	{
	}

	public int Compare(EntityHandle x, EntityHandle y)
	{
		throw null;
	}

	public int Compare(Handle x, Handle y)
	{
		throw null;
	}

	public bool Equals(EntityHandle x, EntityHandle y)
	{
		throw null;
	}

	public bool Equals(Handle x, Handle y)
	{
		throw null;
	}

	public int GetHashCode(EntityHandle obj)
	{
		throw null;
	}

	public int GetHashCode(Handle obj)
	{
		throw null;
	}
}
