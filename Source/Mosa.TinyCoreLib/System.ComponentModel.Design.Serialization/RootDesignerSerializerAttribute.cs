namespace System.ComponentModel.Design.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
[Obsolete("RootDesignerSerializerAttribute has been deprecated. Use DesignerSerializerAttribute instead. For example, to specify a root designer for CodeDom, use DesignerSerializerAttribute(...,typeof(TypeCodeDomSerializer)) instead.")]
public sealed class RootDesignerSerializerAttribute : Attribute
{
	public bool Reloadable
	{
		get
		{
			throw null;
		}
	}

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

	public RootDesignerSerializerAttribute(string? serializerTypeName, string? baseSerializerTypeName, bool reloadable)
	{
	}

	public RootDesignerSerializerAttribute(string serializerTypeName, Type baseSerializerType, bool reloadable)
	{
	}

	public RootDesignerSerializerAttribute(Type serializerType, Type baseSerializerType, bool reloadable)
	{
	}
}
