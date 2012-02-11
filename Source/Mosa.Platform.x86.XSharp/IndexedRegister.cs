using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Platform.x86.XSharp
{


	public class IndexedRegister
	{
		protected int index;
		protected Register register;

		protected XSharpMethod XSharpMethod { get { return register.XSharpMethod; } }
		internal Register Register { get { return register; } }
		internal int Index { get { return index; } }

		internal IndexedRegister(Register register, int index) { this.register = register; this.index = index; }

		public int Value
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
