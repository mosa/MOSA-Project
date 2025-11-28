using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class HideModuleNameAttribute : Attribute
{
}
