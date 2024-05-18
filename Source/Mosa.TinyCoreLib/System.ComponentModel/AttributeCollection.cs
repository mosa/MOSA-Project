using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

public class AttributeCollection : ICollection, IEnumerable
{
	public static readonly AttributeCollection Empty;

	protected virtual Attribute[] Attributes
	{
		get
		{
			throw null;
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public virtual Attribute this[int index]
	{
		get
		{
			throw null;
		}
	}

	public virtual Attribute? this[[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type attributeType]
	{
		get
		{
			throw null;
		}
	}

	int ICollection.Count
	{
		get
		{
			throw null;
		}
	}

	bool ICollection.IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	object ICollection.SyncRoot
	{
		get
		{
			throw null;
		}
	}

	protected AttributeCollection()
	{
	}

	public AttributeCollection(params Attribute[]? attributes)
	{
	}

	[RequiresUnreferencedCode("The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	public bool Contains(Attribute? attribute)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	public bool Contains(Attribute[]? attributes)
	{
		throw null;
	}

	public void CopyTo(Array array, int index)
	{
	}

	public static AttributeCollection FromExisting(AttributeCollection existing, params Attribute[]? newAttributes)
	{
		throw null;
	}

	protected Attribute? GetDefaultAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type attributeType)
	{
		throw null;
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public bool Matches(Attribute? attribute)
	{
		throw null;
	}

	public bool Matches(Attribute[]? attributes)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
