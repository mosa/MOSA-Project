// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics.Contracts;

namespace System
{
	public interface IFormattable
	{
		[Pure]
		String ToString(String format, IFormatProvider formatProvider);
	}
}
