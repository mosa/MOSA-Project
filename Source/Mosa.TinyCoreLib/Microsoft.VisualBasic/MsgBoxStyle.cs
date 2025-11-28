using System;

namespace Microsoft.VisualBasic;

[Flags]
public enum MsgBoxStyle
{
	ApplicationModal = 0,
	DefaultButton1 = 0,
	OkOnly = 0,
	OkCancel = 1,
	AbortRetryIgnore = 2,
	YesNoCancel = 3,
	YesNo = 4,
	RetryCancel = 5,
	Critical = 0x10,
	Question = 0x20,
	Exclamation = 0x30,
	Information = 0x40,
	DefaultButton2 = 0x100,
	DefaultButton3 = 0x200,
	SystemModal = 0x1000,
	MsgBoxHelp = 0x4000,
	MsgBoxSetForeground = 0x10000,
	MsgBoxRight = 0x80000,
	MsgBoxRtlReading = 0x100000
}
