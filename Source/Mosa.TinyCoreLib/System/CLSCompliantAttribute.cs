namespace System;

[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
public sealed class CLSCompliantAttribute(bool isCompliant) : Attribute
{
	public bool IsCompliant { get; } = isCompliant;
}
