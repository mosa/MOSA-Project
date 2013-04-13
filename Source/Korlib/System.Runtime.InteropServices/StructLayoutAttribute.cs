/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public sealed class StructLayoutAttribute : Attribute
	{
		private LayoutKind lkind;
		public int Pack = 8;
		public int Size = 0;

		public StructLayoutAttribute(short layoutKind)
		{
			lkind = (LayoutKind)layoutKind;
		}

		public StructLayoutAttribute(LayoutKind layoutKind)
		{
			lkind = layoutKind;
		}

		public LayoutKind Value
		{
			get { return lkind; }
		}
	}
}