namespace System.Runtime.CompilerServices;

public static class RuntimeFeature
{
	public const string ByRefFields = "ByRefFields";

	public const string CovariantReturnsOfClasses = "CovariantReturnsOfClasses";

	public const string DefaultImplementationsOfInterfaces = "DefaultImplementationsOfInterfaces";

	public const string NumericIntPtr = "NumericIntPtr";

	public const string PortablePdb = "PortablePdb";

	public const string UnmanagedSignatureCallingConvention = "UnmanagedSignatureCallingConvention";

	public const string VirtualStaticsInInterfaces = "VirtualStaticsInInterfaces";

	public static bool IsDynamicCodeCompiled
	{
		get
		{
			throw null;
		}
	}

	public static bool IsDynamicCodeSupported
	{
		get
		{
			throw null;
		}
	}

	public static bool IsSupported(string feature)
	{
		throw null;
	}
}
