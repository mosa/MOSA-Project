using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Test.Quick
{
	class Test
	{
		enum MARKER
		{
			TRY_CATCH_FINALLY_BEGIN = 0x1111,
			TRY_BEGIN = 0x2222,
			TRY_END = 0x3333,
			CATCH_BEGIN = 0x4444,
			CATCH_END = 0x5555,
			FINALLY_BEGIN = 0x6666,
			FINALLY_END = 0x7777,
			TRY_CATCH_FINALLY_END = 0x8888
		}

		private static MARKER Path
		{
			set
			{
				//Console.WriteLine(value);
				return;
			}
		}

		private static void ThrowException()
		{
			throw new Exception("");
		}

		static void Main()
		{
			Path = MARKER.TRY_CATCH_FINALLY_BEGIN;

			try
			{
				Path = MARKER.TRY_BEGIN;

				ThrowException();

				Path = MARKER.TRY_END;
			}
			catch (Exception e)
			{
				Path = MARKER.CATCH_BEGIN;

				Path = MARKER.CATCH_END;
			}

			finally
			{
				Path = MARKER.FINALLY_BEGIN;
				Path = MARKER.FINALLY_END;

				//Console.ReadLine();
			}

			Path = MARKER.TRY_CATCH_FINALLY_END;
		}
	}
}