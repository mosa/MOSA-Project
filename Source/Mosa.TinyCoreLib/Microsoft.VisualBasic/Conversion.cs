using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic;

[StandardModule]
public sealed class Conversion
{
	internal Conversion()
	{
	}

	[RequiresUnreferencedCode("The Expression's underlying type cannot be statically analyzed and its members may be trimmed")]
	public static object CTypeDynamic(object? Expression, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type TargetType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Expression's underlying type cannot be statically analyzed and its members may be trimmed")]
	public static TargetType CTypeDynamic<TargetType>(object? Expression)
	{
		throw null;
	}

	public static string ErrorToString()
	{
		throw null;
	}

	public static string ErrorToString(int ErrorNumber)
	{
		throw null;
	}

	public static decimal Fix(decimal Number)
	{
		throw null;
	}

	public static double Fix(double Number)
	{
		throw null;
	}

	public static short Fix(short Number)
	{
		throw null;
	}

	public static int Fix(int Number)
	{
		throw null;
	}

	public static long Fix(long Number)
	{
		throw null;
	}

	public static object Fix(object Number)
	{
		throw null;
	}

	public static float Fix(float Number)
	{
		throw null;
	}

	public static string Hex(byte Number)
	{
		throw null;
	}

	public static string Hex(short Number)
	{
		throw null;
	}

	public static string Hex(int Number)
	{
		throw null;
	}

	public static string Hex(long Number)
	{
		throw null;
	}

	public static string Hex(object Number)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string Hex(sbyte Number)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string Hex(ushort Number)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string Hex(uint Number)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string Hex(ulong Number)
	{
		throw null;
	}

	public static decimal Int(decimal Number)
	{
		throw null;
	}

	public static double Int(double Number)
	{
		throw null;
	}

	public static short Int(short Number)
	{
		throw null;
	}

	public static int Int(int Number)
	{
		throw null;
	}

	public static long Int(long Number)
	{
		throw null;
	}

	public static object Int(object Number)
	{
		throw null;
	}

	public static float Int(float Number)
	{
		throw null;
	}

	public static string Oct(byte Number)
	{
		throw null;
	}

	public static string Oct(short Number)
	{
		throw null;
	}

	public static string Oct(int Number)
	{
		throw null;
	}

	public static string Oct(long Number)
	{
		throw null;
	}

	public static string Oct(object Number)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string Oct(sbyte Number)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string Oct(ushort Number)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string Oct(uint Number)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string Oct(ulong Number)
	{
		throw null;
	}

	public static string Str(object Number)
	{
		throw null;
	}

	public static int Val(char Expression)
	{
		throw null;
	}

	public static double Val(object? Expression)
	{
		throw null;
	}

	public static double Val(string? InputStr)
	{
		throw null;
	}
}
