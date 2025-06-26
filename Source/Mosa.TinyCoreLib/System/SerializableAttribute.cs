using System.ComponentModel;

namespace System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate, Inherited = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class SerializableAttribute : Attribute;