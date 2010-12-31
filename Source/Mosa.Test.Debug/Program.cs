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
			StringFixture s = new StringFixture();

			s.FirstCharacterMustMatchInStrings();

			return;
		}
	}
}
