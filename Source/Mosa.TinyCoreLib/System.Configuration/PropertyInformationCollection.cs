using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace System.Configuration;

public sealed class PropertyInformationCollection : NameObjectCollectionBase
{
	public PropertyInformation this[string propertyName]
	{
		get
		{
			throw null;
		}
	}

	internal PropertyInformationCollection()
	{
	}

	public void CopyTo(PropertyInformation[] array, int index)
	{
	}

	public override IEnumerator GetEnumerator()
	{
		throw null;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
