// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	/// Implementation of the "CLSCompliantAttribute" class.
	/// </summary>
	public sealed class CLSCompliantAttribute : Attribute
	{
		private bool is_compliant;

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