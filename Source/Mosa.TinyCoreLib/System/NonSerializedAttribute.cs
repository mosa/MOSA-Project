using System.ComponentModel;

namespace System;

[AttributeUsage(AttributeTargets.Field, Inherited = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class NonSerializedAttribute : Attribute
{
}
