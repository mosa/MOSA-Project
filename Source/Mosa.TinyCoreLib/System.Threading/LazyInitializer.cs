using System.Diagnostics.CodeAnalysis;

namespace System.Threading;

public static class LazyInitializer
{
	public static T EnsureInitialized<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>([NotNull] ref T? target) where T : class
	{
		throw null;
	}

	public static T EnsureInitialized<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>([AllowNull] ref T target, ref bool initialized, [NotNullIfNotNull("syncLock")] ref object? syncLock)
	{
		throw null;
	}

	public static T EnsureInitialized<T>([AllowNull] ref T target, ref bool initialized, [NotNullIfNotNull("syncLock")] ref object? syncLock, Func<T> valueFactory)
	{
		throw null;
	}

	public static T EnsureInitialized<T>([NotNull] ref T? target, Func<T> valueFactory) where T : class
	{
		throw null;
	}

	public static T EnsureInitialized<T>([NotNull] ref T? target, [NotNullIfNotNull("syncLock")] ref object? syncLock, Func<T> valueFactory) where T : class
	{
		throw null;
	}
}
