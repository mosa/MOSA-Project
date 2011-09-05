
using Mosa.Test.Cases.FIX.IL;
using Mosa.Test.Cases.OLD.IL;

namespace Mosa.Test.Debug
{
	class Program
	{
		public static void Main()
		{
			ConditionalOperator s = new ConditionalOperator();

			s.CompareEqualI1((sbyte)0x02, (sbyte)0x02, (sbyte)0x05, (sbyte)0x01);
			s.CompareEqualI1((sbyte)0x60, (sbyte)0x00, (sbyte)0x60, (sbyte)0x06);
			s.CompareEqualI1((sbyte)1, (sbyte)0, (sbyte)1, (sbyte)2);
			s.CompareEqualI1((sbyte)2, (sbyte)2, (sbyte)2, (sbyte)1);

			Add add = new Add();

			add.AddConstantCLeft('a', 'Z');
			add.AddConstantCLeft('a', 'Z');
			add.AddConstantCLeft('a', 'Z');

			return;
		}
	}
}
