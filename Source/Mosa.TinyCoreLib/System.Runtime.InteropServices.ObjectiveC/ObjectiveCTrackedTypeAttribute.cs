using System.Runtime.Versioning;

namespace System.Runtime.InteropServices.ObjectiveC;

[SupportedOSPlatform("macos")]
[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class ObjectiveCTrackedTypeAttribute : Attribute
{
}
