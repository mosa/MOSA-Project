namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
internal sealed class NonVersionableAttribute : Attribute;
