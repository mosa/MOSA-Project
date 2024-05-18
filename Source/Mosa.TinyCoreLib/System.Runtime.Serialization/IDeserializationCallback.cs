namespace System.Runtime.Serialization;

public interface IDeserializationCallback
{
	void OnDeserialization(object? sender);
}
