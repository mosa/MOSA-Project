using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Reflection;

[CLSCompliant(false)]
public sealed class Pointer : ISerializable
{
	internal Pointer()
	{
	}

	public unsafe static object Box(void* ptr, Type type)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public unsafe static void* Unbox(object ptr)
	{
		throw null;
	}
}
