using System.Collections;

namespace System.ComponentModel.Design.Serialization;

public interface IDesignerSerializationService
{
	ICollection Deserialize(object serializationData);

	object Serialize(ICollection objects);
}
