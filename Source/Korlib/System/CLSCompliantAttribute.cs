/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace System
{
	/// <summary>
	/// Implementation of the "CLSCompliantAttribute" class.
	/// </summary>
	public sealed class CLSCompliantAttribute : Attribute
	{
		bool is_compliant;

		public CLSCompliantAttribute(bool isCompliant)
		{
			this.is_compliant = isCompliant;
		}

		public bool IsCompliant
		{
			get
			{
				return is_compliant;
			}
		}
	}
}
