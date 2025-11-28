namespace System.Runtime.InteropServices.Marshalling;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
[CLSCompliant(false)]
public sealed class ComExposedClassAttribute<T> : Attribute, IComExposedDetails where T : IComExposedClass
{
	public unsafe ComWrappers.ComInterfaceEntry* GetComInterfaceEntries(out int count)
	{
		throw null;
	}
}
