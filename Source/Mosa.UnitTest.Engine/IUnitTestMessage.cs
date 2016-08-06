// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.UnitTest.Engine
{
	internal interface IUnitTestMessage
	{
		int DebugCode { get; }

		IList<int> MessageAsInts { get; }

		IList<byte> MessageAsBytes { get; }
	}
}
