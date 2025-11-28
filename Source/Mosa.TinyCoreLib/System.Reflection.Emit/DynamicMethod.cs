using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Reflection.Emit;

public sealed class DynamicMethod : MethodInfo
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

	[RequiresDynamicCode("Creating a DynamicMethod requires dynamic code.")]
	public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes, Module m, bool skipVisibility)
	{
	}

	[RequiresDynamicCode("Creating a DynamicMethod requires dynamic code.")]
	public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes, Type owner, bool skipVisibility)
	{
	}

	[RequiresDynamicCode("Creating a DynamicMethod requires dynamic code.")]
	public DynamicMethod(string name, Type? returnType, Type[]? parameterTypes)
	{
	}

	[RequiresDynamicCode("Creating a DynamicMethod requires dynamic code.")]
	public DynamicMethod(string name, Type? returnType, Type[]? parameterTypes, bool restrictedSkipVisibility)
	{
	}

	[RequiresDynamicCode("Creating a DynamicMethod requires dynamic code.")]
	public DynamicMethod(string name, Type? returnType, Type[]? parameterTypes, Module m)
	{
	}

	[RequiresDynamicCode("Creating a DynamicMethod requires dynamic code.")]
	public DynamicMethod(string name, Type? returnType, Type[]? parameterTypes, Module m, bool skipVisibility)
	{
	}

	[RequiresDynamicCode("Creating a DynamicMethod requires dynamic code.")]
	public DynamicMethod(string name, Type? returnType, Type[]? parameterTypes, Type owner)
	{
	}

	[RequiresDynamicCode("Creating a DynamicMethod requires dynamic code.")]
	public DynamicMethod(string name, Type? returnType, Type[]? parameterTypes, Type owner, bool skipVisibility)
	{
	}

	public sealed override Delegate CreateDelegate(Type delegateType)
	{
		throw null;
	}

	public sealed override Delegate CreateDelegate(Type delegateType, object? target)
	{
		throw null;
	}

	public ParameterBuilder? DefineParameter(int position, ParameterAttributes attributes, string? parameterName)
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

	public DynamicILInfo GetDynamicILInfo()
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

	public override MethodImplAttributes GetMethodImplementationFlags()
	{
		throw null;
	}

	public override ParameterInfo[] GetParameters()
	{
		throw null;
	}

	public override object? Invoke(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? parameters, CultureInfo? culture)
	{
		throw null;
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
