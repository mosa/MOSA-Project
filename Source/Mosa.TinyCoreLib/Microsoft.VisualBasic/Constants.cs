using System;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic;

[StandardModule]
public sealed class Constants
{
	public const MsgBoxResult vbAbort = MsgBoxResult.Abort;

	public const MsgBoxStyle vbAbortRetryIgnore = MsgBoxStyle.AbortRetryIgnore;

	public const MsgBoxStyle vbApplicationModal = MsgBoxStyle.ApplicationModal;

	public const FileAttribute vbArchive = FileAttribute.Archive;

	public const VariantType vbArray = VariantType.Array;

	public const string vbBack = "\b";

	public const CompareMethod vbBinaryCompare = CompareMethod.Binary;

	public const VariantType vbBoolean = VariantType.Boolean;

	public const VariantType vbByte = VariantType.Byte;

	public const MsgBoxResult vbCancel = MsgBoxResult.Cancel;

	public const string vbCr = "\r";

	public const MsgBoxStyle vbCritical = MsgBoxStyle.Critical;

	public const string vbCrLf = "\r\n";

	public const VariantType vbCurrency = VariantType.Currency;

	public const VariantType vbDate = VariantType.Date;

	public const VariantType vbDecimal = VariantType.Decimal;

	public const MsgBoxStyle vbDefaultButton1 = MsgBoxStyle.ApplicationModal;

	public const MsgBoxStyle vbDefaultButton2 = MsgBoxStyle.DefaultButton2;

	public const MsgBoxStyle vbDefaultButton3 = MsgBoxStyle.DefaultButton3;

	public const FileAttribute vbDirectory = FileAttribute.Directory;

	public const VariantType vbDouble = VariantType.Double;

	public const VariantType vbEmpty = VariantType.Empty;

	public const MsgBoxStyle vbExclamation = MsgBoxStyle.Exclamation;

	public const TriState vbFalse = TriState.False;

	public const FirstWeekOfYear vbFirstFourDays = FirstWeekOfYear.FirstFourDays;

	public const FirstWeekOfYear vbFirstFullWeek = FirstWeekOfYear.FirstFullWeek;

	public const FirstWeekOfYear vbFirstJan1 = FirstWeekOfYear.Jan1;

	public const string vbFormFeed = "\f";

	public const FirstDayOfWeek vbFriday = FirstDayOfWeek.Friday;

	public const DateFormat vbGeneralDate = DateFormat.GeneralDate;

	public const CallType vbGet = CallType.Get;

	public const FileAttribute vbHidden = FileAttribute.Hidden;

	public const AppWinStyle vbHide = AppWinStyle.Hide;

	public const VbStrConv vbHiragana = VbStrConv.Hiragana;

	public const MsgBoxResult vbIgnore = MsgBoxResult.Ignore;

	public const MsgBoxStyle vbInformation = MsgBoxStyle.Information;

	public const VariantType vbInteger = VariantType.Integer;

	public const VbStrConv vbKatakana = VbStrConv.Katakana;

	public const CallType vbLet = CallType.Let;

	public const string vbLf = "\n";

	public const VbStrConv vbLinguisticCasing = VbStrConv.LinguisticCasing;

	public const VariantType vbLong = VariantType.Long;

	public const DateFormat vbLongDate = DateFormat.LongDate;

	public const DateFormat vbLongTime = DateFormat.LongTime;

	public const VbStrConv vbLowerCase = VbStrConv.Lowercase;

	public const AppWinStyle vbMaximizedFocus = AppWinStyle.MaximizedFocus;

	public const CallType vbMethod = CallType.Method;

	public const AppWinStyle vbMinimizedFocus = AppWinStyle.MinimizedFocus;

	public const AppWinStyle vbMinimizedNoFocus = AppWinStyle.MinimizedNoFocus;

	public const FirstDayOfWeek vbMonday = FirstDayOfWeek.Monday;

	public const MsgBoxStyle vbMsgBoxHelp = MsgBoxStyle.MsgBoxHelp;

