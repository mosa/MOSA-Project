using System.Collections;

namespace System.Security.AccessControl;

public abstract class GenericAcl : ICollection, IEnumerable
{
	public static readonly byte AclRevision;

	public static readonly byte AclRevisionDS;

	public static readonly int MaxBinaryLength;

	public abstract int BinaryLength { get; }

	public abstract int Count { get; }

	public bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public abstract GenericAce this[int index] { get; set; }

	public abstract byte Revision { get; }

	public virtual object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public void CopyTo(GenericAce[] array, int index)
	{
	}

	public abstract void GetBinaryForm(byte[] binaryForm, int offset);

	public AceEnumerator GetEnumerator()
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
