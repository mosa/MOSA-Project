namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
[CustomMarshaller(typeof(string), MarshalMode.Default, typeof(Utf8StringMarshaller))]
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToUnmanagedIn))]
public static class Utf8StringMarshaller
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

		public unsafe byte* ToUnmanaged()
		{
			throw null;
		}

		public void Free()
		{
			throw null;
		}
	}

	public unsafe static byte* ConvertToUnmanaged(string? managed)
	{
		throw null;
	}

	public unsafe static string? ConvertToManaged(byte* unmanaged)
	{
		throw null;
	}

	public unsafe static void Free(byte* unmanaged)
	{
		throw null;
	}
}
