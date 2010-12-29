using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mosa.Test.Runtime.CompilerFramework.IL;
using Mosa.Test.Runtime.CompilerFramework.CIL;

namespace Mosa.Test.Debug
{
	class Program
	{
		public static void Main()
		{
			Add add = new Add();

			add.AddU4(0x32, 0x32);
			
			add.AddU4(0x32, 0x32);

			ByteFixture byteFixture = new ByteFixture();

			byteFixture.Newarr();

			return;
		}
	}
}
