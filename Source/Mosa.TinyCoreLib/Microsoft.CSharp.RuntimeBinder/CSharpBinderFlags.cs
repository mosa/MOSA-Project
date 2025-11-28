using System;
using System.ComponentModel;

namespace Microsoft.CSharp.RuntimeBinder;

[EditorBrowsable(EditorBrowsableState.Never)]
[Flags]
public enum CSharpBinderFlags
{
	None = 0,
	CheckedContext = 1,
	InvokeSimpleName = 2,
	InvokeSpecialName = 4,
	BinaryOperationLogical = 8,
	ConvertExplicit = 0x10,
	ConvertArrayIndex = 0x20,
	ResultIndexed = 0x40,
	ValueFromCompoundAssignment = 0x80,
	ResultDiscarded = 0x100
}
