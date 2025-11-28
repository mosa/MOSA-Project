namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
[CustomMarshaller(typeof(ReadOnlySpan<>), MarshalMode.ManagedToUnmanagedIn, typeof(ReadOnlySpanMarshaller<, >.ManagedToUnmanagedIn))]
[CustomMarshaller(typeof(ReadOnlySpan<>), MarshalMode.UnmanagedToManagedOut, typeof(ReadOnlySpanMarshaller<, >.UnmanagedToManagedOut))]
[ContiguousCollectionMarshaller]
public static class ReadOnlySpanMarshaller<T, TUnmanagedElement> where TUnmanagedElement : unmanaged
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

		public void FromManaged(ReadOnlySpan<T> managed, Span<TUnmanagedElement> buffer)
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

		public static ref T GetPinnableReference(ReadOnlySpan<T> managed)
		{
			throw null;
		}
	}

	public static class UnmanagedToManagedOut
	{
		public unsafe static TUnmanagedElement* AllocateContainerForUnmanagedElements(ReadOnlySpan<T> managed, out int numElements)
		{
			throw null;
		}

		public static ReadOnlySpan<T> GetManagedValuesSource(ReadOnlySpan<T> managed)
		{
			throw null;
		}

		public unsafe static Span<TUnmanagedElement> GetUnmanagedValuesDestination(TUnmanagedElement* unmanaged, int numElements)
		{
			throw null;
		}
	}
}
