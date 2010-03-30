using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    [TestFixture]
    public class SZArrayFixture : CodeDomTestRunner
    {
        private const string TestCode = @"
            public static class SZArrayTest
            {
                public static bool CompileNewarr()
                {
                    int[] arr = new int[5];
                    return arr != null;
                }

                public static bool AllocatesArrayOfCorrectLength(int length)
                {
                    int[] arr = new int[length];
                    return arr.Length == length;
                }

                public static bool CanGetSetArrayElements(int value)
                {
                    int[] arr = new int[2];
                    arr[0] = value;
                    return value == arr[0];
                }

                public static bool GetAndSetRandomArrayElements(int index, int value)
                {
                    int[] arr = new int[6];
                    arr[index] = value;
                    return value == arr[index];
                }
            }
        " + Code.ObjectClassDefinition;

        public delegate bool B();
        public delegate bool B_I(int length);
        public delegate bool B_I_I(int index, int length);

        [SetUp]
        public void SetUp()
        {
            this.CodeSource = TestCode;
        }

        [Test]
        public void MustCompileNewarrInstruction()
        {
            bool result = (bool)this.Run<B>(String.Empty, @"SZArrayTest", @"CompileNewarr");
            Assert.IsTrue(result);
        }

        [Test]
        public void MustAllocateArrayOfCorrectLength([Column(0, 1, 5, 25)] int length)
        {
            bool result = (bool)this.Run<B_I>(String.Empty, @"SZArrayTest", @"AllocatesArrayOfCorrectLength", length);
            Assert.IsTrue(result);
        }

        [Test]
        public void MustSetAndGetArrayElementZero([Column(Int32.MinValue, -1, 0, 1, Int32.MaxValue)] int value)
        {
            bool result = (bool)this.Run<B_I>(String.Empty, @"SZArrayTest", @"CanGetSetArrayElements", value);
            Assert.IsTrue(result);
        }

        [Test]
        public void MustGetAndSetRandomArrayElements([Column(1, 5)]int index, [Column(Int32.MinValue, -1, 0, 1, Int32.MaxValue)] int value)
        {
            bool result = (bool)this.Run<B_I_I>(String.Empty, @"SZArrayTest", @"GetAndSetRandomArrayElements", index, value);
            Assert.IsTrue(result);
        }
    }
}
