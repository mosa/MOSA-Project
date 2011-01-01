using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mosa.Test.Cases.IL;
using Mosa.Test.Cases.CIL;

namespace Mosa.Test.Debug
{
	class Program
	{
		public static void Main()
		{
			ConditionalOperator s = new ConditionalOperator();

			s.EQ_I1(0x02, 0x02, 0x00, 0x01);
			s.EQ_U1(0xff, 0x00, 0xff, 0x00);
			s.EQ_U2(1, 0, 1, 0);
			s.EQ_U8(2, 2, 0, 1);

			return;
		}
	}
}
