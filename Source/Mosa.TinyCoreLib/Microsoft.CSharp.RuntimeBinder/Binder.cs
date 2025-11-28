using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Microsoft.CSharp.RuntimeBinder;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class Binder
{
	[RequiresUnreferencedCode("Using dynamic types might cause types or members to be removed by trimmer.")]
	public static CallSiteBinder BinaryOperation(CSharpBinderFlags flags, ExpressionType operation, Type? context, IEnumerable<CSharpArgumentInfo>? argumentInfo)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Using dynamic types might cause types or members to be removed by trimmer.")]
	public static CallSiteBinder Convert(CSharpBinderFlags flags, Type type, Type? context)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Using dynamic types might cause types or members to be removed by trimmer.")]
	public static CallSiteBinder GetIndex(CSharpBinderFlags flags, Type? context, IEnumerable<CSharpArgumentInfo>? argumentInfo)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Using dynamic types might cause types or members to be removed by trimmer.")]
	public static CallSiteBinder GetMember(CSharpBinderFlags flags, string name, Type? context, IEnumerable<CSharpArgumentInfo>? argumentInfo)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Using dynamic types might cause types or members to be removed by trimmer.")]
	public static CallSiteBinder Invoke(CSharpBinderFlags flags, Type? context, IEnumerable<CSharpArgumentInfo>? argumentInfo)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Using dynamic types might cause types or members to be removed by trimmer.")]
	public static CallSiteBinder InvokeConstructor(CSharpBinderFlags flags, Type? context, IEnumerable<CSharpArgumentInfo>? argumentInfo)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Using dynamic types might cause types or members to be removed by trimmer.")]
	public static CallSiteBinder InvokeMember(CSharpBinderFlags flags, string name, IEnumerable<Type>? typeArguments, Type? context, IEnumerable<CSharpArgumentInfo>? argumentInfo)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Using dynamic types might cause types or members to be removed by trimmer.")]
	public static CallSiteBinder IsEvent(CSharpBinderFlags flags, string name, Type? context)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Using dynamic types might cause types or members to be removed by trimmer.")]
	public static CallSiteBinder SetIndex(CSharpBinderFlags flags, Type? context, IEnumerable<CSharpArgumentInfo>? argumentInfo)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Using dynamic types might cause types or members to be removed by trimmer.")]
	public static CallSiteBinder SetMember(CSharpBinderFlags flags, string name, Type? context, IEnumerable<CSharpArgumentInfo>? argumentInfo)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Using dynamic types might cause types or members to be removed by trimmer.")]
	public static CallSiteBinder UnaryOperation(CSharpBinderFlags flags, ExpressionType operation, Type? context, IEnumerable<CSharpArgumentInfo>? argumentInfo)
	{
		throw null;
	}
}
