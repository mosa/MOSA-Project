namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
[CustomMarshaller(typeof(string), MarshalMode.Default, typeof(BStrStringMarshaller))]
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToUnmanagedIn))]
public static class BStrStringMarshaller
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public ref struct ManagedToUnmanagedIn
	{
		public static int BufferSize
		{
			get
			{
				throw null;
			}
		}

		public void FromManaged(string? managed, Span<byte> buffer)
		{
			throw null;
		}

		public unsafe ushort* ToUnmanaged()
		{
			throw null;
		}

		public void Free()
		{
			throw null;
		}
	}

	public unsafe static ushort* ConvertToUnmanaged(string? managed)
	{
		throw null;
	}

	public unsafe static string? ConvertToManaged(ushort* unmanaged)
	{
		throw null;
	}

	public unsafe static void Free(ushort* unmanaged)
	{
		throw null;
	}
}
