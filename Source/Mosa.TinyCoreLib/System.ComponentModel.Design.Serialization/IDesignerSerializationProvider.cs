namespace System.ComponentModel.Design.Serialization;

public interface IDesignerSerializationProvider
{
	object? GetSerializer(IDesignerSerializationManager manager, object? currentSerializer, Type? objectType, Type serializerType);
}
