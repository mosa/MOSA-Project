
using Mosa.Test.Cases.CIL;
using Mosa.Test.Compiler.Framework;

namespace Mosa.Test.Debug
{
	class Program
	{
		public static void Main()
		{

			//SimpleFastDominanceTests.DominanceCalculation1();
			//SimpleFastDominanceTests.DominanceCalculation2();

			//ConditionalOperator s = new ConditionalOperator();
			//s.CompareEqualI1((sbyte)0x02, (sbyte)0x02, (sbyte)0x05, (sbyte)0x01);
			//s.CompareEqualI1((sbyte)0x60, (sbyte)0x00, (sbyte)0x60, (sbyte)0x06);
			//s.CompareEqualI1((sbyte)1, (sbyte)0, (sbyte)1, (sbyte)2);
			//s.CompareEqualI1((sbyte)2, (sbyte)2, (sbyte)2, (sbyte)1);

			//Add add = new Add();
			//add.AddConstantCLeft('a', 'Z');
			//add.AddConstantCLeft('a', 'Z');
			//add.AddConstantCLeft('a', 'Z');

			CallFixture call = new CallFixture();
			call.CallU1((byte)1);

			//Int32Fixture int32Fixture = new Int32Fixture();
			//BooleanFixture booleanFixture = new BooleanFixture();
			//CallVirtualFixture callVirtualFixture = new CallVirtualFixture();

			//for (int i = 0; i < 2000; i++)
			//{
			//    int32Fixture.Ldlen(i);
			//    call.CallI4(i);
			//    call.CallU4((uint)i);
			//    int32Fixture.Newarr();
			//    int32Fixture.LdelemaI4(i, i);

			//    booleanFixture.Ldlen(i);
			//    booleanFixture.Newarr();
			//    booleanFixture.LdelemB(i, true);
			//    booleanFixture.LdelemaB(i, true);
			//    booleanFixture.StelemB(i, true);

			//    callVirtualFixture.TestBaseCall();
			//    callVirtualFixture.TestVirtualCall();
			//}
			
			ComparisonFixture Comparison = new ComparisonFixture();
			Comparison.CompareEqualI2I8(-1, -1);

			return;
		}
	}
}
