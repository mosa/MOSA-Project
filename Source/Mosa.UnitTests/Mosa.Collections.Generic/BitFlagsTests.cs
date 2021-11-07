// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Collections.Generic;

namespace Mosa.UnitTests.Mosa.Collections.Generic
{
	public static class BitFlagsTests
	{
		[MosaUnitTest]
		public static bool Create()
		{
			BitFlags Options = new BitFlags(32, 0xCAFEBABE);

			return (Options != null);
		}

		[MosaUnitTest]
		public static bool Operations()
		{
			BitFlags Options = new BitFlags(32, 0xCAFEBABE);

			bool ResultFlags1 = (Options.GetFlags() == 0xCAFEBABE);
			bool ResultTest = (Options.Test(0) == false);

			Options.Reset(0);
			bool ResultReset = (Options.Test(0) == false);

			Options.Set(0);
			bool ResultSet = (Options.Test(0) == true);

			bool ResultFlags2 = (Options.GetFlags() == 0xCAFEBABF);

			bool ResultException1 = false;
			try
			{
				Options.Test(32);
			}
			catch (CollectionsDataOverflowException BitFlagsException)
			{
				ResultException1 = true;
			}

			bool ResultFinal = ResultFlags1 && ResultTest && ResultReset && ResultSet && ResultFlags2 && ResultException1;

			return ResultFinal;
		}
	}
}

