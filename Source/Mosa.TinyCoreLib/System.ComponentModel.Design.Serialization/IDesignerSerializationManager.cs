using System.Collections;

namespace System.ComponentModel.Design.Serialization;

public interface IDesignerSerializationManager : IServiceProvider
{
	ContextStack Context { get; }

	PropertyDescriptorCollection Properties { get; }

	event ResolveNameEventHandler ResolveName;

	event EventHandler SerializationComplete;

	void AddSerializationProvider(IDesignerSerializationProvider provider);

	object CreateInstance(Type type, ICollection? arguments, string? name, bool addToContainer);

	object? GetInstance(string name);

	string? GetName(object value);

	object? GetSerializer(Type? objectType, Type serializerType);

	Type? GetType(string typeName);

	void RemoveSerializationProvider(IDesignerSerializationProvider provider);

	void ReportError(object errorInformation);

	void SetName(object instance, string name);
}
