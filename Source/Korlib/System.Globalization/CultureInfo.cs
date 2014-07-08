/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace System.Globalization
{
	/// <summary>
	/// Provides information about a specific culture (called a locale for unmanaged code development).
	/// The information includes the names for the culture, the writing system, the calendar used, and formatting for dates and sort strings.
	/// </summary>
	[Serializable]
	public class CultureInfo : ICloneable, IFormatProvider
	{
		// TODO
		public object Clone()
		{
			throw new NotImplementedException();
		}

		public object GetFormat(Type formatType)
		{
			throw new NotImplementedException();
		}
	}
}