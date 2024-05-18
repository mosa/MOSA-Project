using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.VisualBasic.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class NewLateBinding
{
	internal NewLateBinding()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("FallbackCall has been deprecated and is not supported.", true)]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static object? FallbackCall(object Instance, string MemberName, object[] Arguments, string[] ArgumentNames, bool IgnoreReturn)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("FallbackGet has been deprecated and is not supported.", true)]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static object? FallbackGet(object Instance, string MemberName, object[] Arguments, string[] ArgumentNames)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("FallbackIndexSet has been deprecated and is not supported.", true)]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void FallbackIndexSet(object Instance, object[] Arguments, string[] ArgumentNames)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("FallbackIndexSetComplex has been deprecated and is not supported.", true)]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void FallbackIndexSetComplex(object Instance, object[] Arguments, string[] ArgumentNames, bool OptimisticSet, bool RValueBase)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("FallbackInvokeDefault1 has been deprecated and is not supported.", true)]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static object? FallbackInvokeDefault1(object Instance, object[] Arguments, string[] ArgumentNames, bool ReportErrors)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("FallbackInvokeDefault2 has been deprecated and is not supported.", true)]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static object? FallbackInvokeDefault2(object Instance, object[] Arguments, string[] ArgumentNames, bool ReportErrors)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("FallbackSet has been deprecated and is not supported.", true)]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void FallbackSet(object Instance, string MemberName, object[] Arguments)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("FallbackSetComplex has been deprecated and is not supported.", true)]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void FallbackSetComplex(object Instance, string MemberName, object[] Arguments, bool OptimisticSet, bool RValueBase)
	{
	}

	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static object? LateCall(object? Instance, Type? Type, string MemberName, object?[]? Arguments, string?[]? ArgumentNames, Type?[]? TypeArguments, bool[]? CopyBack, bool IgnoreReturn)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static object? LateCallInvokeDefault(object? Instance, object?[]? Arguments, string?[]? ArgumentNames, bool ReportErrors)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static object? LateGet(object? Instance, Type? Type, string MemberName, object?[]? Arguments, string?[]? ArgumentNames, Type?[]? TypeArguments, bool[]? CopyBack)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static object? LateGetInvokeDefault(object Instance, object?[]? Arguments, string?[]? ArgumentNames, bool ReportErrors)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static object? LateIndexGet(object Instance, object?[]? Arguments, string?[]? ArgumentNames)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void LateIndexSet(object Instance, object?[]? Arguments, string?[]? ArgumentNames)
	{
	}

	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void LateIndexSetComplex(object Instance, object?[]? Arguments, string?[]? ArgumentNames, bool OptimisticSet, bool RValueBase)
	{
	}

	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void LateSet(object? Instance, Type? Type, string MemberName, object?[]? Arguments, string?[]? ArgumentNames, Type[]? TypeArguments)
	{
	}

	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void LateSet(object? Instance, Type? Type, string MemberName, object?[]? Arguments, string?[]? ArgumentNames, Type[]? TypeArguments, bool OptimisticSet, bool RValueBase, CallType CallType)
	{
	}

	[RequiresUnreferencedCode("Late binding is dynamic and cannot be statically analyzed. The referenced types and members may be trimmed")]
	public static void LateSetComplex(object? Instance, Type? Type, string MemberName, object?[]? Arguments, string?[]? ArgumentNames, Type[]? TypeArguments, bool OptimisticSet, bool RValueBase)
	{
	}
}
