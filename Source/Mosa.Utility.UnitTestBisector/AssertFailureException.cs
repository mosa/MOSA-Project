// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.UnitTestBisector;

public sealed partial class UnitTestBisectorSystem
{
	private sealed class AssertFailureException : Exception
	{
		public AssertFailureException(string message, string detailMessage)
			: base(string.IsNullOrWhiteSpace(detailMessage) ? message : $"{message} {detailMessage}")
		{
		}
	}
}
