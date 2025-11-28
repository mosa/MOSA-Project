namespace System.Runtime.Serialization;

public abstract class SerializationBinder
{
	public virtual void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
	{
		throw null;
	}

	public abstract Type? BindToType(string assemblyName, string typeName);
}
