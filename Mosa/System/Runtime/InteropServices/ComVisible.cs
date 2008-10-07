/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace System.Runtime.InteropServices {

	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class
		| AttributeTargets.Struct | AttributeTargets.Enum |
		AttributeTargets.Method | AttributeTargets.Property |
		AttributeTargets.Field | AttributeTargets.Interface |
		AttributeTargets.Delegate, Inherited=false)]
	[ComVisible (true)]
	public sealed class ComVisibleAttribute : Attribute {

		private bool Visible = false;

		public ComVisibleAttribute (bool visibility)
		{
			Visible = visibility;
		}

		public bool Value {
			get { return Visible; }
		}
	}
}
