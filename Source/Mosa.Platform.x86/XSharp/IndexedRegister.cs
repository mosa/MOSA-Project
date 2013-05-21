/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Platform.x86.XSharp
{
	public class IndexedRegister
	{
		protected int index;
		protected Register register;

		protected XSharpMethod XSharpMethod { get { return register.XSharpMethod; } }

		internal Register Register { get { return register; } }

		internal int Index { get { return index; } }

		internal IndexedRegister(Register register, int index)
		{
			this.register = register; this.index = index;
		}

		public object Value
		{
			set
			{
				if (value is Int32)
				{
					XSharpMethod.Store(this, (int)value);
					return;
				}
				else if (value is UInt32)
				{
					XSharpMethod.Store(this, (uint)value);
					return;
				}
				else if (value is Register)
				{
					return;
				}
			}
		}
	}
}