using System.Globalization;

namespace System.Reflection.Emit;

public abstract class FieldBuilder : FieldInfo
{
	public override FieldAttributes Attributes
	{
		get
		{
			throw null;
		}
	}

	public override Type? DeclaringType
	{
		get
		{
			throw null;
		}
	}

	public override RuntimeFieldHandle FieldHandle
	{
		get
		{
			throw null;
		}
	}

	public override Type FieldType
	{
		get
		{
			throw null;
		}
	}

	public override int MetadataToken
	{
		get
		{
			throw null;
		}
	}

	public override Module Module
	{
		get
		{
			throw null;
		}
	}

	public override string Name
	{
		get
		{
			throw null;
		}
	}

	public override Type? ReflectedType
	{
		get
		{
			throw null;
		}
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		throw null;
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw null;
	}

	public override object? GetValue(object? obj)
	{
		throw null;
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw null;
	}

	public void SetConstant(object? defaultValue)
	{
	}

	protected abstract void SetConstantCore(object? defaultValue);

	public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
	{
	}

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
	}

	protected abstract void SetCustomAttributeCore(ConstructorInfo con, ReadOnlySpan<byte> binaryAttribute);

	public void SetOffset(int iOffset)
	{
	}

	protected abstract void SetOffsetCore(int iOffset);

	public override void SetValue(object? obj, object? val, BindingFlags invokeAttr, Binder? binder, CultureInfo? culture)
	{
	}
}
