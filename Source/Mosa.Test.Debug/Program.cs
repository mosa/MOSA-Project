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
			Add add = new Add();

			add.AddU4(0x32, 0x32);
			
			add.AddU4(0x32, 0x32);

			Int8Fixture byteFixture = new Int8Fixture();

			byteFixture.Newarr();

			return;
		}
	}
}
