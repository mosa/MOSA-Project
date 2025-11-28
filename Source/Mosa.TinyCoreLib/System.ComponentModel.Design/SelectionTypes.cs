namespace System.ComponentModel.Design;

[Flags]
public enum SelectionTypes
{
	Auto = 1,
	[Obsolete("SelectionTypes.Normal has been deprecated. Use SelectionTypes.Auto instead.")]
	Normal = 1,
	Replace = 2,
	[Obsolete("SelectionTypes.MouseDown has been deprecated and is not supported.")]
	MouseDown = 4,
	[Obsolete("SelectionTypes.MouseUp has been deprecated and is not supported.")]
	MouseUp = 8,
	[Obsolete("SelectionTypes.Click has been deprecated. Use SelectionTypes.Primary instead.")]
	Click = 0x10,
	Primary = 0x10,
	[Obsolete("SelectionTypes.Valid has been deprecated. Use Enum class methods to determine valid values, or use a type converter instead.")]
	Valid = 0x1F,
	Toggle = 0x20,
	Add = 0x40,
	Remove = 0x80
}
