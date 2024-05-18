namespace System.ComponentModel.Design.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public sealed class DesignerSerializerAttribute : Attribute
{
	public string? SerializerBaseTypeName
	{
		get
		{
			throw null;
		}
	}

	public string? SerializerTypeName
	{
		get
		{
			throw null;
		}
	}

	public override object TypeId
	{
		get
		{
			throw null;
		}
	}

	public DesignerSerializerAttribute(string? serializerTypeName, string? baseSerializerTypeName)
	{
	}

	public DesignerSerializerAttribute(string? serializerTypeName, Type baseSerializerType)
	{
	}

	public DesignerSerializerAttribute(Type serializerType, Type baseSerializerType)
	{
	}
}
