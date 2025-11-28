using System.Globalization;

namespace System.Reflection.Emit;

public abstract class ConstructorBuilder : ConstructorInfo
{
	public override MethodAttributes Attributes
	{
		get
		{
			throw null;
		}
	}

	public override CallingConventions CallingConvention
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

	public bool InitLocals
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected abstract bool InitLocalsCore { get; set; }

	public override int MetadataToken
	{
		get
		{
			throw null;
		}
	}

	public override RuntimeMethodHandle MethodHandle
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

	public ParameterBuilder DefineParameter(int iSequence, ParameterAttributes attributes, string? strParamName)
	{
		throw null;
	}

	protected abstract ParameterBuilder DefineParameterCore(int iSequence, ParameterAttributes attributes, string strParamName);

	public override object[] GetCustomAttributes(bool inherit)
	{
		throw null;
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw null;
	}

	public ILGenerator GetILGenerator()
	{
		throw null;
	}

	public ILGenerator GetILGenerator(int streamSize)
	{
		throw null;
	}

	protected abstract ILGenerator GetILGeneratorCore(int streamSize);

	public override MethodImplAttributes GetMethodImplementationFlags()
	{
		throw null;
	}

	public override ParameterInfo[] GetParameters()
	{
		throw null;
	}

	public override object Invoke(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? parameters, CultureInfo? culture)
	{
		throw null;
	}

	public override object Invoke(BindingFlags invokeAttr, Binder? binder, object?[]? parameters, CultureInfo? culture)
	{
		throw null;
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw null;
	}

	public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
	{
	}

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
	}

	protected abstract void SetCustomAttributeCore(ConstructorInfo con, ReadOnlySpan<byte> binaryAttribute);

	public void SetImplementationFlags(MethodImplAttributes attributes)
	{
	}

	protected abstract void SetImplementationFlagsCore(MethodImplAttributes attributes);

	public override string ToString()
	{
		throw null;
	}
}
