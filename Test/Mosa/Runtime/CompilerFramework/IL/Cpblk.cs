using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.Reflection.Emit;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Provides test cases for the cpblk IL operation.
    /// </summary>
    [TestFixture]
    public class Cpblk : ReflectionEmitTestRunner
    {
        private void GenerateCpblk(ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Ldc_I4_8);
            generator.Emit(OpCodes.Cpblk);
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Ret);
        }

        private delegate bool I4_I4(int a, int b);

        /// <summary>
        /// Tests the cpblk opcode implementation.
        /// </summary>
        [Test, Author("grover", @"sharpos@michaelruck.de")]
        public void Test_Cpblk()
        {
            this.Generator = new GenerateIL(GenerateCpblk);
            Run<I4_I4>(@"", @"Cpblk", @"TestCpblk", 0, 0);
        }
    }
}
