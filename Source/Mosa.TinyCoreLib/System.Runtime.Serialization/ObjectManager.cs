using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.Runtime.Serialization;

[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public class ObjectManager
{
	public ObjectManager(ISurrogateSelector? selector, StreamingContext context)
	{
	}

	[RequiresUnreferencedCode("ObjectManager is not trim compatible because the type of objects being managed cannot be statically discovered.")]
	public virtual void DoFixups()
	{
	}

	public virtual object? GetObject(long objectID)
	{
		throw null;
	}

	public virtual void RaiseDeserializationEvent()
	{
	}

	[RequiresUnreferencedCode("ObjectManager is not trim compatible because the type of objects being managed cannot be statically discovered.")]
	public void RaiseOnDeserializingEvent(object obj)
	{
	}

	public virtual void RecordArrayElementFixup(long arrayToBeFixed, int index, long objectRequired)
	{
	}

	public virtual void RecordArrayElementFixup(long arrayToBeFixed, int[] indices, long objectRequired)
	{
	}

	public virtual void RecordDelayedFixup(long objectToBeFixed, string memberName, long objectRequired)
	{
	}

	public virtual void RecordFixup(long objectToBeFixed, MemberInfo member, long objectRequired)
	{
	}

	[RequiresUnreferencedCode("ObjectManager is not trim compatible because the type of objects being managed cannot be statically discovered.")]
	public virtual void RegisterObject(object obj, long objectID)
	{
	}

	[RequiresUnreferencedCode("ObjectManager is not trim compatible because the type of objects being managed cannot be statically discovered.")]
	public void RegisterObject(object obj, long objectID, SerializationInfo info)
	{
	}

	[RequiresUnreferencedCode("ObjectManager is not trim compatible because the type of objects being managed cannot be statically discovered.")]
	public void RegisterObject(object obj, long objectID, SerializationInfo? info, long idOfContainingObj, MemberInfo? member)
	{
	}

	[RequiresUnreferencedCode("ObjectManager is not trim compatible because the type of objects being managed cannot be statically discovered.")]
	public void RegisterObject(object obj, long objectID, SerializationInfo? info, long idOfContainingObj, MemberInfo? member, int[]? arrayIndex)
	{
	}
}
