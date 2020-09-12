using System;
using System.Diagnostics.Contracts;

namespace System.System
{
	public interface IFormattable
	{
		[Pure]
		String ToString(String format, IFormatProvider formatProvider);
	}
}
