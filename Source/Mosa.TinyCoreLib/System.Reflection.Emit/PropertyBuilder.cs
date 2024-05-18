using System.Globalization;

namespace System.Reflection.Emit;

public abstract class PropertyBuilder : PropertyInfo
{
	public override PropertyAttributes Attributes
	{
		get
		{
			throw null;
		}
	}

	public override bool CanRead
	{
		get
		{
			throw null;
		}
	}

	public override bool CanWrite
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

	public override Type PropertyType
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

	public void AddOtherMethod(MethodBuilder mdBuilder)
	{
	}

	protected abstract void AddOtherMethodCore(MethodBuilder mdBuilder);

	public override MethodInfo[] GetAccessors(bool nonPublic)
	{
		throw null;
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		throw null;
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw null;
	}

	public override MethodInfo? GetGetMethod(bool nonPublic)
	{
		throw null;
	}

	public override ParameterInfo[] GetIndexParameters()
	{
		throw null;
	}

	public override MethodInfo? GetSetMethod(bool nonPublic)
	{
		throw null;
	}

	public override object GetValue(object? obj, object?[]? index)
	{
		throw null;
	}

	public override object GetValue(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture)
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

	public void SetGetMethod(MethodBuilder mdBuilder)
	{
	}

	protected abstract void SetGetMethodCore(MethodBuilder mdBuilder);

	public void SetSetMethod(MethodBuilder mdBuilder)
	{
	}

	protected abstract void SetSetMethodCore(MethodBuilder mdBuilder);

	public override void SetValue(object? obj, object? value, object?[]? index)
	{
	}

	public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture)
	{
	}
}
