namespace System.Reflection.Emit;

public sealed class SignatureHelper
{
	internal SignatureHelper()
	{
	}

	public void AddArgument(Type clsArgument)
	{
	}

	public void AddArgument(Type argument, bool pinned)
	{
	}

	public void AddArgument(Type argument, Type[]? requiredCustomModifiers, Type[]? optionalCustomModifiers)
	{
	}

	public void AddArguments(Type[]? arguments, Type[][]? requiredCustomModifiers, Type[][]? optionalCustomModifiers)
	{
	}

	public void AddSentinel()
	{
	}

	public override bool Equals(object? obj)
	{
		throw null;
	}

	public static SignatureHelper GetFieldSigHelper(Module? mod)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static SignatureHelper GetLocalVarSigHelper()
	{
		throw null;
	}

	public static SignatureHelper GetLocalVarSigHelper(Module? mod)
	{
		throw null;
	}

	public static SignatureHelper GetMethodSigHelper(CallingConventions callingConvention, Type? returnType)
	{
		throw null;
	}

	public static SignatureHelper GetMethodSigHelper(Module? mod, CallingConventions callingConvention, Type? returnType)
	{
		throw null;
	}

	public static SignatureHelper GetMethodSigHelper(Module? mod, Type? returnType, Type[]? parameterTypes)
	{
		throw null;
	}

	public static SignatureHelper GetPropertySigHelper(Module? mod, CallingConventions callingConvention, Type? returnType, Type[]? requiredReturnTypeCustomModifiers, Type[]? optionalReturnTypeCustomModifiers, Type[]? parameterTypes, Type[][]? requiredParameterTypeCustomModifiers, Type[][]? optionalParameterTypeCustomModifiers)
	{
		throw null;
	}

	public static SignatureHelper GetPropertySigHelper(Module? mod, Type? returnType, Type[]? parameterTypes)
	{
		throw null;
	}

	public static SignatureHelper GetPropertySigHelper(Module? mod, Type? returnType, Type[]? requiredReturnTypeCustomModifiers, Type[]? optionalReturnTypeCustomModifiers, Type[]? parameterTypes, Type[][]? requiredParameterTypeCustomModifiers, Type[][]? optionalParameterTypeCustomModifiers)
	{
		throw null;
	}

	public byte[] GetSignature()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
