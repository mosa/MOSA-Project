namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
[CustomMarshaller(typeof(CustomMarshallerAttribute.GenericPlaceholder[]), MarshalMode.Default, typeof(ArrayMarshaller<, >))]
[CustomMarshaller(typeof(CustomMarshallerAttribute.GenericPlaceholder[]), MarshalMode.ManagedToUnmanagedIn, typeof(ArrayMarshaller<, >.ManagedToUnmanagedIn))]
[ContiguousCollectionMarshaller]
public static class ArrayMarshaller<T, TUnmanagedElement> where TUnmanagedElement : unmanaged
{
	public ref struct ManagedToUnmanagedIn
	{
		private object _dummy;

		private int _dummyPrimitive;

		public static int BufferSize
		{
			get
			{
				throw null;
			}
		}

		public void FromManaged(T[]? array, Span<TUnmanagedElement> buffer)
		{
		}

		public ReadOnlySpan<T> GetManagedValuesSource()
		{
			throw null;
		}

		public Span<TUnmanagedElement> GetUnmanagedValuesDestination()
		{
			throw null;
		}

		public ref TUnmanagedElement GetPinnableReference()
		{
			throw null;
		}

		public static ref T GetPinnableReference(T[]? array)
		{
			throw null;
		}

		public unsafe TUnmanagedElement* ToUnmanaged()
		{
			throw null;
		}

		public void Free()
		{
		}
	}

	public unsafe static TUnmanagedElement* AllocateContainerForUnmanagedElements(T[]? managed, out int numElements)
	{
		throw null;
	}

	public static ReadOnlySpan<T> GetManagedValuesSource(T[]? managed)
	{
		throw null;
	}

	public unsafe static Span<TUnmanagedElement> GetUnmanagedValuesDestination(TUnmanagedElement* unmanaged, int numElements)
	{
		throw null;
	}

	public unsafe static T[]? AllocateContainerForManagedElements(TUnmanagedElement* unmanaged, int numElements)
	{
		throw null;
	}

	public static Span<T> GetManagedValuesDestination(T[]? managed)
	{
		throw null;
	}

	public unsafe static ReadOnlySpan<TUnmanagedElement> GetUnmanagedValuesSource(TUnmanagedElement* unmanagedValue, int numElements)
	{
		throw null;
	}

	public unsafe static void Free(TUnmanagedElement* unmanaged)
	{
	}
}
