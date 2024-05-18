using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.VisualBasic.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class LateBinding
{
	internal LateBinding()
	{
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void LateCall(object? o, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? objType, string name, object?[]? args, string?[]? paramnames, bool[]? CopyBack)
	{
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static object? LateGet(object? o, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? objType, string name, object?[]? args, string?[]? paramnames, bool[]? CopyBack)
	{
		throw null;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static object? LateIndexGet(object o, object?[]? args, string?[]? paramnames)
	{
		throw null;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void LateIndexSet(object o, object?[] args, string?[]? paramnames)
	{
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void LateIndexSetComplex(object o, object?[] args, string?[]? paramnames, bool OptimisticSet, bool RValueBase)
	{
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void LateSet(object? o, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? objType, string name, object?[]? args, string?[]? paramnames)
	{
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void LateSetComplex(object? o, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? objType, string name, object?[]? args, string?[]? paramnames, bool OptimisticSet, bool RValueBase)
	{
	}
}
