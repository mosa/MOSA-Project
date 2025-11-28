using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Versioning;

namespace System.ComponentModel;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class MaskedTextProvider : ICloneable
{
	public bool AllowPromptAsInput
	{
		get
		{
			throw null;
		}
	}

	public bool AsciiOnly
	{
		get
		{
			throw null;
		}
	}

	public int AssignedEditPositionCount
	{
		get
		{
			throw null;
		}
	}

	public int AvailableEditPositionCount
	{
		get
		{
			throw null;
		}
	}

	public CultureInfo Culture
	{
		get
		{
			throw null;
		}
	}

	public static char DefaultPasswordChar
	{
		get
		{
			throw null;
		}
	}

	public int EditPositionCount
	{
		get
		{
			throw null;
		}
	}

	public IEnumerator EditPositions
	{
		get
		{
			throw null;
		}
	}

	public bool IncludeLiterals
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IncludePrompt
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static int InvalidIndex
	{
		get
		{
			throw null;
		}
	}

	public bool IsPassword
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public char this[int index]
	{
		get
		{
			throw null;
		}
	}

	public int LastAssignedPosition
	{
		get
		{
			throw null;
		}
	}

	public int Length
	{
		get
		{
			throw null;
		}
	}

	public string Mask
	{
		get
		{
			throw null;
		}
	}

	public bool MaskCompleted
	{
		get
		{
			throw null;
		}
	}

	public bool MaskFull
	{
		get
		{
			throw null;
		}
	}

	public char PasswordChar
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public char PromptChar
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ResetOnPrompt
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ResetOnSpace
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool SkipLiterals
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MaskedTextProvider(string mask)
	{
	}

	public MaskedTextProvider(string mask, bool restrictToAscii)
	{
	}

	public MaskedTextProvider(string mask, char passwordChar, bool allowPromptAsInput)
	{
	}

	public MaskedTextProvider(string mask, CultureInfo? culture)
	{
	}

	public MaskedTextProvider(string mask, CultureInfo? culture, bool restrictToAscii)
	{
	}

	public MaskedTextProvider(string mask, CultureInfo? culture, bool allowPromptAsInput, char promptChar, char passwordChar, bool restrictToAscii)
	{
	}

	public MaskedTextProvider(string mask, CultureInfo? culture, char passwordChar, bool allowPromptAsInput)
	{
	}

	public bool Add(char input)
	{
		throw null;
	}

	public bool Add(char input, out int testPosition, out MaskedTextResultHint resultHint)
	{
		throw null;
	}

	public bool Add(string input)
	{
		throw null;
	}

	public bool Add(string input, out int testPosition, out MaskedTextResultHint resultHint)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public void Clear(out MaskedTextResultHint resultHint)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public object Clone()
	{
		throw null;
	}

	public int FindAssignedEditPositionFrom(int position, bool direction)
	{
		throw null;
	}

	public int FindAssignedEditPositionInRange(int startPosition, int endPosition, bool direction)
	{
		throw null;
	}

	public int FindEditPositionFrom(int position, bool direction)
	{
		throw null;
	}

	public int FindEditPositionInRange(int startPosition, int endPosition, bool direction)
	{
		throw null;
	}

	public int FindNonEditPositionFrom(int position, bool direction)
	{
		throw null;
	}

	public int FindNonEditPositionInRange(int startPosition, int endPosition, bool direction)
	{
		throw null;
	}

	public int FindUnassignedEditPositionFrom(int position, bool direction)
	{
		throw null;
	}

	public int FindUnassignedEditPositionInRange(int startPosition, int endPosition, bool direction)
	{
		throw null;
	}

	public static bool GetOperationResultFromHint(MaskedTextResultHint hint)
	{
		throw null;
	}

	public bool InsertAt(char input, int position)
	{
		throw null;
	}

	public bool InsertAt(char input, int position, out int testPosition, out MaskedTextResultHint resultHint)
	{
		throw null;
	}

	public bool InsertAt(string input, int position)
	{
		throw null;
	}

	public bool InsertAt(string input, int position, out int testPosition, out MaskedTextResultHint resultHint)
	{
		throw null;
	}

	public bool IsAvailablePosition(int position)
	{
		throw null;
	}

	public bool IsEditPosition(int position)
	{
		throw null;
	}

	public static bool IsValidInputChar(char c)
	{
		throw null;
	}

	public static bool IsValidMaskChar(char c)
	{
		throw null;
	}

	public static bool IsValidPasswordChar(char c)
	{
		throw null;
	}

	public bool Remove()
	{
		throw null;
	}

	public bool Remove(out int testPosition, out MaskedTextResultHint resultHint)
	{
		throw null;
	}

	public bool RemoveAt(int position)
	{
		throw null;
	}

	public bool RemoveAt(int startPosition, int endPosition)
	{
		throw null;
	}

	public bool RemoveAt(int startPosition, int endPosition, out int testPosition, out MaskedTextResultHint resultHint)
	{
		throw null;
	}

	public bool Replace(char input, int position)
	{
		throw null;
	}

	public bool Replace(char input, int startPosition, int endPosition, out int testPosition, out MaskedTextResultHint resultHint)
	{
		throw null;
	}

	public bool Replace(char input, int position, out int testPosition, out MaskedTextResultHint resultHint)
	{
		throw null;
	}

	public bool Replace(string input, int position)
	{
		throw null;
	}

	public bool Replace(string input, int startPosition, int endPosition, out int testPosition, out MaskedTextResultHint resultHint)
	{
		throw null;
	}

	public bool Replace(string input, int position, out int testPosition, out MaskedTextResultHint resultHint)
	{
		throw null;
	}

	public bool Set(string input)
	{
		throw null;
	}

	public bool Set(string input, out int testPosition, out MaskedTextResultHint resultHint)
	{
		throw null;
	}

	public string ToDisplayString()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public string ToString(bool ignorePasswordChar)
	{
		throw null;
	}

	public string ToString(bool includePrompt, bool includeLiterals)
	{
		throw null;
	}

	public string ToString(bool ignorePasswordChar, bool includePrompt, bool includeLiterals, int startPosition, int length)
	{
		throw null;
	}

	public string ToString(bool includePrompt, bool includeLiterals, int startPosition, int length)
	{
		throw null;
	}

	public string ToString(bool ignorePasswordChar, int startPosition, int length)
	{
		throw null;
	}

	public string ToString(int startPosition, int length)
	{
		throw null;
	}

	public bool VerifyChar(char input, int position, out MaskedTextResultHint hint)
	{
		throw null;
	}

	public bool VerifyEscapeChar(char input, int position)
	{
		throw null;
	}

	public bool VerifyString(string input)
	{
		throw null;
	}

	public bool VerifyString(string input, out int testPosition, out MaskedTextResultHint resultHint)
	{
		throw null;
	}
}
