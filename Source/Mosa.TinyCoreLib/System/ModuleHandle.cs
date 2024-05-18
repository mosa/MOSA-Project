using System.Diagnostics.CodeAnalysis;

namespace System;

public struct ModuleHandle : IEquatable<ModuleHandle>
{
	private object _dummy;

	private int _dummyPrimitive;

	public static readonly ModuleHandle EmptyHandle;

	public int MDStreamVersion
	{
		get
		{
			throw null;
		}
	}

	public bool Equals(ModuleHandle handle)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public RuntimeFieldHandle GetRuntimeFieldHandleFromMetadataToken(int fieldToken)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public RuntimeMethodHandle GetRuntimeMethodHandleFromMetadataToken(int methodToken)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public RuntimeTypeHandle GetRuntimeTypeHandleFromMetadataToken(int typeToken)
	{
		throw null;
	}

	public static bool operator ==(ModuleHandle left, ModuleHandle right)
	{
		throw null;
	}

	public static bool operator !=(ModuleHandle left, ModuleHandle right)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public RuntimeFieldHandle ResolveFieldHandle(int fieldToken)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public RuntimeFieldHandle ResolveFieldHandle(int fieldToken, RuntimeTypeHandle[]? typeInstantiationContext, RuntimeTypeHandle[]? methodInstantiationContext)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public RuntimeMethodHandle ResolveMethodHandle(int methodToken)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public RuntimeMethodHandle ResolveMethodHandle(int methodToken, RuntimeTypeHandle[]? typeInstantiationContext, RuntimeTypeHandle[]? methodInstantiationContext)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public RuntimeTypeHandle ResolveTypeHandle(int typeToken)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public RuntimeTypeHandle ResolveTypeHandle(int typeToken, RuntimeTypeHandle[]? typeInstantiationContext, RuntimeTypeHandle[]? methodInstantiationContext)
	{
		throw null;
	}
}
