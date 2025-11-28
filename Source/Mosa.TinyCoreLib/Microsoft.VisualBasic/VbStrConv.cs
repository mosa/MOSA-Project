using System;

namespace Microsoft.VisualBasic;

[Flags]
public enum VbStrConv
{
	None = 0,
	Uppercase = 1,
	Lowercase = 2,
	ProperCase = 3,
	Wide = 4,
	Narrow = 8,
	Katakana = 0x10,
	Hiragana = 0x20,
	SimplifiedChinese = 0x100,
	TraditionalChinese = 0x200,
	LinguisticCasing = 0x400
}
