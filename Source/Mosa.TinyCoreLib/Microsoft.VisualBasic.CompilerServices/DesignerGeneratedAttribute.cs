using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.CompilerServices;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class DesignerGeneratedAttribute : Attribute
{
}
