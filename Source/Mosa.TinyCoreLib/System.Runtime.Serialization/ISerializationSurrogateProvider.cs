namespace System.Runtime.Serialization;

public interface ISerializationSurrogateProvider
{
	object GetDeserializedObject(object obj, Type targetType);

	object GetObjectToSerialize(object obj, Type targetType);

	Type GetSurrogateType(Type type);
}
