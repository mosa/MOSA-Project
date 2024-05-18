using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace System.Runtime.Serialization;

[Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", DiagnosticId = "SYSLIB0011", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[CLSCompliant(false)]
public abstract class Formatter : IFormatter
{
	protected ObjectIDGenerator m_idGenerator;

	protected Queue m_objectQueue;

	public abstract SerializationBinder? Binder { get; set; }

	public abstract StreamingContext Context { get; set; }

	public abstract ISurrogateSelector? SurrogateSelector { get; set; }

	[RequiresDynamicCode("BinaryFormatter serialization uses dynamic code generation, the type of objects being processed cannot be statically discovered.")]
	[RequiresUnreferencedCode("BinaryFormatter serialization is not trim compatible because the type of objects being processed cannot be statically discovered.")]
	public abstract object Deserialize(Stream serializationStream);

	protected virtual object? GetNext(out long objID)
	{
		throw null;
	}

	protected virtual long Schedule(object? obj)
	{
		throw null;
	}

	[RequiresUnreferencedCode("BinaryFormatter serialization is not trim compatible because the type of objects being processed cannot be statically discovered.")]
	public abstract void Serialize(Stream serializationStream, object graph);

	protected abstract void WriteArray(object obj, string name, Type memberType);

	protected abstract void WriteBoolean(bool val, string name);

	protected abstract void WriteByte(byte val, string name);

	protected abstract void WriteChar(char val, string name);

	protected abstract void WriteDateTime(DateTime val, string name);

	protected abstract void WriteDecimal(decimal val, string name);

	protected abstract void WriteDouble(double val, string name);

	protected abstract void WriteInt16(short val, string name);

	protected abstract void WriteInt32(int val, string name);

	protected abstract void WriteInt64(long val, string name);

	protected virtual void WriteMember(string memberName, object? data)
	{
	}

	protected abstract void WriteObjectRef(object? obj, string name, Type memberType);

	[CLSCompliant(false)]
	protected abstract void WriteSByte(sbyte val, string name);

	protected abstract void WriteSingle(float val, string name);

	protected abstract void WriteTimeSpan(TimeSpan val, string name);

	[CLSCompliant(false)]
	protected abstract void WriteUInt16(ushort val, string name);

	[CLSCompliant(false)]
	protected abstract void WriteUInt32(uint val, string name);

	[CLSCompliant(false)]
	protected abstract void WriteUInt64(ulong val, string name);

	protected abstract void WriteValueType(object obj, string name, Type memberType);
}
