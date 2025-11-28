using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Reflection.Emit;

public abstract class MethodBuilder : MethodInfo
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

	public override bool ContainsGenericParameters
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

	public override bool IsGenericMethod
	{
		get
		{
			throw null;
		}
	}

	public override bool IsGenericMethodDefinition
	{
		get
		{
			throw null;
		}
	}

	public override bool IsSecurityCritical
	{
		get
		{
			throw null;
		}
	}

	public override bool IsSecuritySafeCritical
	{
		get
		{
			throw null;
		}
	}

	public override bool IsSecurityTransparent
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

	public override ParameterInfo ReturnParameter
	{
		get
		{
			throw null;
		}
	}

	public override Type ReturnType
	{
		get
		{
			throw null;
		}
	}

	public override ICustomAttributeProvider ReturnTypeCustomAttributes
	{
		get
		{
			throw null;
		}
	}

	public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
	{
		throw null;
	}

	protected abstract GenericTypeParameterBuilder[] DefineGenericParametersCore(params string[] names);

	public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string? strParamName)
	{
		throw null;
	}

	protected abstract ParameterBuilder DefineParameterCore(int position, ParameterAttributes attributes, string? strParamName);

	public override bool Equals(object? obj)
	{
		throw null;
	}

	public override MethodInfo GetBaseDefinition()
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

	public override Type[] GetGenericArguments()
	{
		throw null;
	}

	public override MethodInfo GetGenericMethodDefinition()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public ILGenerator GetILGenerator()
	{
		throw null;
	}

	public ILGenerator GetILGenerator(int size)
	{
		throw null;
	}

	protected abstract ILGenerator GetILGeneratorCore(int size);

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

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw null;
	}

	[RequiresDynamicCode("The native code for this instantiation might not be available at runtime.")]
	[RequiresUnreferencedCode("If some of the generic arguments are annotated (either with DynamicallyAccessedMembersAttribute, or generic constraints), trimming can't validate that the requirements of those annotations are met.")]
	public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
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

	public void SetParameters(params Type[] parameterTypes)
	{
	}

	public void SetReturnType(Type? returnType)
	{
	}

	public void SetSignature(Type? returnType, Type[]? returnTypeRequiredCustomModifiers, Type[]? returnTypeOptionalCustomModifiers, Type[]? parameterTypes, Type[][]? parameterTypeRequiredCustomModifiers, Type[][]? parameterTypeOptionalCustomModifiers)
	{
	}

	protected abstract void SetSignatureCore(Type? returnType, Type[]? returnTypeRequiredCustomModifiers, Type[]? returnTypeOptionalCustomModifiers, Type[]? parameterTypes, Type[][]? parameterTypeRequiredCustomModifiers, Type[][]? parameterTypeOptionalCustomModifiers);

	public override string ToString()
	{
		throw null;
	}
}
