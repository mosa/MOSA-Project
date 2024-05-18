namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
[CustomMarshaller(typeof(Span<>), MarshalMode.Default, typeof(SpanMarshaller<, >))]
[CustomMarshaller(typeof(Span<>), MarshalMode.ManagedToUnmanagedIn, typeof(SpanMarshaller<, >.ManagedToUnmanagedIn))]
[ContiguousCollectionMarshaller]
public static class SpanMarshaller<T, TUnmanagedElement> where TUnmanagedElement : unmanaged
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

		public void FromManaged(Span<T> managed, Span<TUnmanagedElement> buffer)
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

		public unsafe TUnmanagedElement* ToUnmanaged()
		{
			throw null;
		}

		public void Free()
		{
		}

		public static ref T GetPinnableReference(Span<T> managed)
		{
			throw null;
		}
	}

	public unsafe static TUnmanagedElement* AllocateContainerForUnmanagedElements(Span<T> managed, out int numElements)
	{
		throw null;
	}

	public static ReadOnlySpan<T> GetManagedValuesSource(Span<T> managed)
	{
		throw null;
	}

	public unsafe static Span<TUnmanagedElement> GetUnmanagedValuesDestination(TUnmanagedElement* unmanaged, int numElements)
	{
		throw null;
	}

	public unsafe static Span<T> AllocateContainerForManagedElements(TUnmanagedElement* unmanaged, int numElements)
	{
		throw null;
	}

	public static Span<T> GetManagedValuesDestination(Span<T> managed)
	{
		throw null;
	}

	public unsafe static ReadOnlySpan<TUnmanagedElement> GetUnmanagedValuesSource(TUnmanagedElement* unmanaged, int numElements)
	{
		throw null;
	}

	public unsafe static void Free(TUnmanagedElement* unmanaged)
	{
		throw null;
	}
}