	public const MsgBoxStyle vbMsgBoxRight = MsgBoxStyle.MsgBoxRight;

	public const MsgBoxStyle vbMsgBoxRtlReading = MsgBoxStyle.MsgBoxRtlReading;

	public const MsgBoxStyle vbMsgBoxSetForeground = MsgBoxStyle.MsgBoxSetForeground;

	public const VbStrConv vbNarrow = VbStrConv.Narrow;

	[Obsolete("vbNewLine has been deprecated. For a carriage return and line feed, use vbCrLf. For the current platform's newline, use System.Environment.NewLine.")]
	public const string vbNewLine = "\r\n";

	public const MsgBoxResult vbNo = MsgBoxResult.No;

	public const FileAttribute vbNormal = FileAttribute.Normal;

	public const AppWinStyle vbNormalFocus = AppWinStyle.NormalFocus;

	public const AppWinStyle vbNormalNoFocus = AppWinStyle.NormalNoFocus;

	public const VariantType vbNull = VariantType.Null;

	public const string vbNullChar = "\0";

	public const string? vbNullString = null;

	public const VariantType vbObject = VariantType.Object;

	public const int vbObjectError = -2147221504;

	public const MsgBoxResult vbOK = MsgBoxResult.Ok;

	public const MsgBoxStyle vbOKCancel = MsgBoxStyle.OkCancel;

	public const MsgBoxStyle vbOKOnly = MsgBoxStyle.ApplicationModal;

	public const VbStrConv vbProperCase = VbStrConv.ProperCase;

	public const MsgBoxStyle vbQuestion = MsgBoxStyle.Question;

	public const FileAttribute vbReadOnly = FileAttribute.ReadOnly;

	public const MsgBoxResult vbRetry = MsgBoxResult.Retry;

	public const MsgBoxStyle vbRetryCancel = MsgBoxStyle.RetryCancel;

	public const FirstDayOfWeek vbSaturday = FirstDayOfWeek.Saturday;

	public const CallType vbSet = CallType.Set;

	public const DateFormat vbShortDate = DateFormat.ShortDate;

	public const DateFormat vbShortTime = DateFormat.ShortTime;

	public const VbStrConv vbSimplifiedChinese = VbStrConv.SimplifiedChinese;

	public const VariantType vbSingle = VariantType.Single;

	public const VariantType vbString = VariantType.String;

	public const FirstDayOfWeek vbSunday = FirstDayOfWeek.Sunday;

	public const FileAttribute vbSystem = FileAttribute.System;

	public const MsgBoxStyle vbSystemModal = MsgBoxStyle.SystemModal;

	public const string vbTab = "\t";

	public const CompareMethod vbTextCompare = CompareMethod.Text;

	public const FirstDayOfWeek vbThursday = FirstDayOfWeek.Thursday;

	public const VbStrConv vbTraditionalChinese = VbStrConv.TraditionalChinese;

	public const TriState vbTrue = TriState.True;

	public const FirstDayOfWeek vbTuesday = FirstDayOfWeek.Tuesday;

	public const VbStrConv vbUpperCase = VbStrConv.Uppercase;

	public const TriState vbUseDefault = TriState.UseDefault;

	public const VariantType vbUserDefinedType = VariantType.UserDefinedType;

	public const FirstWeekOfYear vbUseSystem = FirstWeekOfYear.System;

	public const FirstDayOfWeek vbUseSystemDayOfWeek = FirstDayOfWeek.System;

	public const VariantType vbVariant = VariantType.Variant;

	public const string vbVerticalTab = "\v";

	public const FileAttribute vbVolume = FileAttribute.Volume;

	public const FirstDayOfWeek vbWednesday = FirstDayOfWeek.Wednesday;

	public const VbStrConv vbWide = VbStrConv.Wide;

	public const MsgBoxResult vbYes = MsgBoxResult.Yes;

	public const MsgBoxStyle vbYesNo = MsgBoxStyle.YesNo;

	public const MsgBoxStyle vbYesNoCancel = MsgBoxStyle.YesNoCancel;

	internal Constants()
	{
	}
}
