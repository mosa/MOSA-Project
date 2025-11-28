namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
[CustomMarshaller(typeof(string), MarshalMode.Default, typeof(Utf16StringMarshaller))]
public static class Utf16StringMarshaller
{
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

	public static ref readonly char GetPinnableReference(string? str)
	{
		throw null;
	}
}
