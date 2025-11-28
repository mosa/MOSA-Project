using System.ComponentModel;

namespace System.Data;

[Editor("Microsoft.VSDesigner.Data.Design.DataViewRowStateEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[Flags]
public enum DataViewRowState
{
	None = 0,
	Unchanged = 2,
	Added = 4,
	Deleted = 8,
	ModifiedCurrent = 0x10,
	CurrentRows = 0x16,
	ModifiedOriginal = 0x20,
	OriginalRows = 0x2A
}
