using System.Diagnostics.CodeAnalysis;
using Internal;

namespace System.Runtime.CompilerServices;

public static class RuntimeHelpers
{
	public delegate void CleanupCode(object? userData, bool exceptionThrown);

	public delegate void TryCode(object? userData);

	[Obsolete("OffsetToStringData has been deprecated. Use string.GetPinnableReference() instead.")]
	public static int OffsetToStringData
	{
		get
		{
			throw null;
		}
	}

	public static IntPtr AllocateTypeAssociatedMemory(Type type, int size)
	{
		throw null;
	}

	public static ReadOnlySpan<T> CreateSpan<T>(RuntimeFieldHandle fldHandle)
	{
		throw null;
	}

	public static void EnsureSufficientExecutionStack()
	{
	}

	public new static bool Equals(object? o1, object? o2) => Impl.RuntimeHelpers.Equals(o1, o2);

	[Obsolete("The Constrained Execution Region (CER) feature is not supported.", DiagnosticId = "SYSLIB0004", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static void ExecuteCodeWithGuaranteedCleanup(TryCode code, CleanupCode backoutCode, object? userData)
	{
	}

	public static int GetHashCode(object? o) => Impl.RuntimeHelpers.GetHashCode(o);

	[return: NotNullIfNotNull("obj")]
	public static object? GetObjectValue(object? obj)
	{
		throw null;
	}

	public static T[] GetSubArray<T>(T[] array, Range range)
	{
		throw null;
	}

	public static object GetUninitializedObject([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type type)
	{
		throw null;
	}

	public static void InitializeArray(Array array, RuntimeFieldHandle fldHandle)
		=> Impl.RuntimeHelpers.InitializeArray(array, fldHandle);

	public static bool IsReferenceOrContainsReferences<T>() => Impl.RuntimeHelpers.IsReferenceOrContainsReferences<T>();

	[Obsolete("The Constrained Execution Region (CER) feature is not supported.", DiagnosticId = "SYSLIB0004", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static void PrepareConstrainedRegions()
	{
	}

	[Obsolete("The Constrained Execution Region (CER) feature is not supported.", DiagnosticId = "SYSLIB0004", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static void PrepareConstrainedRegionsNoOP()
	{
	}

	[Obsolete("The Constrained Execution Region (CER) feature is not supported.", DiagnosticId = "SYSLIB0004", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static void PrepareContractedDelegate(Delegate d)
	{
	}

	public static void PrepareDelegate(Delegate d)
	{
	}

	public static void PrepareMethod(RuntimeMethodHandle method)
	{
	}

	public static void PrepareMethod(RuntimeMethodHandle method, RuntimeTypeHandle[]? instantiation)
	{
	}

	[Obsolete("The Constrained Execution Region (CER) feature is not supported.", DiagnosticId = "SYSLIB0004", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static void ProbeForSufficientStack()
	{
	}

	[RequiresUnreferencedCode("Trimmer can't guarantee existence of class constructor")]
	public static void RunClassConstructor(RuntimeTypeHandle type)
	{
	}

	public static void RunModuleConstructor(ModuleHandle module)
	{
	}

	public static bool TryEnsureSufficientExecutionStack()
	{
		throw null;
	}
}
